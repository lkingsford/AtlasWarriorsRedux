using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using AtlasWarriorsGame;
using Microsoft.Xna.Framework;
using System.Linq;

namespace MgUiCommon
{
    /// <summary>
    /// 'Element' of the game screen which draws the map part
    /// </summary>
    public class MapViewElement
    {
        /// <summary>
        /// Constructor, with required parts to run
        /// </summary>
        public MapViewElement(AtlasWarriorsGame.Game Game, 
                              GraphicsDevice Device,
                              ContentManager Content)
        {
            this.Game = Game;
            this.Content = Content;
            this.Device = Device;

            // Initialise bits for drawing
            MapFont = Content.Load<SpriteFont>("GameMapState/MapFont");
            // W tends to be widest - hence, if not monospace -  will still be OK
            TileWidth = MapFont.MeasureString("W").X;
            // f tends to be tallest - hence, if not monospace -  will still be OK
            TileHeight = MapFont.MeasureString("f").Y;

            // Initialise last dungeon to initial dungeon
            LastTurnDungeon = Game.CurrentDungeon;

            ResetRenderTarget();
        }

        /// <summary>
        /// Set the render target to a new render target for the current map
        /// </summary>
        private void ResetRenderTarget()
        {
            RenderTarget = new RenderTarget2D(Device, (int)(TileWidth * Game.CurrentDungeon.Width),
                (int)(TileHeight * Game.CurrentDungeon.Height));
        }

        /// <summary>
        /// The game which is being played
        /// </summary>
        private AtlasWarriorsGame.Game Game;

        /// <summary>
        /// Currently active GraphicsDevice
        /// </summary>
        private GraphicsDevice Device;

        /// <summary>
        /// Where to render to
        /// </summary>
        private RenderTarget2D RenderTarget;

        /// <summary>
        /// Content manager containing game assets
        /// </summary>
        private ContentManager Content;

        /// <summary>
        /// Font to display map with
        /// </summary>
        private SpriteFont MapFont;

        /// <summary>
        /// Width of each tile
        /// </summary>
        private float TileWidth;

        /// <summary>
        /// Height of each tile
        /// </summary>
        private float TileHeight;

        /// <summary>
        /// The dungeons used in the last draw operation.
        /// Used to create a new RenderTarget2D if the dungeon changes (and the size may have 
        /// changed)
        /// </summary>
        private Dungeon LastTurnDungeon;

        /// <summary>
        /// Draw the current game map to a buffer
        /// Requires that sprite batch be ended
        /// </summary>
        /// <param name="GameTime">Elapsed time since last frame</param>
        /// <param name="SpriteBatch">Sprite batch to draw with</param>
        /// <returns>Texture containing whole map</returns>
        public RenderTarget2D DrawMap(Microsoft.Xna.Framework.GameTime GameTime,
                                      SpriteBatch SpriteBatch)
        {
            // Create a new texture if new map
            if (LastTurnDungeon != Game.CurrentDungeon)
            {
                ResetRenderTarget();
            }

            // Draw to texture, not screen
            Device.SetRenderTarget(RenderTarget);

            // Start a new spritebatch - needed for RenderTarget
            SpriteBatch.Begin();

            // Clear texture
            Device.Clear(Color.Black);

            // Draw map

            // Get list of character locations not to draw
            var actorLocations = Game.CurrentDungeon.Actors.Select(i => i.Location);

            // Draw map
            for (var ix = 0; ix < Game.CurrentDungeon.Width; ++ix)
            {
                for (var iy = 0; iy < Game.CurrentDungeon.Height; ++iy)
                {
                    // Tile to draw
                    var currentcell = Game.CurrentDungeon.GetCell(new XY(ix, iy));

                    var visibility = Game.CurrentDungeon.GetVisibility(new XY(ix, iy));

                    // Repeating, as show even if actor there where SEEN
                    if (visibility == Dungeon.CellVisibility.VISIBLE)
                    {
                        if (!actorLocations.Any(i => (i.X == ix) && (i.Y == iy)))
                        {
                            var drawChar = UiCommon.CellToScreen.CellScreenChar(currentcell);
                            var location = new Vector2(ix * TileWidth,
                                iy * TileHeight);
                            var color = Color.White;
                            SpriteBatch.DrawString(
                                MapFont,
                                drawChar.ToString(),
                                location,
                                color);
                        }
                    }
                    else if (visibility == Dungeon.CellVisibility.SEEN)
                    {
                        var drawChar = UiCommon.CellToScreen.CellScreenChar(currentcell);
                        var location = new Vector2(ix * TileWidth,
                            iy * TileHeight);
                        var color = Color.DimGray;
                        SpriteBatch.DrawString(
                            MapFont,
                            drawChar.ToString(),
                            location,
                            color);
                    }
                }
            }

            // Draw characters
            foreach (var actor in Game.CurrentDungeon.Actors)
            {
                // Tile to draw
                var drawChar = UiCommon.CellToScreen.ActorToChar(actor);
                var location = new Vector2(actor.Location.X * TileWidth,
                    actor.Location.Y * TileHeight);
                // Only show character if visible
                if (Game.CurrentDungeon.GetVisibility(actor.Location) == 
                    Dungeon.CellVisibility.VISIBLE)
                {
                    var color = Color.White;
                    SpriteBatch.DrawString(
                        MapFont,
                        drawChar.ToString(),
                        location,
                        color);
                }
            }

            SpriteBatch.End();

            // And draw to screen again
            Device.SetRenderTarget(null);

            return RenderTarget;
        }
    }
}
