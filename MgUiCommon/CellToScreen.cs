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
                case DungeonCell.StairUp:
                    return new ConsoleCell()
                    {
                        DrawChar = '<',
                        BackColor = visible ? Color.Black : Color.Black,
                        ForeColor = visible ? Color.Green : Color.DarkGreen
                    };
                case DungeonCell.StairDown:
                    return new ConsoleCell()
                    {
                        DrawChar = '>',
                        BackColor = visible ? Color.Black : Color.Black,
                        ForeColor = visible ? Color.Red : Color.DarkRed
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
                case "ASSASSIN":
                    return new ConsoleCell()
                    {
                        DrawChar = 'A',
                        BackColor = Color.Black,
                        ForeColor = Color.DarkGray
                    };
                case "BANDIT":
                    return new ConsoleCell()
                    {
                        DrawChar = 'B',
                        BackColor = Color.Black,
                        ForeColor = Color.Brown
                    };
                case "CRITTER":
                    return new ConsoleCell()
                    {
                        DrawChar = 'c',
                        BackColor = Color.Black,
                        ForeColor = Color.MintCream
                    };
                case "DRAKE":
                    return new ConsoleCell()
                    {
                        DrawChar = 'd',
                        BackColor = Color.Black,
                        ForeColor = Color.MediumVioletRed
                    };
                case "GOLIATH":
                    return new ConsoleCell()
                    {
                        DrawChar = 'G',
                        BackColor = Color.Black,
                        ForeColor = Color.Silver
                    };
                case "HEALER":
                    return new ConsoleCell()
                    {
                        DrawChar = 'h',
                        BackColor = Color.Black,
                        ForeColor = Color.CornflowerBlue
                    };
                case "ORC":
                    return new ConsoleCell()
                    {
                        DrawChar = 'o',
                        BackColor = Color.Green,
                        ForeColor = Color.DarkGray
                    };
                case "ZOMBIE":
                    return new ConsoleCell()
                    {
                        DrawChar = 'z',
                        BackColor = Color.Black,
                        ForeColor = Color.GreenYellow
                    };
                default:
                    return new ConsoleCell()
                    {
                        // Default to first character of SpriteId, or ? if none
                        DrawChar = actor.SpriteId.Length > 0 ?
                            actor.SpriteId.ToCharArray()[0] : '?',
                        BackColor = Color.Black,
                        ForeColor = Color.DarkGray
                    }; ; ;
            }
        }
    }
}
