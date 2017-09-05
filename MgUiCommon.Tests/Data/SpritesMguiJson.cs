using MgUiCommon;
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
    /// Tests for verification of sprites_mgui.json 
    /// </summary>
    [TestFixture]
    class SpritesMguiJson
    {
        /// <summary>
        /// Verify that sprites_mgui.json is valid and successfully loaded by the MonsterFactory/Game
        /// </summary>
        [Test]
        public static void Valid()
        {
            var path = Path.Combine(
                TestContext.CurrentContext.TestDirectory, @"data\sprites_mgui.json");
            // Following is copied almost verbatim from MapViewElement.cs
            using (var spriteFileReader = new System.IO.StreamReader(path))
            {
                var spriteFileText = spriteFileReader.ReadToEnd();
                var Sprites = JsonConvert.DeserializeObject<Dictionary<String, ConsoleCell>>
                    (spriteFileText);
            }

            // If we're here - it's valid!
        }

        /// <summary>
        /// Verify that all monsters loaded from monsters.json have sprites in sprites_mgui.json
        /// </summary>
        [Test]
        public static void AllMonstersCovered()
        {
            // Load sprites
            var sprites = new Dictionary<String, ConsoleCell>();
            var spritePath = Path.Combine(
                TestContext.CurrentContext.TestDirectory, @"data\sprites_mgui.json");
            using (var spriteFileReader = new System.IO.StreamReader(spritePath))
            {
                var spriteFileText = spriteFileReader.ReadToEnd();
                sprites = JsonConvert.DeserializeObject<Dictionary<String, ConsoleCell>>
                    (spriteFileText);
            }

            // Load monsters
            var monstersPath = Path.Combine(
                TestContext.CurrentContext.TestDirectory, @"data\monsters.json");
            // Following is copied almost verbatim from Game.cs
            // Load the data from Monsters.Json into the Monster Factory
            using (var monsterFileReader = new System.IO.StreamReader(monstersPath))
            {
                var monsterFileText = monsterFileReader.ReadToEnd();
                var deserializedMonsterPrototypes =
                    JsonConvert.DeserializeObject<Dictionary<String, MonsterFactoryPrototype>>(monsterFileText);
                foreach (var prototype in deserializedMonsterPrototypes)
                {
                    Assert.Contains(prototype.Value.SpriteId, sprites.Keys,
                        "Sprite not found");
                }
            }
        }
    }
}
