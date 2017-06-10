using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame
{
    public class XY
    {
        /// <summary>
        /// Construct pair with given X, Y
        /// </summary>
        /// <param name="X">X Coordinate</param>
        /// <param name="Y">Y Coordinate</param>
        public XY(int X, int Y)
        {
            this._X = X;
            this._Y = Y; 
        }

        /// <summary>
        /// Underlying value for X
        /// </summary>
        private int _X;

        /// <summary>
        /// X coordinate
        /// </summary>
        public int X
        {
            get
            {
                return _X;
            }
        }

        /// <summary>
        /// Underlying value for Y
        /// </summary>
        private int _Y;

        /// <summary>
        /// Y coordinate
        /// </summary>
        public int Y
        {
            get
            {
                return _Y;
            }
        }

        /// <summary>
        /// Return coordinates as a string
        /// </summary>
        /// <returns>(X, Y)</returns>
        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        /// <summary>
        /// Addition operator for XY. Adds X to X and Y to Y - cartesian addition.
        /// </summary>
        /// <param name="first">First operand</param>
        /// <param name="second">Second operand</param>
        /// <returns></returns>
        public static XY operator +(XY first, XY second)
        {
            return new XY(first.X + second.X, first.Y + second.Y);
        }

        /// <summary>
        /// Subtraction operator for XY. Subtracts X from X and Y from Y.
        /// </summary>
        /// <param name="first">First operand</param>
        /// <param name="second">Second operand</param>
        /// <returns></returns>
        public static XY operator -(XY first, XY second)
        {
            return new XY(first.X - second.X, first.Y - second.Y);
        }

        /// <summary>
        /// Equality operator for XY
        /// </summary>
        /// <param name="first">First operand</param>
        /// <param name="second">Second operand</param>
        /// <returns></returns>
        public static bool operator ==(XY first, XY second)
        {
            return (first.X == second.X) && (first.Y == second.Y);
        }

        /// <summary>
        /// Equality operator for XY
        /// </summary>
        /// <param name="first">First operand</param>
        /// <param name="second">Second operand</param>
        /// <returns></returns>
        public static bool Equals(XY first, XY second)
        {
            return first == second;
        }
        /// <summary>
        /// Inequality operator for XY
        /// </summary>
        /// <param name="first">First operand</param>
        /// <param name="second">Second operand</param>
        /// <returns></returns>
        public static bool operator !=(XY first, XY second)
        {
            return !(first == second);
        }

    }
}
