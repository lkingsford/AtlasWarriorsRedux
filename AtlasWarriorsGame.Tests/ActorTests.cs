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
            OpenDungeon = new AtlasWarriorsGame.Dungeon(10,10);
            ClosedDungeon = new AtlasWarriorsGame.Dungeon(3,3);
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
        public void TestOpenMove(XY DxDy)
        {
            var actor = new Actor(OpenDungeon);
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
        public void TestBlockedMove(XY DxDy)
        {
            var actor = new Actor(ClosedDungeon);
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
            var a = new Actor();
            a.SetHealth(current);
            a.MaxHealth = maxHealth;
            Assert.AreEqual(result, a.Heal(healAmount), "Healed to incorrect value");
        }
    }
}
