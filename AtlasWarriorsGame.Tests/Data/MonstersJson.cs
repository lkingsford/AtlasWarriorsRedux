using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasWarriorsGame.Tests.Data
{
    /// <summary>
    /// Tests for verification of monsters.json 
    /// </summary>
    [TestFixture]
    class MonstersJson
    {
        /// <summary>
        /// Verify that monsters.json is valid and successfully loaded by the MonsterFactory/Game
        /// </summary>
        [Test]
        public static void Valid()
        {
            var path = Path.Combine(
                TestContext.CurrentContext.TestDirectory, @"data\monsters.json");
            // Following is copied almost verbatim from Game.cs
            // Load the data from Monsters.Json into the Monster Factory
            using (var monsterFileReader = new System.IO.StreamReader(path))
            {
                var monsterFileText = monsterFileReader.ReadToEnd();
                var deserializedMonsterPrototypes =
                    JsonConvert.DeserializeObject<Dictionary<String, JObject>>(monsterFileText);
                foreach (var prototype in deserializedMonsterPrototypes)
                {
                    MonsterFactory.AddPrototype(prototype.Key, prototype.Value);
                }
            }

            // If we're here - it's valid!
        }
    }
}
