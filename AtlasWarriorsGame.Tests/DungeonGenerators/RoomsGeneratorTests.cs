using AtlasWarriorsGame.DungeonGenerators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AtlasWarriorsGame.Dungeon;
using static AtlasWarriorsGame.Tests.TestUtils;

namespace AtlasWarriorsGame.Tests.DungeonGenerators
{
    /// <summary>
    /// Tests for Room Generator
    /// </summary>
    [TestFixture]
    class RoomsGeneratorTests
    {
        /// <summary>
        /// 10 by 7 feature for feature tests
        /// </summary>
        Feature Feature7by10 = RoomsGenerator.CreateFeature(10, 7);

        /// <summary>
        /// Generated library with max size 6 for tests
        /// </summary>
        List<Feature> FeatureLibrary = RoomsGenerator.GenerateFeatureLibrary(6);

        // Where doors should be in a 10x7 room
        static object[] FeatureDoorCases =
        {
            new object[] {1, 0},
            new object[] {3, 0},
            new object[] {5, 0},
            new object[] {7, 0},
            new object[] {1, 6},
            new object[] {3, 6},
            new object[] {5, 6},
            new object[] {7, 6},
            new object[] {0, 2},
            new object[] {0, 2},
            new object[] {0, 4},
            new object[] {0, 4},
            new object[] {9, 2},
            new object[] {9, 4},
        };

        [Test]
        [TestCaseSource("FeatureDoorCases")]
        public void CreateFeatureDoor(int ix, int iy)
        {
            Assert.IsTrue(Feature7by10.PossibleDoors.Contains(new XY(ix, iy)));
        }

        // Where doors should not be but would be easily accidentally broken to
        static object[] BadFeatureDoorCases =
        {
            new object[] {0, 0},
            new object[] {6, 1},
            new object[] {2, 0},
            new object[] {9, 3},
        };

        [Test]
        [TestCaseSource("BadFeatureDoorCases")]
        public void CreateFeatureDoorFail(int ix, int iy)
        {
            Assert.IsFalse(Feature7by10.PossibleDoors.Contains(new XY(ix, iy)));
        }

        /// <summary>
        /// Sizes of all the rooms that should exist in a feature library after generating it
        /// with max size 6
        /// </summary>
        static object[] FeatureLibraryTestCases =
        {
            new object[] {3, 3},
            new object[] {4, 3},
            new object[] {4, 4},
            new object[] {5, 3},
            new object[] {5, 4},
            new object[] {5, 5},
            new object[] {6, 3},
            new object[] {6, 4},
            new object[] {6, 5},
            new object[] {6, 6},
        };

        /// <summary>
        /// Check features created are what is expected
        /// </summary>
        [Test]
        [TestCaseSource("FeatureLibraryTestCases")]
        public void GenerateFeatureLibrary(int ExpectedX, int ExpectedY)
        {
            // Is this a good way to express this? I dunno!
            Assert.IsTrue(FeatureLibrary.Select(i => new XY(i.Width, i.Height))
                                        .Contains(new XY(ExpectedX, ExpectedY)));
        }



        /// <summary>
        /// Slightly complicated test cases for the AddFeatureTest tests
        /// </summary>
        static object[] AddFeatureTestCases =
        {
            // Basic shape, no translation
            new object[]
            {
                new XY(0, 0),
                GetDungeon(
                    "          ",
                    "          ",
                    "          ",
                    "          ",
                    "          "),
                GetFeature(
                    "#"
                    ),
                GetDungeon(
                    "#         ",
                    "          ",
                    "          ",
                    "          ",
                    "          "),
            },

            // Basic shape, translation
            new object[]
            {
                new XY(1, 2),
                GetDungeon(
                    "          ",
                    "          ",
                    "          ",
                    "          ",
                    "          "),
                GetFeature(
                    "#"
                    ),
                GetDungeon(
                    "          ",
                    "          ",
                    " #        ",
                    "          ",
                    "          ")
            },

