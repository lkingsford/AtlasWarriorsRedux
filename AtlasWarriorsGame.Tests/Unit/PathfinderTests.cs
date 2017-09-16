using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasWarriorsGame.Tests.Unit
{
    [TestFixture]
    class PathfinderTests
    {
        /// <summary>
        /// Tests for 'FindPath'
        /// </summary>
        public static object[] FoundShortestTestSource = new object[]
        {
            // Straight line, left to right
            new object[]
            {
                new bool[,]
                {
                    {false, false, false, false, false},
                    {false, true,  true,  true,  false},
                    {false, false, false, false, false},
                },
                new XY(1, 1),
                new XY(3, 1),
                new List<XY> {new XY(1, 1), new XY(2, 1), new XY(3, 1)},
                true,
            },

            // Straight line, right to left
            new object[]
            {
                new bool[,]
                {
                    {false, false, false, false, false},
                    {false, true , true , true , false},
                    {false, false, false, false, false},
                },
                new XY(3, 1),
                new XY(1, 1),
                new List<XY> {new XY(3, 1), new XY(2, 1), new XY(1, 1)},
                true,
            },

            // No path, failure allowed
            new object[]
            {
                new bool[,]
                {
                    {false, false, false, false, false},
                    {false, false, false, false, false},
                    {false, false, false, false, false},
                },
                new XY(3, 1),
                new XY(1, 1),
                null,
                true,
            },

            // Nearest point, no path, no failure allowed
            new object[]
            {
                new bool[,]
                {
                    {false, false, false, false, false},
                    {false, true , false, false, false},
                    {false, true , true , true , false},
                    {false, false, false, false, false},
                },
                new XY(1, 1),
                new XY(4, 1),
                new List<XY> {new XY(1, 1), new XY(2, 2), new XY(3, 2)},
                false,
            },

            // Path goes on edge
            new object[]
            {
                new bool[,]
                {
                    {false, false, false, false, false},
                    {true , false, false, false, false},
                    {true , true , true , true , false},
                    {false, false, false, false, false},
                },
                new XY(0, 1),
                new XY(3, 2),
                new List<XY> {new XY(0, 1), new XY(1, 2), new XY(2, 2), new XY(3, 2), },
                true,
            }
        };

        /// <summary>
        /// Test that shortest route found for given path
        /// </summary>
        /// <param name="walkableMap">The bool walkable map</param>
        /// <param name="start">Start location</param>
        /// <param name="end">End location</param>
        /// <param name="path">Desired result</param>
        /// <param name="nullAllowed">Whether failure allowed passed to FindPathu</param>
        [Test]
        [TestCaseSource("FoundShortestTestSource")]
        public static void FoundShortest(bool[,] walkableMap,
                                         XY start,
                                         XY end,
                                         List<XY> path,
                                         bool nullAllowed)
        {
            // Rotate map from Y,X to X,Y
            var rotatedMap = new bool[walkableMap.GetLength(1), walkableMap.GetLength(0)];
            for(var x = 0; x < walkableMap.GetLength(1); ++x)
            {
                for(var y = 0; y < walkableMap.GetLength(0); ++y)
                {
                    rotatedMap[x, y] = walkableMap[y, x];
                }
            }
            var foundPath = Pathfinder.FindPath(rotatedMap, start, end, 20, nullAllowed);
            Assert.AreEqual(path, foundPath, "Path did not match expected result");
        }
    }
}
