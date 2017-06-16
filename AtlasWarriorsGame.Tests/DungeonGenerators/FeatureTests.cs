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
            new object[] { new XY(3, 1), DungeonCell.OPEN_DOOR },
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
    }
}
