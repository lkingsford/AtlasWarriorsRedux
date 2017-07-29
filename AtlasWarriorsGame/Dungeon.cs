using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AtlasWarriorsGame.Message;

namespace AtlasWarriorsGame
{
    /// <summary>
    /// Play-field area - a specific dungeon level
    /// </summary>
    public class Dungeon : IReceiver
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
        public Dungeon(int Width, int Height)
        {
            TileMap = new DungeonCell[Width, Height];
            VisibilityMap = new CellVisibility[Width, Height];
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
        /// Do a full turn on this level
        /// </summary>
        /// <returns>Messages created in the turn</returns>
        public List<Message.Message> DoTurn()
        {
            // Messages to return
            foreach (var Actor in Actors)
            {
                Actor.DoTurn();
            }
            Clean();
            var messages = new List<Message.Message>(MessageQueue);
            MessageQueue.Clear();
            return messages;
        }

        /// <summary>
        /// Clean out the dead bodies
        /// </summary>
        public void Clean()
        {
            Actors.RemoveAll(i => i.Dead);
        }

        /// <summary>
        /// What is currently known/seen
        /// </summary>
        public enum CellVisibility
        {
            // Never seen
            UNSEEN,
            // Seen, but not now
            SEEN,
            // Visible now
            VISIBLE
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
        /// Get if there's an actor on a tile
        /// </summary>
        /// <param name="Coord">Coordinate to check</param>
        /// <returns>Actor on the tile. Null if none.</returns>
        public Actor Occupant(XY Coord)
        {
            return Actors.FirstOrDefault(i=>i.Location == Coord);
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
        /// Array containing whether each part of the map is visible
        /// </summary>
        /// <remarks>I really dunno if this should be here or in player character...</remarks>
        protected CellVisibility[,] VisibilityMap;

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
        /// Return visibility of cell
        /// </summary>
        /// <param name="Coord">Coordinates to look up</param>
        /// <returns>Visibility of requested cell</returns>
        public CellVisibility GetVisibility(XY Coord)
        {
            return VisibilityMap[Coord.X, Coord.Y];
        }

        /// <summary>
        /// Recalculate visibility with new PC coordinates
        /// </summary>
        /// <param name="Coord">Coordinate where PC has moved</param>
        public void PcMoved(XY Coord)
        {
            // Reset all visible to seen
            for (int ix = 0; ix < Width; ++ix)
            {
                for (int iy = 0; iy < Height; ++iy)
                {
                    if (VisibilityMap[ix, iy] == CellVisibility.VISIBLE)
                    {
                        VisibilityMap[ix, iy] = CellVisibility.SEEN;
                    }
                }
            }
            VisibleFill(Coord, true);
        }

        /// <summary>
        /// Fill current visible with SEEN. Recursive flood fill algorithm.
        /// </summary>
        /// <param name="Coord">Coordinate to mark as seen</param>
        /// <param name="Force">Force to continue, even if would normally block</param>
        protected void VisibleFill(XY Coord, bool Force = false)
        {
            var cell = GetCell(Coord);
            bool alreadyVisible = VisibilityMap[Coord.X, Coord.Y] == CellVisibility.VISIBLE;
            // EMPTY might be out of bounds. Should never be where player is.
            if (cell != DungeonCell.EMPTY)
            {
                VisibilityMap[Coord.X, Coord.Y] = CellVisibility.VISIBLE;
            }
            bool blocksSight =
                (cell == DungeonCell.WALL) || 
                (cell == DungeonCell.DOOR) ||
                (cell == DungeonCell.EMPTY);
            if (Force || (!alreadyVisible && !blocksSight))
            {
                VisibleFill(Coord + new XY(-1, 0), false);
                VisibleFill(Coord + new XY(1, 0), false);
                VisibleFill(Coord + new XY(0, -1), false);
                VisibleFill(Coord + new XY(0, 1), false);
                VisibleFill(Coord + new XY(-1, -1), false);
                VisibleFill(Coord + new XY(1, 1), false);
                VisibleFill(Coord + new XY(1, -1), false);
                VisibleFill(Coord + new XY(-1, 1), false); ;
            }
        }

        /// <summary>
        /// Add message to queue, that will be sent at end of turn
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(Message.Message message)
        {
            MessageQueue.Add(message);
        }

        /// <summary>
        /// Messages created during turn
        /// </summary>
        protected List<Message.Message> MessageQueue = new List<Message.Message>();

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
        /// Places where enemies can be spawned
        /// </summary>
        public List<SpawnArea> SpawnAreas = new List<SpawnArea>();

        /// <summary>
        /// Where the player starts on this dungeon
        /// </summary>
        public XY StartLocation;

        /// <summary>
        /// All Actors, currently in this dungeon
        /// </summary>
        public List<Actor> Actors = new List<Actor>();
    }
}
