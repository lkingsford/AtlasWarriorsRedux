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
        /// <param name="Game">Game object that is being played</param>
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

            // Get list of character locations not to draw
            var actorLocations = G.CurrentDungeon.Actors.Select(i=>i.Location);

            // Draw map
            for (var ix = 0; ix < G.CurrentDungeon.Width; ++ix)
            {
                for (var iy = 0; iy < G.CurrentDungeon.Height; ++iy)
                {
                    // Tile to draw
                    var currentcell = G.CurrentDungeon.GetCell(new XY(ix, iy));
                    if (!actorLocations.Any(i=>(i.X==ix) && (i.Y == iy)))
                    {
                        var drawChar = UiCommon.CellToScreen.CellScreenChar(currentcell);
                        var location = new Vector2 (ix * TileWidth, iy * TileHeight);
                        var color = Color.White;
                        AppSpriteBatch.DrawString(MapFont, drawChar.ToString(), location, color);
                    }
                }
            }

            // Draw characters
            foreach (var actor in G.CurrentDungeon.Actors)
            {
                // Tile to draw
                var drawChar = UiCommon.CellToScreen.ActorToChar(actor);
                var location = new Vector2 (actor.Location.X * TileWidth,
                    actor.Location.Y * TileHeight);
                var color = Color.White;
                AppSpriteBatch.DrawString(MapFont, drawChar.ToString(), location, color);
            }

            AppSpriteBatch.End();
        }

    }
}
