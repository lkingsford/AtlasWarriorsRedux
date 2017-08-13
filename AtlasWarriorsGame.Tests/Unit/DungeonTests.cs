using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasWarriorsGame;
using static AtlasWarriorsGame.Dungeon;

namespace AtlasWarriorsGame.Tests
{
    /// <summary>
    /// Tests for the Dungeon class
    /// </summary>
    [TestFixture]
    public class DungeonTests
    {
        /// <summary>
        /// Initialise dungeons for test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dungeon1 = new AtlasWarriorsGame.Dungeon(10, 10);
            Dungeon2 = new AtlasWarriorsGame.Dungeon(20, 10);
            Dungeon3 = new AtlasWarriorsGame.Dungeon(10, 20);
            Dungeon4 = new AtlasWarriorsGame.Dungeon(10, 10);
            Dungeon4.SetCell(new XY(2,1), DungeonCell.Empty);
            Dungeon4.SetCell(new XY(2,2), DungeonCell.Wall);
            Dungeon4.SetCell(new XY(2,3), DungeonCell.Door);
            Dungeon4.SetCell(new XY(2,4), DungeonCell.Door);
            Dungeon4.SetCell(new XY(2,5), DungeonCell.Floor);
        }

        /// 10x10 Dungeon, empty
        Dungeon Dungeon1;

        /// <summary>
        /// Locations that are out of bounds on Map 1
        /// </summary>
        static List<XY> Dungeon1OutOfBoundsData = new List<XY>
        {
            new XY(-1, -1),
            new XY(2, -1),
            new XY(11, -1),
            new XY(10, -1),
            new XY(10, 10),
            new XY(11, 10)
        };

        /// 20x10 Dungeon, empty
        Dungeon Dungeon2;

        /// 10x20 Dungeon, empty
        Dungeon Dungeon3;

        /// 10x10 Dungeon,
        /// [2,2] is wall
        /// [2,3] is closed door
        /// [2,4] is open door
        /// [2,5] is floor
        Dungeon Dungeon4;

        /// <summary>
        /// Test that Width returns correctly
        /// </summary>
        [Test]
        public void Width()
        {
            Assert.AreEqual(Dungeon1.Width, 10, "Width not reported correctly");
            Assert.AreEqual(Dungeon2.Width, 20, "Width not reported correctly");
            Assert.AreEqual(Dungeon3.Width, 10, "Width not reported correctly");
        }

        /// <summary>
        /// Test that Height returns correctly
        /// </summary>
        [Test]
        public void Height()
        {
            Assert.AreEqual(Dungeon1.Height, 10, "Height not reported correctly");
            Assert.AreEqual(Dungeon2.Height, 10, "Height not reported correctly");
            Assert.AreEqual(Dungeon3.Height, 20, "Height not reported correctly");
        }

        /// <summary>
        /// Valid points for the GetSetCellTest
        /// </summary>
        static object[] GetSetValidPoints =
        {
            new object[] {0, 0 },
            new object[] {5, 5 },
            new object[] {9, 0 },
            new object[] {0, 9 }
        };

        /// <summary>
        /// Test that Get/Set of cell works in ordinary circumstances
        /// </summary>
        /// <param name="X">X coord to test</param>
        /// <param name="Y">Y coord to test</param>
        [Test]
        [TestCaseSource("GetSetValidPoints")]
        public void GetSetCell(int X, int Y)
        {
            Dungeon1.SetCell(new XY(X, Y), Dungeon.DungeonCell.Wall);
            Assert.AreEqual(Dungeon1.GetCell(new XY(X, Y)), Dungeon.DungeonCell.Wall,
                "Dungeon cell not correctly read or set");
            Dungeon1.SetCell(new XY(X, Y), Dungeon.DungeonCell.Empty);
            Assert.AreEqual(Dungeon1.GetCell(new XY(X, Y)), Dungeon.DungeonCell.Empty,
                "Dungeon cell not correctly read or set");
            Dungeon1.SetCell(new XY(X, Y), Dungeon.DungeonCell.Floor);
            Assert.AreEqual(Dungeon1.GetCell(new XY(X, Y)), Dungeon.DungeonCell.Floor,
                "Dungeon cell not correctly read or set");
        }

        /// <summary>
        /// Test that an out of bounds get cell returns Empty
        /// </summary>
        [Test]
        [TestCaseSource("Dungeon1OutOfBoundsData")] 
        public void GetOutOfBoundsCell(XY Coord)
        {
            Assert.AreEqual(Dungeon1.GetCell(Coord), DungeonCell.Empty);
        }

