using Newtonsoft.Json;
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
    /// Tests for verification of dungeons.json
    /// </summary>
    [TestFixture]
    class DungeonsJson
    {
        /// <summary>
        /// Verify that dungeons.json is valid and successfully loaded by the MonsterFactory/Game
        /// Verify that dungeons.json has a single dungeon with a one START passage
        /// </summary>
        [Test]
        public static void Valid()
        {
            var path = Path.Combine(
                TestContext.CurrentContext.TestDirectory, @"data\dungeons.json");

            Dictionary<string, DungeonPrototype> dungeonsToDig;
            using (var dungeonsFileReader = new System.IO.StreamReader(path))
            {
                var dungeonsFileText = dungeonsFileReader.ReadToEnd();
                dungeonsToDig = JsonConvert.DeserializeObject<Dictionary<string, DungeonPrototype>>
                    (dungeonsFileText);
            }

            // Verify that 1 START passage
            Assert.AreEqual(1, dungeonsToDig.SelectMany(i => i.Value.Passages)
                              .Where(i => i.DestinationID == "START")
                              .Count(),
                              "Wrong amount of START passages");

            // Verify each dungeon
            foreach (var i in dungeonsToDig)
            {
                // That generator is not null
                Assert.NotNull(i.Value.Generator, $"Generator null for {i.Key}");
                // Dimensions
                Assert.Greater(i.Value.Width, 0, "Width <= 0");
                Assert.Greater(i.Value.Height, 0, "Height <= 0");
            }
        }
    }
}
