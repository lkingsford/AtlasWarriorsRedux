using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasWarriorsGame.Tests
{
    [TestFixture]
    public class ActorTests
    {
        /// <summary>
        /// Dig OpenDungeon as empty room
        /// Dig ClosedDungeon as 3x3 room
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            OpenDungeon = AtlasWarriorsGame.DungeonGenerators.DefaultGenerator.Generate(10, 10);
            ClosedDungeon = AtlasWarriorsGame.DungeonGenerators.DefaultGenerator.Generate(3, 3);
        }

        /// <summary>
        /// Dungeon for testing that can move when walkable
        /// </summary>
        Dungeon OpenDungeon;

        /// <summary>
        /// ... and the one for when not
        /// </summary>
        Dungeon ClosedDungeon;

        /// <summary>
        /// All potential 1 space player movements
        /// </summary>
        static object[] MoveDirections =
        {
            new XY(-1, -1),
            new XY(0, -1),
            new XY(1, -1),
            new XY(-1, 0),
            new XY(0, 0),
            new XY(1, 0),
            new XY(-1, 1),
            new XY(0, 1),
            new XY(1, 1),
        };

        /// <summary>
        /// Verify that in an open space, the character moves in each direction
        /// </summary>
        /// <param name="DxDy">Amount to move</param>
        [Test]
        [TestCaseSource("MoveDirections")]
        public void OpenMove(XY DxDy)
        {
            var actor = new AtlasWarriorsGame.Actor(OpenDungeon);
            var startLocation = actor.Location;
            actor.Move(DxDy);
            Assert.AreEqual(startLocation.X + DxDy.X, actor.Location.X, "X movement incorrect");
            Assert.AreEqual(startLocation.Y + DxDy.Y, actor.Location.Y, "Y movement incorrect");
        }

        /// <summary>
        /// Verify that can't move if can't move
        /// </summary>
        /// <param name="DxDy"></param>
        [Test]
        [TestCaseSource("MoveDirections")]
        public void BlockedMove(XY DxDy)
        {
            var actor = new AtlasWarriorsGame.Actor(ClosedDungeon);
            var startLocation = actor.Location;
            actor.Move(DxDy);
            Assert.AreEqual(startLocation.X, actor.Location.X, "X failed to block");
            Assert.AreEqual(startLocation.Y, actor.Location.Y, "Y failed to block");
        }

        /// <summary>
        /// Heal values for the heal test. They're current, max, amount of healing, and desired 
        /// result
        /// </summary>
        static object[] HealValues =
        {
            new object[] {10, 10, 2, 10 },
            new object[] {5, 10, 2, 7},
            new object[] {-1, 10, 1, 0},
            new object[] {9, 10, 2, 10}
        };

        /// <summary>
        /// Test that heal works - taking max into account
        /// </summary>
        /// <param name="current">Current health</param>
        /// <param name="maxHealth">Maximum health of actor</param>
        /// <param name="healAmount">Amount to heal</param>
        /// <param name="result">Desired result</param>
        [Test]
        [TestCaseSource("HealValues")]
        public void Heal(int current, int maxHealth, int healAmount, int result)
        {
            var a = new AtlasWarriorsGame.Actor();
            a.SetHealth(current);
            a.MaxHealth = maxHealth;
            Assert.AreEqual(result, a.Heal(healAmount), "Healed to incorrect value");
        }

        /// <summary>
        /// Attack values. They're current, amount of injury, after health
        /// </summary>
        static object[] InjureValues =
        {
            new object[] {10, 1, 9},
            new object[] {5, 5, 0 },
            new object[] {1, 10, -9},
            new object[] {-1, 1, -2}
        };

        /// <summary>
        /// Test that injure reduces health - past 0 too
        /// </summary>
        /// <param name="current">Start/Max HP</param>
        /// <param name="damage">Amount to injure</param>
        /// <param name="after">After attack HP</param>
        [Test]
        [TestCaseSource("InjureValues")]
        public void Injure(int current, int damage, int after)
        {
            var a = new AtlasWarriorsGame.Actor();
            a.SetHealth(current);
            a.MaxHealth = current;
            a.Injure(damage);
            Assert.AreEqual(after, a.CurrentHealth, "Injured to incorrect value");
        }

        /// <summary>
        /// Tests that for dead
        /// </summary>
        static object[] DeadValues =
        {
            new object[] {1, false},
            new object[] {0, true},
            new object[] {-1, true}
        };

        /// <summary>
        /// Test that below 1 is dead
        /// </summary>
        /// <param name="current">Start/Max HP</param>
        /// <param name="deadResult">Whether dead should be true</param>
        [Test]
        [TestCaseSource("DeadValues")]
        public void Dead(int current, bool deadResult)
        {
            var a = new AtlasWarriorsGame.Actor();
            a.SetHealth(current);
            a.MaxHealth = 10;
            Assert.AreEqual(deadResult, a.Dead);
        }


        /// <summary>
        /// Data for Attack tests
        /// Start health, attack, damage, defence, attack roll, result
        /// </summary>
        static object[] AttackValues =
        {
            new object[] {10, 10, 2, 8, 1, 8 }, // Hit
            new object[] {8, 5, 5, 8, 1, 8 },   // Miss, due to insufficient roll
            new object[] {8, 5, 5, 8, 5, 3 },   // Hit, due to big enough roll
            new object[] {10, 2, 5, 3, 1, 5 },  // Hit, attack == defence
            new object[] {10, 9, 5, 2, 2, 5 },  // Miss, attack < defence
        };

        /// <summary>
        /// Test the attack
        /// </summary>
        /// <param name="currentHealth">Start health</param>
        /// <param name="atk">Attack stat</param>
        /// <param name="dmg">Damage to cause</param>
        /// <param name="def">Def stat</param>
        /// <param name="roll">Random roll stat</param>
        /// <param name="result">End health</param>
        [Test]
        [TestCaseSource("AttackValues")]
        public void Attack(int currentHealth, int atk, int dmg, int def, int roll, int result)
        {
            var a = new AtlasWarriorsGame.Actor();
            a.BaseAtk = atk;
            a.BaseDmg = dmg;

            var b = new AtlasWarriorsGame.Actor();
            b.BaseDef = def;
            b.SetHealth(currentHealth);

            // Seed roll
            AtlasWarriorsGame.GlobalRandom.Inject(roll);

            // Attack
            a.Attack(b);

            Assert.AreEqual(result, b.CurrentHealth);
        }
    }
}