            // Bigger shape, translation
            new object[]
            {
                new XY(2, 1),
                GetDungeon(
                    "          ",
                    "          ",
                    "          ",
                    "          ",
                    "          "),
                GetFeature(
                    "#####",
                    "#...#",
                    "#...#",
                    "#####"),
                GetDungeon(
                    "          ",
                    "  #####   ",
                    "  #...#   ",
                    "  #...#   ",
                    "  #####   ")
            },

            // Empty space in feature on existing dungeon
            new object[]
            {
                new XY(1, 1),
                GetDungeon(
                    "..........",
                    "..........",
                    "..........",
                    "..........",
                    ".........."),
                GetFeature(
                    "#####",
                    "#..# ",
                    "#.#  ",
                    "##   "),
                GetDungeon(
                    "..........",
                    ".#####....",
                    ".#..#.....",
                    ".#.#......",
                    ".##......."),
            },
        };

        /// <summary>
        /// Test that add feature works correctly
        /// </summary>
        /// <param name="Translate">Translate parameter for AddFeature</param>
        /// <param name="Dungeon">Dungeon parameter for AddFeature</param>
        /// <param name="Feature">Feature parameter for AddFeature</param>
        /// <param name="DesiredFilling">What filling should be after AddFeature</param>
        /// <param name="DesiredDungeon">
        /// What dungeon should be after AddFeature - only checks GetTiles
        /// </param>
        [Test]
        [TestCaseSource("AddFeatureTestCases")]
        public void AddFeature(XY Translate,
                               Dungeon Dungeon,
                               Feature Feature,
                               Dungeon DesiredDungeon)
        {
            RoomsGenerator.AddFeature(Translate, Dungeon, Feature);

            for (int ix = 0; ix < DesiredDungeon.Width; ++ix)
            {
                for (int iy = 0; iy < DesiredDungeon.Height; ++iy)
                {
                    Assert.AreEqual(Dungeon.GetCell(new XY(ix, iy)),
                        DesiredDungeon.GetCell(new XY(ix, iy)));
                }
            }
        }
        /// <summary>
        /// Slightly complicated test cases for feature fits returning true
        /// </summary>
        static object[] FeatureFitsSuccessfulTases =
        {
            // Basic shape, no translation
            new object[]
            {
                new XY(0, 0),
                GetDungeon(
                    "          ",
                    "          ",
                    "          ",
                    "          ",
                    "          "),
                GetFeature(
                    "#"
                    ),
            },

            // Basic shape, translation
            new object[]
            {
                new XY(1, 2),
                GetDungeon(
                    "          ",
                    "          ",
                    "          ",
                    "          ",
                    "          "),
                GetFeature(
                    "#"
                    ),
            },

            // Bigger shape, translation
            new object[]
            {
                new XY(2, 1),
                GetDungeon(
                    "          ",
                    "          ",
                    "          ",
                    "          ",
                    "          "),
                GetFeature(
                    "#####",
                    "#...#",
                    "#...#",
                    "#####"),
            },

            // Empty space in feature on existing dungeon, complex shape
            new object[]
            {
                new XY(1, 1),
                GetDungeon(
                    "..........",
                    ".     ....",
                    ".    .....",
                    ".   ......",
                    ".  ......."),
                GetFeature(
                    "#####",
                    "#..# ",
                    "#.#  ",
                    "##   "),
            }, 

            // Partially empty space, with correct overlap
            new object[]
            {
                new XY(0, 2),
                GetDungeon(
                    "###       ",
                    "#.#       ",
                    "#+#       ",
                    "          ",
                    "          ",
                    "          "),
                GetFeature(
                    "#+###",
                    "#...#",
                    "#####"),
            },
        };

