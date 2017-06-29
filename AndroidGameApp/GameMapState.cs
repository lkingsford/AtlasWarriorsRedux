﻿using AtlasWarriorsGame; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace AndroidGameApp
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
        /// State as of the previous Update
        /// </summary>
        private KeyboardState LastState;

        /// <summary>
        /// Run logic for this state - including input
        /// </summary>
        /// <param name="GameTime">Snapshot of timing</param>
        public override void Update(GameTime GameTime)
        {
            // Get the current state, get the last state, and any new buttons are acted upon
            var currentState = Keyboard.GetState();
            foreach (var i in currentState.GetPressedKeys())
            {
                if (LastState.IsKeyUp(i))
                {
                    KeyPressed(i);
                }
            }
            LastState = currentState;

            // Get touch state
            var touchState = TouchPanel.GetState();

            // Iterate through. Move the dude if in the 9 spaces.
            foreach (var touch in touchState)
            {
                if (touch.State == TouchLocationState.Pressed)
                {
                    ProcessTouch(touch.Position);
                }
            }

            // Do turn, if player next move is set
            if (G.Player.NextMove != Player.Instruction.NOT_SET)
            {
                G.DoTurn();
            }
        }

        /// <summary>
        /// Act upon a pressed screen
        /// </summary>
        /// <param name="position"></param>
        private void ProcessTouch(Vector2 position)
        {
            // We divide the screen into 3x4 regions
            float regionWidth = AppGraphicsDevice.DisplayMode.Width / 3;
            float regionHeight = AppGraphicsDevice.DisplayMode.Height / 4;

            // Get the touched region
            int xRegion = (int)(position.X / regionWidth);
            int yRegion = (int)(position.Y / regionHeight);

            if (xRegion == 0 && yRegion == 0)
            {
                G.Player.NextMove = Player.Instruction.MOVE_NW;
            }
            else if (xRegion == 1 && yRegion == 0)
            { 
                G.Player.NextMove = Player.Instruction.MOVE_N;
            }
            else if (xRegion == 2 && yRegion == 0)
            { 
                G.Player.NextMove = Player.Instruction.MOVE_NE;
            }
            else if (xRegion == 0 && yRegion == 1)
            { 
                G.Player.NextMove = Player.Instruction.MOVE_W;
            }
            else if (xRegion == 1 && yRegion == 1)
            { 
            }
            else if (xRegion == 2 && yRegion == 1)
            { 
                G.Player.NextMove = Player.Instruction.MOVE_E;
            }
            else if (xRegion == 0 && yRegion == 2)
            { 
                G.Player.NextMove = Player.Instruction.MOVE_SW;
            }
            else if (xRegion == 1 && yRegion == 2)
            { 
                G.Player.NextMove = Player.Instruction.MOVE_S;
            }
            else if (xRegion == 2 && yRegion == 2)
            { 
                G.Player.NextMove = Player.Instruction.MOVE_SE;
            }

        }

        /// <summary>
        /// Act upon a pressed key
        /// </summary>
        /// <param name="Key"></param>
        private void KeyPressed(Keys Key)
        {
            switch (Key)
            {
                case Keys.H:
                case Keys.Left:
                case Keys.NumPad4:
                    G.Player.NextMove = Player.Instruction.MOVE_W;
                    break;
                case Keys.K:
                case Keys.Up:
                case Keys.NumPad8:
                    G.Player.NextMove = Player.Instruction.MOVE_N;
                    break;
                case Keys.L:
                case Keys.Right:
                case Keys.NumPad6:
                    G.Player.NextMove = Player.Instruction.MOVE_E;
                    break;
                case Keys.J:
                case Keys.Down:
                case Keys.NumPad2:
                    G.Player.NextMove = Player.Instruction.MOVE_S;
                    break;
                case Keys.Y:
                case Keys.NumPad7:
                    G.Player.NextMove = Player.Instruction.MOVE_NW;
                    break;
                case Keys.U:
                case Keys.NumPad9:
                    G.Player.NextMove = Player.Instruction.MOVE_NE;
                    break;
                case Keys.B:
                case Keys.NumPad1:
                    G.Player.NextMove = Player.Instruction.MOVE_SW;
                    break;
                case Keys.N:
                case Keys.NumPad3:
                    G.Player.NextMove = Player.Instruction.MOVE_SE;
                    break;

            }
        }

        /// <summary>
        /// Draw this state
        /// </summary>
        /// <param name="GameTime">Snapshot of timing</param>
        public override void Draw(GameTime GameTime)
        {
            AppSpriteBatch.Begin();

            // Get list of character locations not to draw
            var actorLocations = G.CurrentDungeon.Actors.Select(i => i.Location);

            // Scale, in case high DPI
            float scale = AppGraphicsDevice.DisplayMode.Width /
                (TileWidth * (float)(G.CurrentDungeon.Width + 2));

            // Draw map
            for (var ix = 0; ix < G.CurrentDungeon.Width; ++ix)
            {
                for (var iy = 0; iy < G.CurrentDungeon.Height; ++iy)
                {
                    // Tile to draw
                    var currentcell = G.CurrentDungeon.GetCell(new XY(ix, iy));
                    if (!actorLocations.Any(i => (i.X == ix) && (i.Y == iy)))
                    {
                        var drawChar = UiCommon.CellToScreen.CellScreenChar(currentcell);
                        var location = new Vector2(scale * ix * TileWidth, 
                            scale * iy * TileHeight);
                        var color = Color.White;
                        AppSpriteBatch.DrawString(
                            MapFont,
                            drawChar.ToString(),
                            location,
                            color, 
                            0,
                            new Vector2(0, 0),
                            scale,
                            SpriteEffects.None,
                            1);
                    }
                }
            }

            // Draw characters
            foreach (var actor in G.CurrentDungeon.Actors)
            {
                // Tile to draw
                var drawChar = UiCommon.CellToScreen.ActorToChar(actor);
                var location = new Vector2(scale * actor.Location.X * TileWidth,
                    scale * actor.Location.Y * TileHeight);
                var color = Color.White;
                AppSpriteBatch.DrawString(
                    MapFont, 
                    drawChar.ToString(), 
                    location, 
                    color,
                    0,
                    new Vector2(0,0),
                    scale,
                    SpriteEffects.None,
                    1);
            }

            AppSpriteBatch.End();
        }

    }
}