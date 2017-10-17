using MainGame.Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MainGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameStateManager gsm;
        public static Game1 Instance;

        public Game1()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // These are general bounds of game window. You can assing constant values 
            // or use the GraphicsDevice display mode which in that case will be 1920x1080
            graphics.PreferredBackBufferWidth = 1280; //GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = 720; //GraphicsDevice.DisplayMode.Height;
            //graphics.ToggleFullScreen();
            graphics.ApplyChanges();

            base.Initialize();

            GraphicsDevice.Viewport = new Viewport(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            GlobalVariables.GAME_WINDOW_WIDTH = graphics.PreferredBackBufferWidth;
            GlobalVariables.GAME_WINDOW_HEIGHT = graphics.PreferredBackBufferHeight;
            GlobalVariables.GAME_WINDOW_SCALE = (graphics.PreferredBackBufferWidth * graphics.PreferredBackBufferHeight) / (1280 * 720);

            gsm = new GameStateManager();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GlobalVariables.fontTitle = this.Content.Load<SpriteFont>(GlobalVariables.FONT_TITLE);
            GlobalVariables.fontSimple = this.Content.Load<SpriteFont>(GlobalVariables.FONT_SIMPLE);
            GlobalVariables.Background = this.Content.Load<Texture2D>(GlobalVariables.BACKGROUND);
            GlobalVariables.Tileset_1 = this.Content.Load<Texture2D>(GlobalVariables.TILESET_L1);

            GlobalVariables.Player_Main = this.Content.Load<Texture2D>(GlobalVariables.PLAYER_MAIN);
            GlobalVariables.Armor_Red = this.Content.Load<Texture2D>(GlobalVariables.ARMOR_RED);
            GlobalVariables.Robe_Black = this.Content.Load<Texture2D>(GlobalVariables.ROBE_BLACK);
            GlobalVariables.Skill_Sword = this.Content.Load<Texture2D>(GlobalVariables.SKILL_SWORD);
            //GlobalVariables.Background = this.Content.Load<Texture2D>(GlobalVariables.BACKGROUND);
            //GlobalVariables.E_SkeletonGreenWalk = Content.Load<Texture2D>(GlobalVariables.SKELETON_GREEN_WALK);
            //GlobalVariables.E_SkeletonGreenDead = Content.Load<Texture2D>(GlobalVariables.SKELETON_GREEN_DEAD);
            //GlobalVariables.E_GhostBlueWalk = Content.Load<Texture2D>(GlobalVariables.GHOST_BLUE_WALK);
            //GlobalVariables.E_BossWalk = Content.Load<Texture2D>(GlobalVariables.BOSS_WALK);
            GlobalVariables.blackRect = new Texture2D(GraphicsDevice, 1, 1);
            GlobalVariables.blackRect.SetData(new[] { Color.Black } );

            GlobalVariables.whiteRect = new Texture2D(GraphicsDevice, 1, 1);
            GlobalVariables.whiteRect.SetData(new[] { Color.White });

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
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            gsm._Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            gsm._Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);

        }
    }
}