        /// <summary>
        /// Tests that feature fits returns true when a feature fits
        /// </summary>
        /// <param name="Translate">Translate parameter for FeatureFits</param>
        /// <param name="Dungeon">Dungeon parameter for FeatureFits</param>
        /// <param name="Feature">Feature parameter for FeatureFits</param>
        [Test]
        [TestCaseSource("FeatureFitsSuccessfulCases")]
        public void FeatureFitsTestSuccessful(XY Translate, Dungeon Dungeon, Feature Feature)
        {
            Assert.IsTrue(RoomsGenerator.FeatureFits(Translate, Dungeon, Feature));
        }

        /// <summary>
        /// Slightly complicated test cases for feature fits returning false
        /// </summary>
        static object[] FeatureFitsFailedCases =
        {
            // Basic shape, no translation
            new object[]
            {
                "Basic, no translation",
                new XY(0, 0),
                GetDungeon(
                    "..........",
                    "..........",
                    "..........",
                    "..........",
                    ".........."),
                GetFeature(
                    "#"
                    ),
            },

            // Basic shape, translation
            new object[]
            {
                "Basic, translation",
                new XY(0, 4),
                GetDungeon(
                    "..........",
                    "..........",
                    "..........",
                    "..........",
                    ".........."),
                GetFeature(
                    "#"
                    ),
            },

            // Basic shape, translation, mostly blank except one
            new object[]
            {
                "Basic, translation, mostly blank",
                new XY(1, 1),
                GetDungeon(
                    "          ",
                    "          ",
                    "  #       ",
                    "          ",
                    "          "),
                GetFeature(
                    "####",
                    "#..#",
                    "#..#",
                    "####")
            },

            // Basic shape, translation, partly out of bounds
            new object[]
            {
                "Basic, translation, out of bounds",
                new XY(1, 4),
                GetDungeon(
                    "          ",
                    "          ",
                    "          ",
                    "          ",
                    "          "),
                GetFeature(
                    "####",
                    "#..#",
                    "#..#",
                    "####")
            },

            // Entire shape overlaps
            new object[]
            {
                "Entire shape overlap",
                new XY(1, 1),
                GetDungeon(
                    "          ",
                    " ####     ",
                    " #..#     ",
                    " #..#     ",
                    " ####     "),
                GetFeature(
                    "####",
                    "#..#",
                    "#..#",
                    "####")
            },

        };

        /// <summary>
        /// Tests that feature fits returns false when a feature fits
        /// </summary>
        /// <param name="Description">Unused description for Test Explorer</param>
        /// <param name="Translate">Translate parameter for FeatureFits</param>
        /// <param name="Dungeon">Dungeon parameter for FeatureFits</param>
        /// <param name="Feature">Feature parameter for FeatureFits</param>
        [Test]
        [TestCaseSource("FeatureFitsFailedCases")]
        public void FeatureFitsFailed(String Description, XY Translate, Dungeon Dungeon, Feature Feature)
        {
            Assert.IsFalse(RoomsGenerator.FeatureFits(Translate, Dungeon, Feature));
        }


        /// <summary>
        /// Test that adding a feature adds to the spawn areas
        /// </summary>
        [Test]
        public void AddFeatureAddsSpawnArea()
        {
            // Build an empty room
            var feature = AtlasWarriorsGame.DungeonGenerators.RoomsGenerator.CreateFeature(4, 3);
            var D = GetDungeon(
                "          ",
                "          ",
                "          ",
                "          ",
                "          "
            );

            // Clear spawn areas to be fresh
            D.SpawnAreas.RemoveAll(i=>true);

            // Add feature, with translation
            RoomsGenerator.AddFeature(new XY(2, 1), D, feature);

            // Check spawn area added
            Assert.AreEqual(1, D.SpawnAreas.Count, "No spawn area added");
            var area = D.SpawnAreas.First();

            // Test all correct
            for (int ix = 3; ix < 5; ++ix)
            {
                int iy = 2;
                Assert.IsTrue(area.Area.Contains(new XY(ix, iy)),
                    $"Part of spawn area new found at {ix}, {iy}");
            }
        }
    }
}
