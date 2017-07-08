using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AtlasWarriorsGame.Dungeon;

namespace AtlasWarriorsGame.Tests.DungeonGenerators
{
    /// <summary>
    /// Tests for the feature
    /// </summary>
    [TestFixture]
    class FeatureTests
    {
        /// <summary>
        /// Feature to do tests to
        /// </summary>
        AtlasWarriorsGame.DungeonGenerators.Feature Feature;

        /// <summary>
        /// Create feature to test
        /// </summary>
        [SetUp]
        public void SetUp ()
        {
            Feature = new AtlasWarriorsGame.DungeonGenerators.Feature(10, 10);
        }

        /// <summary>
        /// Values to test getter/setter
        /// </summary>
        static object[] GetSetTestValues =
        {
            new object[] { new XY(1, 1), DungeonCell.EMPTY },
            new object[] { new XY(1, 2), DungeonCell.FLOOR },
            new object[] { new XY(2, 1), DungeonCell.WALL },
            new object[] { new XY(3, 1), DungeonCell.DOOR },
        };

        /// <summary>
        /// Test getter/setter
        /// </summary>
        /// <param name="coord">Coordinate to set</param>
        /// <param name="value">Value to retrieve</param>
        [Test]
        [TestCaseSource("GetSetTestValues")]
        public void GetSetTest(XY coord, DungeonCell value)
        {
            // Set it, then get it
            Feature.SetCell(coord, value);
            Assert.AreEqual(Feature.GetCell(coord), value, "Get or set didn't work");
        }

        /// <summary>
        /// Test that doors that are added are returned
        /// </summary>
        [Test]
        public void AddDoorTest()
        {
            Feature.AddPossibleDoor(new XY(5, 1));
            Feature.AddPossibleDoor(new XY(5, 7));
            Assert.IsTrue(Feature.PossibleDoors.Contains(new XY(5, 1)));
            Assert.IsTrue(Feature.PossibleDoors.Contains(new XY(5, 7)));
        }

        /// <summary>
        /// Test that the copy constructor creates copies of the feature map and what's in it - 
        /// not a reference
        /// </summary>
        [Test]
        public void CopyConstructorClonesFeatureMap()
        {
            var f1 = new AtlasWarriorsGame.DungeonGenerators.Feature(10, 10);
            var f2 = new AtlasWarriorsGame.DungeonGenerators.Feature(f1);
            Assert.AreNotSame(f1.GetCell(new XY(5, 5)), f2.GetCell(new XY(5, 5)));
        }

        /// <summary>
        /// Return x, y pairs from (0,0) to (3, 3) and constant dungeon cells
        /// </summary>
        /// <returns></returns>
        static object[] CopyConstructorCopiesMapValues = 
        {
            new object[] { new XY(0,0), DungeonCell.DOOR },
            new object[] { new XY(0,2), DungeonCell.FLOOR },
            new object[] { new XY(0,3), DungeonCell.WALL },
            new object[] { new XY(1,0), DungeonCell.EMPTY },
            new object[] { new XY(1,2), DungeonCell.DOOR},
            new object[] { new XY(1,3), DungeonCell.FLOOR },
            new object[] { new XY(2,0), DungeonCell.WALL },
            new object[] { new XY(2,1), DungeonCell.EMPTY },
            new object[] { new XY(2,3), DungeonCell.DOOR },
            new object[] { new XY(3,0), DungeonCell.FLOOR },
            new object[] { new XY(3,1), DungeonCell.WALL },
            new object[] { new XY(3,2), DungeonCell.EMPTY},
            new object[] { new XY(3,3), DungeonCell.DOOR },
        };

        /// <summary>
        /// Test that the entire map is copied. 
        /// </summary>
        /// <param name="Coord">Location to set</param>
        /// <param name="Value">Value to set it to</param>
        [TestCaseSource("CopyConstructorCopiesMapValues")]
        [Test]
        public void CopyConstructorCopiesMap(XY Coord, DungeonCell Value)
        {
            var f1 = new AtlasWarriorsGame.DungeonGenerators.Feature(4, 4);
            f1.SetCell(Coord, Value);
            var f2 = new AtlasWarriorsGame.DungeonGenerators.Feature(f1);
            Assert.AreEqual(Value, f2.GetCell(Coord));
        }

        /// <summary>
        /// Test that the copy constructor copies the potential doors
        /// </summary>
        [Test]
        public void CopyConstructorCopiesPossibleDoors()
        {
            Feature.AddPossibleDoor(new XY(4, 1));
            Feature.AddPossibleDoor(new XY(6, 2));
            var f2 = new AtlasWarriorsGame.DungeonGenerators.Feature(Feature);
            Assert.IsTrue(f2.PossibleDoors.Contains(new XY(4, 1)));
            Assert.IsTrue(f2.PossibleDoors.Contains(new XY(6, 2)));
        }

