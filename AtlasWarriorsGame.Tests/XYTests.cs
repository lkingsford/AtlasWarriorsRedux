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
            var xy = new AtlasWarriorsGame.XY(X, Y);
            Assert.AreEqual(xy.X, X, "X not set correctly");
            Assert.AreEqual(xy.Y, Y, "Y not set correctly");
        }


        /// <summary>
        /// Valid values, with result, for Contains tests
        /// </summary>
        static object[] ContainsValues =
        {
            new object[] {new XY(0,0), 0, 0, 0, 0, true},
            new object[] {new XY(0,0), -1, -1, 1, 1, true},
            new object[] {new XY(-3, -3), -5, -3, 3, 1, true},
            new object[] {new XY(3, -3), -5, -3, 3, 1, true},
            new object[] {new XY(3, 5), -5, -3, 3, 5, true},
            new object[] {new XY(3, 5), -5, -3, 3, 4, false},
            new object[] {new XY(-3, -5), -1, -3, 3, 4, false},
            new object[] {new XY(0, 0), 1, -3, 3, 4, false}
        };

        /// <summary>
        /// Test that contains returns correct results for correct input
        /// </summary>
        /// <param name="coord">Coord to test</param>
        /// <param name="left">MinX bound</param>
        /// <param name="top">MinY bound</param>
        /// <param name="right">MaxX bound</param>
        /// <param name="bottom">MaxY bound</param>
        /// <param name="result">Desired result</param>
        [Test]
        [TestCaseSource("ContainsValues")]
        public void ContainsTest(XY coord,
                                 int left,
                                 int top,
                                 int right,
                                 int bottom,
                                 bool result)
        {
            Assert.AreEqual(coord.ContainedBy(left, top, right, bottom), result);
        }

        /// <summary>
        /// Invalid values for contains that should raise argument exception
        /// </summary>
        static object[] ContainsInvalidValues =
        {
            new object[] {new XY(0,0), -1, -2, -3, -4},
            new object[] {new XY(0,0), 4, 2, 0, 2},
            new object[] {new XY(0,0), 0, 4, 0, 2},
        };

        /// <summary>
        /// Test that invalid parameters raise argument exceptions
        /// </summary>
        /// <param name="coord">Coord to test</param>
        /// <param name="left">MinX bound</param>
        /// <param name="top">MinY bound</param>
        /// <param name="right">MaxX bound</param>
        /// <param name="bottom">MaxY bound</param>
        [Test]
        [TestCaseSource("ContainsInvalidValues")]
        public void ContainsArgumentExceptionTest(XY coord,
                                                  int left,
                                                  int top,
                                                  int right,
                                                  int bottom)
        {
            Assert.Throws(typeof(ArgumentException), delegate { coord.ContainedBy(left, top, right, bottom); });
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

        /// <summary>
        /// Points for testing subtraction - operand 1, operand 2, correct result
        /// </summary>
        static object[] SubValues =
        {
            new object[] {new XY(0,0), new XY(0,0), new XY(0,0) },
            new object[] {new XY(1,0), new XY(0,0), new XY(1,0) },
            new object[] {new XY(5,5), new XY(2,2), new XY(3,3) },
            new object[] {new XY(1,5), new XY(1,2), new XY(0,3) },
            new object[] {new XY(0,0), new XY(-1,0), new XY(1,0) },
            new object[] {new XY(-1,-1), new XY(-1,0), new XY(0,-1) },
            new object[] {new XY(1,1), new XY(3,0), new XY(-2,1) }
        };

        /// <summary>
        /// Test addition
        /// </summary>
        /// <param name="Op1">First operand</param>
        /// <param name="Op2">Second operand</param>
        /// <param name="Result">Correct result</param>
        [Test]
        [TestCaseSource("SubValues")]
        public void SubTest(XY Op1, XY Op2, XY Result)
        {
           // IsTrue, because AreEqual wasn't using custom ==
           Assert.IsTrue((Op1 - Op2) == Result); 
        }


        /// <summary>
        /// Points for testing addition - operand 1, operand 2, correct result
        /// </summary>
        static object[] AddValues =
        {
            new object[] {new XY(0,0), new XY(0,0), new XY(0,0) },
            new object[] {new XY(1,0), new XY(0,0), new XY(1,0) },
            new object[] {new XY(5,5), new XY(2,2), new XY(7,7) },
            new object[] {new XY(1,5), new XY(1,2), new XY(2,7) },
            new object[] {new XY(0,0), new XY(-1,0), new XY(-1,0) },
            new object[] {new XY(-1,-1), new XY(-1,0), new XY(-2,-1) },
        };

        /// <summary>
        /// Test addition
        /// </summary>
        /// <param name="Op1">First operand</param>
        /// <param name="Op2">Second operand</param>
        /// <param name="Result">Correct result</param>
        [Test]
        [TestCaseSource("AddValues")]
        public void AddTest(XY Op1, XY Op2, XY Result)
        {
           // IsTrue, because AreEqual wasn't using custom ==
           Assert.IsTrue((Op1 + Op2) == Result); 
        }

        /// <summary>
        /// Points for testing equality
        /// </summary>
        static object[] EqualsValues =
        {
            new XY(0,0) ,
            new XY(5,0),
            new XY(-1,-1),
            new XY(-1,0)
        };

        /// <summary>
        /// Test that equals is correct
        /// </summary>
        /// <param name="Op1"></param>
        [Test]
        [TestCaseSource("EqualsValues")]
        public void EqualsTest(XY Op1)
        {
            Assert.AreEqual(Op1, Op1);
        }
    }
}
