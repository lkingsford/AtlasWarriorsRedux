using AtlasWarriorsGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AtlasWarriorsGame.Dungeon;

namespace GLGameApp
{
    /// <summary>
    /// State when the game is actually being played, and the map is visible
    /// </summary>
    class GameMapState : State
    {

        /// <summary>
        /// The game being played
        /// </summary>
        protected AtlasWarriorsGame.Game G;

        /// <summary>
        /// Font that the map is drawn with
        /// </summary>
        private SpriteFont MapFont;

        /// <summary>
        /// Width of a tile
        /// </summary>
        private float TileWidth;

        /// <summary>
        /// Height of a tile
        /// </summary>
        private float TileHeight;

        /// <summary>
        /// Create a game interface from a given game
        /// </summary>
        /// <param name="Game"></param>
        public GameMapState(AtlasWarriorsGame.Game Game)
        {
            this.G = Game;

            MapFont = AppContentManager.Load<SpriteFont>("GameMapState/MapFont");
            // W tends to be widest - hence, if not monospace -  will still be OK
            TileWidth = MapFont.MeasureString("W").X;
            // f tends to be tallest - hence, if not monospace -  will still be OK
            TileHeight = MapFont.MeasureString("f").Y;
        }

        /// <summary>
        /// Run logic for this state - including input
        /// </summary>
        /// <param name="GameTime">Snapshot of timing</param>
        public override void Update(GameTime GameTime)
        {
            
        }

        /// <summary>
        /// Draw this state
        /// </summary>
        /// <param name="GameTime">Snapshot of timing</param>
        public override void Draw(GameTime GameTime)
        {
            AppSpriteBatch.Begin();

            // Draw map
            for (var ix = 0; ix < G.CurrentDungeon.Width; ++ix)
            {
                for (var iy = 0; iy < G.CurrentDungeon.Height; ++iy)
                {
                    // Tile to draw
                    var currentcell = G.CurrentDungeon.GetCell(new XY(ix, iy));
                    var drawChar = CellScreenChar(currentcell);
                    var location = new Vector2 (ix * TileWidth, iy * TileHeight);
                    var color = Color.White;
                    AppSpriteBatch.DrawString(MapFont, drawChar.ToString(), location, color);
                }
            }

            AppSpriteBatch.End();
        }

        /// <summary>
        /// Get the character used to display a particular type of cell
        /// </summary>
        /// <param name="Cell">Cell to check</param>
        /// <returns>Character to display on screen for cell</returns>
        static char CellScreenChar(DungeonCell Cell)
        {
            switch (Cell)
            {
                case DungeonCell.EMPTY:
                    return ' ';
                case DungeonCell.FLOOR:
                    return '.';
                case DungeonCell.CLOSED_DOOR:
                    return '+';
                case DungeonCell.OPEN_DOOR:
                    return '-';
                case DungeonCell.WALL:
                    return '#';
                default:
                    return 'X';
            }
        }
    }
}
