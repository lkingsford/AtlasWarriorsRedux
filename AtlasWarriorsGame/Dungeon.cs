using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame
{
    /// <summary>
    /// Play-field area - a specific dungeon level
    /// </summary>
    public class Dungeon
    {
        /// <summary>
        /// Create instance of dungeon with given size
        /// </summary>
        /// <param name="Width">Width of the dungeon</param>
        /// <param name="Height">Height of the dungeon</param>
        /// <param name="Generator">
        /// Generator to use to build the dungeon.
        /// Defaults to DefaultGenerator
        /// </param>
        public Dungeon(int Width, int Height, Func<Dungeon, bool> Generator = null)
        {
            TileMap = new DungeonCell[Width, Height];
            Actors = new List<Actor>();
            // Invoke generator, unless it's null which use default
            (Generator ?? DungeonGenerators.DefaultGenerator.Generate).Invoke(this);
        }

        /// <summary>
        /// Types of dungeon cells in the map
        /// </summary>
        public enum DungeonCell 
        {
            EMPTY,
            FLOOR,
            WALL,
            DOOR
        }

        /// <summary>
        /// Return if given position is walkable
        /// </summary>
        /// <param name="x">X coord to check</param>
        /// <param name="y">Y coord to check</param>
        /// <returns></returns>
        public bool Walkable(XY Coord)
        {
            switch (GetCell(Coord))
            {
                case DungeonCell.FLOOR:
                case DungeonCell.DOOR:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Return true if any cell is empty
        /// </summary>
        /// <returns>Return true if any cell is empty</returns>
        public bool AnyEmpty()
        {
            for (var ix = 0; ix < Width; ++ix)
            {
                for (var iy = 0; iy < Height; ++iy)
                {
                    if (TileMap[ix, iy] == DungeonCell.EMPTY)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Array of dungeon cells that forms the map 
        /// </summary>
        protected DungeonCell[,] TileMap;

        /// <summary>
        /// Change part of the map
        /// </summary>
        /// <param name="Coord">Point to change</param>
        /// <param name="NewValue">Value to change point to</param>
        public void SetCell(XY Coord, DungeonCell NewValue)
        {
            TileMap[Coord.X, Coord.Y] = NewValue;
        }

        /// <summary>
        /// Get tile on map
        /// </summary>
        /// <param name="Coord">Point to get</param>
        /// <returns></returns>
        public DungeonCell GetCell(XY Coord)
        {
            // If in bounds, return TileMap
            if (Coord.X >= 0 &&
                Coord.X < Width &&
                Coord.Y >= 0 &&
                Coord.Y < Height)
            {
                return TileMap[Coord.X, Coord.Y];
            }
            // Otherwise, return empty
            return DungeonCell.EMPTY;
        }

        /// <summary>
        /// Width of the map
        /// </summary>
        public int Width
        {
            get
            {
                return TileMap.GetLength(0);
            }
        }

        /// <summary>
        /// Height of the map
        /// </summary>
        public int Height
        {
            get
            {
                return TileMap.GetLength(1);
            }
        }

        /// <summary>
        /// Where the player starts on this dungeon
        /// </summary>
        public XY StartLocation;

        /// <summary>
        /// All Actors, currently in this dungeon
        /// </summary>
        public List<Actor> Actors;
    }
}
