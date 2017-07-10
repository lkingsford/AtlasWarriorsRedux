using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MgUiCommon
{
    /// <summary>
    /// Cell in the console display
    /// </summary>
    public struct ConsoleCell
    {
        /// <summary>
        /// Character that will be drawn
        /// </summary>
        public char DrawChar;

        /// <summary>
        /// Color in the background of the cell
        /// </summary>
        public Color BackColor;

        /// <summary>
        /// Color in the foreground of the cell
        /// </summary>
        public Color ForeColor;
    }

}
