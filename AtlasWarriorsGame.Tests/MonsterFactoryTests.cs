using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasWarriorsGame.Tests
{
    /// <summary>
    /// Tests of the MonsterFactory
    /// </summary>
    [TestFixture]
    class MonsterFactoryTests
    {
        // Dungeon for use in tests
        private Dungeon OpenDungeon;

        /// <summary>
        /// Dig OpenDungeon as empty room
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            OpenDungeon = AtlasWarriorsGame.DungeonGenerators.DefaultGenerator.Generate(10, 10);
        }

        /// <summary>
        /// Load a prototype from a string, and create a monster from that prototype
        /// </summary>
        [Test]
        public void LoadPrototypeAndPlaceMonster()
        {
            string monsterJson = "{\"SpriteId\": \"ASSASSIN\", \"Description\": \"Assassin\", \"Health\": 5, \"BaseAtk\": 17, \"BaseDef\": 6, \"BaseDmg\": 15 }";
            MonsterFactory.AddPrototype("TEST1",
                JsonConvert.DeserializeObject<JObject>(monsterJson));
            var testMonster = MonsterFactory.CreateMonster(null, OpenDungeon, "TEST1", 
                new XY(3, 3));

            // Test in dungeon
            Assert.Contains(testMonster, OpenDungeon.Actors, "Monster not added");

            // Test valid results
            Assert.AreEqual("ASSASSIN", testMonster.SpriteId, "SpriteID not set correctly");
            Assert.AreEqual(5, testMonster.MaxHealth, "Health not set correctly");
            Assert.AreEqual(17, testMonster.BaseAtk, "BaseAtk not set correctly");
            Assert.AreEqual(6, testMonster.BaseDef, "BaseDef not set correctly");
            Assert.AreEqual(15, testMonster.BaseDmg, "BaseDmg not set correctly");
        }
    }
}
