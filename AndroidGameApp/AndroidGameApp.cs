using Android.Util;
using MgUiCommon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace AndroidGameApp
{
    /// <summary>
    /// Main class for the GL version of Atlas Warriors
    /// </summary>
    public class AndroidGameApp : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        /// <summary>
        /// Current stack of states. 
        /// Front of stack is the current state.
        /// </summary>
        List<State> States = new List<State>();

        /// <summary>
        /// Metrics of the display
        /// </summary>
        private DisplayMetrics Metrics;

        /// <summary>
        /// Current state
        /// </summary>
        State CurrentState
        {
            get
            {
                // Supposedly, .Last is optimized for List<>... I hope that LastOrDefault is too
                return States.LastOrDefault();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="metrics">Screen metrics</param>
        public AndroidGameApp(DisplayMetrics metrics)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Metrics = metrics;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Initialise static state fields
            State.AppContentManager = Content;
            State.AppGraphicsDevice = GraphicsDevice;
            State.AppSpriteBatch = spriteBatch;
            State.ScreenWidth = GraphicsDevice.DisplayMode.Width;
            State.ScreenHeight = GraphicsDevice.DisplayMode.Height;

            // Set up GPU
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.ApplyChanges();

            // Create a new game, and make the UI on top of the stack
            States.Add(new GameMapState(new AtlasWarriorsGame.Game(), Metrics));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="GameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime GameTime)
        {
            // If the stack is empty, then it's quittin time
            if (CurrentState == null)
                Exit();

            // Otherwise, that's the thing we're doing
            CurrentState?.Update(GameTime);

            base.Update(GameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="GameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime GameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            CurrentState?.Draw(GameTime);

            base.Draw(GameTime);
        }
    }
}