        /// <summary>
        /// Test that the copy constructor doesn't create a reference list - the two are 
        /// independent
        /// </summary>
        [Test]
        public void CopyConstructorCreatesNewPossibleDoors()
        {
            var f2 = new AtlasWarriorsGame.DungeonGenerators.Feature(Feature);
            Feature.AddPossibleDoor(new XY(6, 2));
            Assert.IsFalse(f2.PossibleDoors.Contains(new XY(6, 2)), 
                "PossibleDoors lists weren't copied and share a reference" );
        }

        /// <summary>
        /// Dimension test data
        /// </summary>
        static object[] DimensionTestSource =
        {
            new object[] {1, 1},
            new object[] {0, 0},
            new object[] {10, 1},
            new object[] {10, 10},
            new object[] {1, 10},
        };

        /// <summary>
        /// Test width and height return correct data
        /// </summary>
        /// <param name="W"></param>
        /// <param name="H"></param>
        [TestCaseSource("DimensionTestSource")]
        [Test]
        public void WidthHeightTest(int W, int H)
        {
            var f = new AtlasWarriorsGame.DungeonGenerators.Feature(W, H);
            Assert.AreEqual(W, f.Width);
            Assert.AreEqual(H, f.Height);
        }

        /// <summary>
        /// Test all rotation of feature with following shape
        /// Other rotations shown
        /// 
        /// #..    ..+#   -..     ...- 
        /// +..    .#..   .#.     ..#.
        /// .#.    -...   ..+     #+..
        /// ..-           ..#
        /// UP     RT      DN      LT 
        /// 
        /// </summary>
        /// <remarks>Can anybody think a better way to test?</remarks>
        [Test]
        public void RotateTest()
        {
            // Define initial one
            var f1 = new AtlasWarriorsGame.DungeonGenerators.Feature(3, 4);
            f1.SetCell(new XY(0, 0), DungeonCell.WALL);
            f1.SetCell(new XY(0, 1), DungeonCell.DOOR);
            f1.SetCell(new XY(1, 2), DungeonCell.WALL);
            f1.SetCell(new XY(2, 3), DungeonCell.DOOR);

            // Test up
            var fUp = f1.Rotate(AtlasWarriorsGame.DungeonGenerators.Feature.Rotation.UP);
            Assert.AreEqual(fUp.GetCell(new XY(0, 0)), DungeonCell.WALL);
            Assert.AreEqual(fUp.GetCell(new XY(0, 1)), DungeonCell.DOOR);
            Assert.AreEqual(fUp.GetCell(new XY(1, 2)), DungeonCell.WALL);
            Assert.AreEqual(fUp.GetCell(new XY(2, 3)), DungeonCell.DOOR);

            // Test right
            var fRight = f1.Rotate(AtlasWarriorsGame.DungeonGenerators.Feature.Rotation.RIGHT);
            Assert.AreEqual(fRight.GetCell(new XY(3, 0)), DungeonCell.WALL);
            Assert.AreEqual(fRight.GetCell(new XY(2, 0)), DungeonCell.DOOR);
            Assert.AreEqual(fRight.GetCell(new XY(1, 1)), DungeonCell.WALL);
            Assert.AreEqual(fRight.GetCell(new XY(0, 2)), DungeonCell.DOOR);

            // Test down
            var fDown = f1.Rotate(AtlasWarriorsGame.DungeonGenerators.Feature.Rotation.DOWN);
            Assert.AreEqual(fDown.GetCell(new XY(2, 3)), DungeonCell.WALL);
            Assert.AreEqual(fDown.GetCell(new XY(2, 2)), DungeonCell.DOOR);
            Assert.AreEqual(fDown.GetCell(new XY(1, 1)), DungeonCell.WALL);
            Assert.AreEqual(fDown.GetCell(new XY(0, 0)), DungeonCell.DOOR);

            // Test left
            var fLeft = f1.Rotate(AtlasWarriorsGame.DungeonGenerators.Feature.Rotation.LEFT);
            Assert.AreEqual(fLeft.GetCell(new XY(0, 2)), DungeonCell.WALL);
            Assert.AreEqual(fLeft.GetCell(new XY(1, 2)), DungeonCell.DOOR);
            Assert.AreEqual(fLeft.GetCell(new XY(2, 1)), DungeonCell.WALL);
            Assert.AreEqual(fLeft.GetCell(new XY(3, 0)), DungeonCell.DOOR);
        }

        /// <summary>
        /// Test that a spawn area is created containing all floors in the feature
        /// </summary>
        [Test]
        public void SpawnAreaTest()
        {
            // Build an empty room
            Feature = AtlasWarriorsGame.DungeonGenerators.RoomsGenerator.CreateFeature(5, 10);

            // Test all correct
            for (int ix = 1; ix < (Feature.Width - 1); ++ix)
            {
                for (int iy = 1; iy < (Feature.Height - 1); ++iy)
                {
                    Assert.IsTrue(Feature.SpawnArea.Area.Contains(new XY(ix, iy)),
                        $"Part of spawn area new found at {ix}, {iy}");
                }
            }
        }
    }
}
