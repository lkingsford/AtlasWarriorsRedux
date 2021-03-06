﻿using AtlasWarriorsGame;
using MgUiCommon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        /// Object that draws the map part to the screen
        /// </summary>
        private MgUiCommon.MapViewElement MapView;

        /// <summary>
        /// Font to display log in
        /// </summary>
        private SpriteFont LogFont;

        /// <summary>
        /// Font to display stats in
        /// </summary>
        private SpriteFont StatFont;

        /// <summary>
        /// Create a game interface from a given game
        /// </summary>
        /// <param name="Game">Game object that is being played</param>
        public GameMapState(AtlasWarriorsGame.Game Game)
        {
            this.G = Game;

            MapView = new MgUiCommon.MapViewElement(Game, AppGraphicsDevice, AppContentManager);
            LogFont = AppContentManager.Load<SpriteFont>("GameMapState/LogFont");
            StatFont = AppContentManager.Load<SpriteFont>("GameMapState/StatFont");
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

            // Do turn, if player next move is set
            if (G.Player.NextMove != Player.Instruction.NOT_SET)
            {
                G.DoTurn();
            }

            if (G.GameOver)
            {
                StateStack.Add(new AtlasWarriors.LoseState(G));
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
            var MapTexture = MapView.DrawMap(GameTime, AppSpriteBatch);
            AppSpriteBatch.Begin();
            AppSpriteBatch.Draw(MapTexture, new Vector2(0.0f, 0.0f), Color.White);
            DrawLog(new Vector2(800.0f, 10.0f));
            DrawStats(new Vector2(800.0f, 100.0f));
            AppSpriteBatch.End();
        }

        /// <summary>
        /// Draw the last turns log to the screen
        /// </summary>
        /// <remarks>Sprite batch must be open</remarks>
        public void DrawLog(Vector2 drawLocation)
        {
            float currentLineY = 0.0f;

            foreach (var message in G.LastTurnMessages)
            {
                var coord = new Vector2(0, currentLineY) + drawLocation;
                var messageDimensions = LogFont.MeasureString(message.ToString());
                AppSpriteBatch.DrawString(LogFont, message.ToString(), coord, Color.White);
                currentLineY += messageDimensions.Y;
            }
        }

        /// <summary>
        /// Draw the stats to the screen
        /// </summary>
        /// <param name="drawLocation"></param>
        public void DrawStats(Vector2 drawLocation)
        {
            string statText = $"HP {G.Player.CurrentHealth} ({G.Player.MaxHealth})";
            AppSpriteBatch.DrawString(StatFont, statText, drawLocation, Color.Red);
        }
    }
}
