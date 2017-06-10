using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame
{
    /// <summary>
    /// Play-field area - a specific dungeon level
    /// </summary>
    class Dungeon
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Dungeon()
        {
        }

        /// <summary>
        /// Types of dungeon cells in the map
        /// </summary>
        public enum DungeonCellTypes 
        {
            FLOOR,
            WALL,
            OPEN_DOOR,
            CLOSED_DOOR
        }

        /// <summary>
        /// Return if given position is walkable
        /// </summary>
        /// <param name="x">X coord to check</param>
        /// <param name="y">Y coord to check</param>
        /// <returns></returns>
        bool Walkable(int x, int y)
        {
            return false;
        }
    }
}
