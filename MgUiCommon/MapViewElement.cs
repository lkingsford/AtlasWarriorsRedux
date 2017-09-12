using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using AtlasWarriorsGame;
using Microsoft.Xna.Framework;
using System.Linq;
using Newtonsoft.Json;

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

            // Populate sprite list
            using (var spriteFileReader =
                new System.IO.StreamReader(Game.GetAssetStream("sprites_mgui.json")))
            {
                var spriteFileText = spriteFileReader.ReadToEnd();
                Sprites = JsonConvert.DeserializeObject<Dictionary<String, ConsoleCell>>
                    (spriteFileText);
            }

            // Initialise last dungeon to initial dungeon
            LastTurnDungeon = Game.CurrentDungeon;

            ResetRenderTargetAndConsole();
        }

        /// <summary>
        /// Set the render target to a new render target for the current map
        /// </summary>
        private void ResetRenderTargetAndConsole()
        {
            ConsoleBuffer = new ConsoleCell[Game.CurrentDungeon.Width, Game.CurrentDungeon.Height];
            RenderTarget = new RenderTarget2D(Device, (int)(TileWidth * Game.CurrentDungeon.Width),
                (int)(TileHeight * Game.CurrentDungeon.Height));
        }

        /// <summary>
        /// 'Sprites' (Currently, letters and colors) for actors and like that are drawn 
        /// </summary>
        private Dictionary<string, ConsoleCell> Sprites;

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
        /// Text console that will be drawn
        /// </summary>
        public ConsoleCell[,] ConsoleBuffer;

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
                ResetRenderTargetAndConsole();
            }

            // Draw to texture, not screen
            Device.SetRenderTarget(RenderTarget);

            // Draw game part
            DrawMapToConsole();
            DrawActorsToConsole();
            DrawConsoleBuffer(SpriteBatch);

            // And draw to screen again
            Device.SetRenderTarget(null);

            return RenderTarget;
        }

        /// <summary>
        /// Draw the current map to the console buffer
        /// </summary>
        protected void DrawMapToConsole()
        {
            // Draw map to Console Buffer
            for (var ix = 0; ix < Game.CurrentDungeon.Width; ++ix)
            {
                for (var iy = 0; iy < Game.CurrentDungeon.Height; ++iy)
                {
                    // Tile to draw
                    var currentcell = Game.CurrentDungeon.GetCell(new XY(ix, iy));

                    var visibility = Game.CurrentDungeon.GetVisibility(new XY(ix, iy));

                    // If visible or seen, we're drawing. 
                    // Check for visible and pass to GetTileCell.
                    if ((visibility == Dungeon.CellVisibility.VISIBLE) || 
                        (visibility == Dungeon.CellVisibility.SEEN))
                    {
                        ConsoleBuffer[ix, iy] = CellToScreen.GetTileCell(currentcell, 
                            visibility == Dungeon.CellVisibility.VISIBLE);
                    }
                }
            }
        }

        /// <summary>
        /// Draw the actors on the current map to the console buffer
        /// </summary>
        protected void DrawActorsToConsole()
        {
            // Draw characters to Console Buffer
            foreach (var actor in Game.CurrentDungeon.Actors)
            {
                // Tile to draw
                var drawCell = Sprites[actor.SpriteId];

                // Only show character if visible
                if (Game.CurrentDungeon.GetVisibility(actor.Location) == 
                    Dungeon.CellVisibility.VISIBLE)
                {
                    ConsoleBuffer[actor.Location.X, actor.Location.Y] = drawCell;
                }
            }
        }

        /// <summary>
        /// Draw the console buffer to the screen
        /// </summary>
        /// <param name="SpriteBatch">SpriteBatch to draw to</param>
        protected void DrawConsoleBuffer(SpriteBatch SpriteBatch)
        {
            // Start a new spritebatch - needed for RenderTarget
            SpriteBatch.Begin();

            // Create a texture to draw as a rectangle
            // This seems a bit rubbish
            // If there's a better way... I'll do it later
            Texture2D rect = new Texture2D(Device, 1, 1);
            rect.SetData( new[] { Color.White } );

            // Draw console to screen
            for (var ix = 0; ix < Game.CurrentDungeon.Width; ++ix)
            {
                for (var iy = 0; iy < Game.CurrentDungeon.Height; ++iy)
                {
                    var cell = ConsoleBuffer[ix, iy];
                    var location = new Vector2(ix * TileWidth, iy * TileHeight);

                    // Draw background
                    SpriteBatch.Draw(rect, new Rectangle(
                        (int)(ix * TileWidth),
                        (int)(iy * TileHeight),
                        (int)((ix + 1) * TileWidth),
                        (int)((iy + 1) * TileHeight)),
                        null,
                        cell.BackColor);

                    var character = cell.DrawChar;
                    if (MapFont.Characters.Contains(character))
                    {
                        SpriteBatch.DrawString(
                            MapFont,
                            cell.DrawChar.ToString(),
                            location,
                            cell.ForeColor);
                    }
                }
            }

            // Clear texture
            Device.Clear(Color.Black);

            SpriteBatch.End();

        }
    }
}
