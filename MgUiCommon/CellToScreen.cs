using AtlasWarriorsGame;
using Microsoft.Xna.Framework;
using static AtlasWarriorsGame.Dungeon;

namespace MgUiCommon
{
    /// <summary>
    /// Classes used to get characters for elements
    /// </summary>
    static class CellToScreen
    {
        /// <summary>
        /// Get cell to draw from dungeon tile
        /// </summary>
        /// <param name="Cell">Cell to get version</param>
        /// <param name="Visible">Whether cell is visibile or </param>
        /// <returns></returns>
        public static ConsoleCell GetTileCell(DungeonCell cell, bool visible)
        {
            switch (cell)
            {
                case DungeonCell.Empty:
                    return new ConsoleCell()
                    {
                        DrawChar = ' ',
                        BackColor = Color.Black,
                        ForeColor = Color.Black
                    }; 
                case DungeonCell.Floor:
                    return new ConsoleCell()
                    {
                        DrawChar = '.',
                        BackColor = visible ? Color.Black : Color.Black,
                        ForeColor = visible ? Color.LightGray : Color.DimGray
                    }; 
                case DungeonCell.Door:
                    return new ConsoleCell()
                    {
                        DrawChar = '+',
                        BackColor = visible ? Color.Black : Color.Black,
                        ForeColor = visible ? Color.BlueViolet : Color.DimGray
                    }; ;
                case DungeonCell.Wall:
                    return new ConsoleCell()
                    {
                        DrawChar = '#',
                        BackColor = visible ? Color.Black : Color.Black,
                        ForeColor = visible ? Color.LightGray : Color.DimGray
                    }; 
                default:
                    return new ConsoleCell()
                    {
                        DrawChar = 'X',
                        BackColor = visible ? Color.Black : Color.Black,
                        ForeColor = visible ? Color.Cyan : Color.DarkRed
                    }; 
            }
        }

        /// <summary>
        /// Get cell to draw from actor
        /// </summary>
        /// <param name="Cell"></param>
        /// <param name="Visible"></param>
        /// <returns></returns>
        public static ConsoleCell GetActorCell(Actor actor)
        {
            switch (actor.SpriteId)
            {
                case "PLAYER":
                    return new ConsoleCell()
                    {
                        DrawChar = '@',
                        BackColor = Color.DarkBlue,
                        ForeColor = Color.LightGray
                    };
                case "MONSTER":
                    return new ConsoleCell()
                    {
                        DrawChar = 'M',
                        BackColor = Color.Black,
                        ForeColor = Color.Green
                    };
                default:
                    return new ConsoleCell()
                    {
                        DrawChar = '?',
                        BackColor = Color.Black,
                        ForeColor = Color.DarkGray
                    }; ; ;
            }
        }
    }
}
