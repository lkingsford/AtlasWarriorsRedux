using Android.Util;
using AtlasWarriorsGame;
using MgUiCommon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Extended;

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
        /// Whether to show the lines on the screen indicating touch markers
        /// </summary>
        private bool ShowGuides = true;

        /// <summary>
        /// Object that draws the map part to the screen
        /// </summary>
        private MgUiCommon.MapViewElement MapView;

        /// <summary>
        /// Font to display log in
        /// </summary>
        private SpriteFont LogFont;

        /// <summary>
        /// Font to display statistics in
        /// </summary>
        private SpriteFont StatFont;

        /// <summary>
        /// Create a game interface from a given game
        /// </summary>
        /// <param name="game">Game object that is being played</param>
        /// <param name="metrics">Display metrics of mobile</param>
        public GameMapState(AtlasWarriorsGame.Game game, DisplayMetrics metrics)
        {
            this.G = game;

            LogFont = AppContentManager.Load<SpriteFont>("GameMapState/LogFont");
            MapView = new MgUiCommon.MapViewElement(game, AppGraphicsDevice, AppContentManager);

            Dpi = metrics.Xdpi;
        }

        // DPI of screen
        private float Dpi = 0;

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
        /// <param name="Position"></param>
        private void ProcessTouch(Vector2 Position)
        {
            // We divide the screen into 3x4 regions
            float regionWidth = ScreenWidth / 3;
            float regionHeight = ScreenHeight / 4;

            // Get the touched region
            int xRegion = (int)(Position.X / regionWidth);
            int yRegion = (int)(Position.Y / regionHeight);

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
            // Draw map to texture
            // Needs to happen before sprite batch begins, as it needs to begin it itself with a
            // different RenderTarget
            var MapTexture = MapView.DrawMap(GameTime, AppSpriteBatch);

            // Scale, in case high DPI
            float scale = (float)ScreenWidth / (float)MapTexture.Width;

            // Scale for log
            // Should stay a consistent size... it's twice the DPI of my real life screen, which
            // means that 14pt is about half the physical size on the phone as it is on my screen
            // which feels about right to me, I guess
            float logScale = Dpi / 220;

            AppSpriteBatch.Begin();

            // Draw map texture to screen
            AppSpriteBatch.Draw(MapTexture,
                // VS's clam that (float) is redundant is a lie. If it's not cast - no right side
                // off
                new Rectangle(0, 0, (int)((float)MapTexture.Width * scale),
                    (int)((float)MapTexture.Height * scale)),
                Color.White);                

            // Draw guides
            if (ShowGuides)
            {
                float regionWidth = ScreenWidth / 3;
                float regionHeight = ScreenHeight / 4;

                for (int ix = 0; ix < 3; ix++)
                {
                    AppSpriteBatch.DrawLine(new Vector2(ix * regionWidth, 0), 
                        new Vector2(ix * regionWidth, ScreenHeight), Color.Gray);
                }

                for (int iy = 0; iy < 4; iy++)
                {
                    AppSpriteBatch.DrawLine(new Vector2(0, iy * regionHeight),
                        new Vector2(ScreenWidth, iy * regionHeight), Color.Gray);
                }
            }

            // Draw log on bottom of screen
            float currentLineY = ScreenHeight;
            foreach (var message in G.LastTurnMessages)
            {
                currentLineY -= logScale * LogFont.MeasureString(message.ToString()).Y;
            }

            foreach (var message in G.LastTurnMessages)
            {
                var coord = new Vector2(0, currentLineY);
                var messageDimensions = LogFont.MeasureString(message.ToString());
                AppSpriteBatch.DrawString(LogFont, message.ToString(), coord, Color.White, 0.0f, 
                    new Vector2(0.0f, 0.0f), logScale, SpriteEffects.None, 1.0f);
                currentLineY += messageDimensions.Y;
            }

            // Draw stats

            AppSpriteBatch.End();
        }
    }
}
