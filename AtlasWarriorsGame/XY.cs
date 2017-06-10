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
    }
}
