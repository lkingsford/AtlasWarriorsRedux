using MgUiCommon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasWarriors
{
    /// <summary>
    /// State when the player has lost the game
    /// </summary>
    class LoseState : State
    {
        /// <summary>
        ///  Font in the title
        /// </summary>
        private readonly SpriteFont LoseTitleFont;

        /// <summary>
        ///  Font in the body
        /// </summary>
        private readonly SpriteFont LoseTextFont;

        /// <summary>
        /// Create a loss state
        /// </summary>
        /// <param name="game">Game being lost</param>
        public LoseState(AtlasWarriorsGame.Game game)
        {
            Game = game;
            LoseTitleFont = AppContentManager.Load<SpriteFont>("LoseState/LoseTitle");
            LoseTextFont = AppContentManager.Load<SpriteFont>("LoseState/LoseText");
            LastState = Keyboard.GetState();
        }

        /// <summary>
        /// State as of the previous Update
        /// </summary>
        private KeyboardState LastState;

        /// <summary>
        /// Game being lost
        /// </summary>
        private readonly AtlasWarriorsGame.Game Game;

        /// <summary>
        /// Draw this state
        /// </summary>
        /// <param name="GameTime">Snapshot of timing</param>
        public override void Draw(GameTime GameTime)
        {
            AppSpriteBatch.Begin();
            AppSpriteBatch.DrawString(LoseTitleFont, Game.EndGameTitle, new Vector2(100, 100),
                Color.White);
            AppSpriteBatch.DrawString(LoseTextFont, Game.EndGameText, new Vector2(100, 250),
                Color.GhostWhite);
            AppSpriteBatch.DrawString(LoseTextFont, "(Press any key to quit)",
                new Vector2(100, 300), Color.GhostWhite);
            AppSpriteBatch.End();
        }

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
                    StateStack.Clear();
                }
            }
            LastState = currentState;
        }
    }
}