        /// <summary>
        /// Test that walkable spaces return true
        /// </summary>
        [Test]
        public void GetWalkable()
        {
            Assert.IsTrue(Dungeon4.Walkable(new XY(2, 3)), "Door not walkable");
            Assert.IsTrue(Dungeon4.Walkable(new XY(2, 4)), "Open door not walkable");
            Assert.IsTrue(Dungeon4.Walkable(new XY(2, 5)), "Floor not walkable");
        }

        /// <summary>
        /// Test that unwalkable spaces return false
        /// </summary>
        [Test]
        public void GetUnWalkable()
        {
            Assert.IsFalse(Dungeon4.Walkable(new XY(2, 1)), "Empty is walkable");
            Assert.IsFalse(Dungeon4.Walkable(new XY(2, 2)), "Wall is walkable");
        }

        /// <summary>
        /// Test that an out of bounds walkable call returns False
        /// </summary>
        [Test]
        [TestCaseSource("Dungeon1OutOfBoundsData")] 
        public void OutOfBoundsWalkable(XY Coord)
        {
            Assert.IsFalse(Dungeon1.Walkable(Coord));
        }
    }

    /// <summary>
    /// Tests for moving between levels
    /// </summary>
    [TestFixture]
    public class DungeonStairsTests
    {
        /// <summary>
        /// Initialise dungeons for test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Dungeon1 = new Dungeon(10, 10);
            Dungeon2 = new Dungeon(10, 10);
            Dungeon3 = new Dungeon(10, 10);

            Dungeon1.Passages.Add(
                new Passage(Passage.PassageTypeEnum.OneWay, "START", new XY(1, 2)));
            Dungeon1.Passages.Add(
                new Passage(Passage.PassageTypeEnum.StairsDown, Dungeon2, new XY(1, 3)));
            Dungeon1.SetCell(new XY(1, 3), DungeonCell.StairDown);
            Dungeon2.Passages.Add(
                new Passage(Passage.PassageTypeEnum.StairsUp, Dungeon1, new XY(1, 6)));
            Dungeon2.SetCell(new XY(1, 6), DungeonCell.StairUp);
            Dungeon2.Passages.Add(
                new Passage(Passage.PassageTypeEnum.StairsUp, Dungeon3, new XY(3, 5)));
            Dungeon2.SetCell(new XY(3, 5), DungeonCell.StairUp);
            Dungeon3.Passages.Add(
                new Passage(Passage.PassageTypeEnum.OneWay, Dungeon2, new XY(6, 7)));
        }

        Dungeon Dungeon1;
        Dungeon Dungeon2;
        Dungeon Dungeon3;

        /// <summary>
        /// Test that trigger for stairs down works, and that character moves to location of
        /// the correct stairs up, for this dungeon, on the correct level
        /// </summary>
        [Test]
        public void StairsDownToLinkedStairsUp()
        {
            Actor actor = new Actor(Dungeon1, new XY(1, 2));
            actor.Move(new XY(0, 1));
            Assert.AreEqual(Dungeon2, actor.Dungeon, "Actor did not go to correct dungeon");
            Assert.AreEqual(new XY(1, 6), actor.Location, "Actor did not go to linking stairs up");
        }

        /// <summary>
        /// Test that trigger for stairs up works, and that character moves to location of
        /// stairs down, for this dungeon, on the correct level
        /// </summary>
        [Test]
        public void StairsUpToLinkedStairsDown()
        {
            Actor actor = new Actor(Dungeon2, new XY(1, 5));
            actor.Move(new XY(0, 1));
            Assert.AreEqual(Dungeon1, actor.Dungeon, "Actor did not go to correct dungeon");
            Assert.AreEqual(new XY(1, 3), actor.Location, "Actor did not go to stairs up");
        }

        /// <summary>
        /// Test that trigger for stairs up works, and that character moves to location of
        /// the one way passage where no stairs down
        /// </summary>
        [Test]
        public void StairsUpToLinkedOneWay()
        {
            Actor actor = new Actor(Dungeon2, new XY(3, 4));
            actor.Move(new XY(0, 1));
            Assert.AreEqual(Dungeon3, actor.Dungeon, "Actor did not go to correct dungeon");
            Assert.AreEqual(new XY(6, 7), actor.Location, "Actor did not go to one way");
        }

        /// <summary>
        /// Test that moving on One Way doesn't move the actor to the linking level
        /// </summary>
        [Test]
        public void OneWayDoesNotChangeLevel()
        {
            Actor actor = new Actor(Dungeon3, new XY(6, 6));
            actor.Move(new XY(0, 1));
            Assert.AreEqual(Dungeon3, actor.Dungeon, "Actor changed dungeon");
        }
    }
}
