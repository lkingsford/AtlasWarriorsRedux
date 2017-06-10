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
        /// Data for Basic Constructor
        /// </summary>
        static List<Tuple<int, int>> BasicConstructorData = new List<Tuple<int, int>>()
        {
            new Tuple<int, int>(-1,-1),
            new Tuple<int, int>(0,-1),
            new Tuple<int, int>(100, 0),
            new Tuple<int, int>(0, 0),
            new Tuple<int, int>(100, 50)
        };

        //[TestCase, TestCaseData("BasicConstructorData")]
        [Test]
        [TestCaseSource("BasicConstructorData")]
        public void BasicConstructor(Tuple<int, int> TestXY)
        {
            var xy = new  AtlasWarriorsGame.XY(TestXY.Item1, TestXY.Item2);
            Assert.AreEqual(xy.X, TestXY.Item1, "X not set correctly");
            Assert.AreEqual(xy.Y, TestXY.Item2, "Y not set correctly");
        }

    }
}
