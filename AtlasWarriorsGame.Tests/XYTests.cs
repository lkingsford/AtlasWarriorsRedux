using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasWarriorsGame.Tests
{
    [TestFixture]
    public class XYTests
    {
        /// <summary>
        /// Valid Points, with String Versions
        /// </summary>
        static object[] ValidPoints =
        {
            new object[] {-1,-1, "(-1, -1)"},
            new object[] {0,-1, "(0, -1)" },
            new object[] {100, 0, "(100, 0)"},
            new object[] {0, 0, "(0, 0)"},
            new object[] {100, 50, "(100, 50)"}
        };

        /// <summary>
        /// Test that basic constructor sets x/y correctly
        /// </summary>
        /// <param name="X">X coord</param>
        /// <param name="Y">Y coord</param>
        /// <param name="ToStringResult">Unused</param>
        [Test]
        [TestCaseSource("ValidPoints")]
        public void BasicConstructor(int X, int Y, string ToStringResult)
        {
            var xy = new  AtlasWarriorsGame.XY(X, Y);
            Assert.AreEqual(xy.X, X, "X not set correctly");
            Assert.AreEqual(xy.Y, Y, "Y not set correctly");
        }

        /// <summary>
        /// Test that XY.ToString() returns "(X, Y)"
        /// </summary>
        /// <param name="TestXY">XY to test</param>
        /// <param name="TestXYString">Correct string of XY</param>
        [Test]
        [TestCaseSource("ValidPoints")]
        public void ToStringTest(int X, int Y, string ToStringResult)
        {
            Assert.AreEqual(new XY(X, Y).ToString(), ToStringResult);
        }
    }
}
