using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AtlasWarriorsGame.Dungeon;

namespace AtlasWarriorsGame.Tests
{
    [TestFixture]
    public class DefaultGeneratorTests
    {
        /// <summary>
        /// Check DefaultGenerator made:
        /// #####
        /// #...#
        /// #...#
        /// ##### exactly
        /// </summary>
        [Test]
        [Category("DungeonGenerators/DefaultGenerator")]
        public void BasicTest()
        {
            var D = new Dungeon(5, 4, 
                AtlasWarriorsGame.DungeonGenerators.DefaultGenerator.Generate);

            for (int ix = 0; ix < 5; ++ix)
            {
                Assert.AreEqual(D.GetCell(new XY(ix, 0)), DungeonCell.WALL,
                    "Top wall not match");
                Assert.AreEqual(D.GetCell(new XY(ix, 3)), DungeonCell.WALL,
                    "Bottom wall not match");
            }
            for (int iy = 0; iy < 4; ++iy)
            {
                Assert.AreEqual(D.GetCell(new XY(0, iy)), Dungeon.DungeonCell.WALL,
                    "Left wall not match");
                Assert.AreEqual(D.GetCell(new XY(4, iy)), Dungeon.DungeonCell.WALL,
                    "Right wall not match");
            }
            for (int ix = 1; ix < 4; ++ix)
            {
                for (int iy = 1; iy < 3; ++iy)
                {
                    Assert.AreEqual(D.GetCell(new XY(ix, iy)), DungeonCell.FLOOR,
                        "Floor not correct");
                }
            }
        }

        /// <summary>
        /// Check that 5x4 DefaultGenerator generates a spawn area for every point
        /// </summary>
        [Test]
        [Category("DungeonGenerators/DefaultGenerator")]
        public void SpawnAreaTest()
        {
            var D = new Dungeon(5, 4,
                AtlasWarriorsGame.DungeonGenerators.DefaultGenerator.Generate);

            for (int ix = 1; ix < 5; ++ix)
            {
                for (int iy = 1; iy < 3; ++iy)
                {
                    Assert.IsTrue(D.SpawnAreas.Any(i => i.Area.Contains(new XY(ix, iy))),
                        "Spawn area not found");
                }
            }
        }
    }
}
