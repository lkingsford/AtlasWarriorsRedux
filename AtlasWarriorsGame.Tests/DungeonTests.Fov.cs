using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasWarriorsGame;
using static AtlasWarriorsGame.Dungeon;
using static AtlasWarriorsGame.Tests.TestUtils;

namespace AtlasWarriorsGame.Tests
{
    /// <summary>
    /// Tests for the FOV manipulation in the Dungeon class
    /// </summary>
    [TestFixture]
    public class Dungeon_Fov
    {
        /// <summary>
        /// Dungeon containing a few rooms
        /// </summary>
        private Dungeon TestDungeon;

        /// <summary>
        /// Set up test dungeon for 
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            TestDungeon = GetDungeon(
                "##########",
                "#..#.....#",
                "#..+.....#",
                "####+#####",
                "#.....#   ",
                "#.....#   ",
                "#######   "
                );
        }

        /// <summary>
        /// When in a room, the room is seen
        /// </summary>
        [Test]
        public void TestRoomVisibleInRoom()
        {
            TestDungeon.PcMoved(new XY(2, 2));

            // Needed a concise way to list desired result. Unsure if this is best or not.
            // Using 'V' as visible, 'S' as seen and 'U' as unseen
            var desiredResult = new string[7] {
                "VVVVUUUUUU",
                "VVVVUUUUUU",
                "VVVVUUUUUU",
                "VVVVUUUUUU",
                "UUUUUUUUUU",
                "UUUUUUUUUU",
                "UUUUUUUUUU"
                };

            // Check every value is identical
            for (int ix = 0; ix < TestDungeon.Width; ++ix)
            {
                for (int iy = 0; iy < TestDungeon.Height; ++iy)
                {
                    // Get character from string
                    var seenRaw = desiredResult[iy].ToCharArray()[ix];
                    // ... and turn it into a cell visibility
                    var seenState = seenRaw == 'S' ? Dungeon.CellVisibility.SEEN :
                        seenRaw == 'U' ? Dungeon.CellVisibility.UNSEEN :
                        Dungeon.CellVisibility.VISIBLE;
                    Assert.AreEqual(seenState, TestDungeon.GetVisibility(new XY(ix, iy)),
                        $"{ix}, {iy} incorrect");
                }
            }
        }

        /// <summary>
        /// When in door, both rooms are seen
        /// </summary>
        [Test]
        public void TestRoomVisibleInDoor()
        {
            TestDungeon.PcMoved(new XY(3, 2));

            // Needed a concise way to list desired result. Unsure if this is best or not.
            // Using 'V' as visible, 'S' as seen and 'U' as unseen
            var desiredResult = new string[7] {
                "VVVVVVVVVV",
                "VVVVVVVVVV",
                "VVVVVVVVVV",
                "VVVVVVVVVV",
                "UUUUUUUUUU",
                "UUUUUUUUUU",
                "UUUUUUUUUU"
                };

            // Check every value is identical
            for (int ix = 0; ix < TestDungeon.Width; ++ix)
            {
                for (int iy = 0; iy < TestDungeon.Height; ++iy)
                {
                    // Get character from string
                    var seenRaw = desiredResult[iy].ToCharArray()[ix];
                    // ... and turn it into a cell visibility
                    var seenState = seenRaw == 'S' ? Dungeon.CellVisibility.SEEN :
                        seenRaw == 'U' ? Dungeon.CellVisibility.UNSEEN :
                        Dungeon.CellVisibility.VISIBLE;
                    Assert.AreEqual(seenState, TestDungeon.GetVisibility(new XY(ix, iy)),
                        $"{ix}, {iy} incorrect");
                }
            }
        }


        /// <summary>
        /// When moved out of room, old area is "Seen" and new area is "Visible"
        /// </summary>
        [Test]
        public void TestSeenPreviouslyVisible()
        {
            TestDungeon.PcMoved(new XY(3, 2));
            TestDungeon.PcMoved(new XY(4, 2));

            // Needed a concise way to list desired result. Unsure if this is best or not.
            // Using 'V' as visible, 'S' as seen and 'U' as unseen
            var desiredResult = new string[7] {
                "SSSVVVVVVV",
                "SSSVVVVVVV",
                "SSSVVVVVVV",
                "SSSVVVVVVV",
                "UUUUUUUUUU",
                "UUUUUUUUUU",
                "UUUUUUUUUU"
                };

            // Check every value is identical
            for (int ix = 0; ix < TestDungeon.Width; ++ix)
            {
                for (int iy = 0; iy < TestDungeon.Height; ++iy)
                {
                    // Get character from string
                    var seenRaw = desiredResult[iy].ToCharArray()[ix];
                    // ... and turn it into a cell visibility
                    var seenState = seenRaw == 'S' ? Dungeon.CellVisibility.SEEN :
                        seenRaw == 'U' ? Dungeon.CellVisibility.UNSEEN :
                        Dungeon.CellVisibility.VISIBLE;
                    Assert.AreEqual(seenState, TestDungeon.GetVisibility(new XY(ix, iy)),
                        $"{ix}, {iy} incorrect");
                }
            }
        }
    }
}