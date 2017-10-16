﻿using MainGame.Maps;
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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            
            

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
           
            base.Initialize();

            GlobalVariables.GAME_WINDOW_WIDTH = graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
            GlobalVariables.GAME_WINDOW_HEIGHT = graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
            GlobalVariables.GAME_WINDOW_SCALE = 1;

            gsm = new GameStateManager();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GlobalVariables.fontTitle = Content.Load<SpriteFont>(GlobalVariables.FONT_TITLE);
            GlobalVariables.fontSimple = Content.Load<SpriteFont>(GlobalVariables.FONT_SIMPLE);
            // GlobalVariables.E_SkeletonGreenWalk = Content.Load<Texture2D>(GlobalVariables.SKELETON_GREEN_WALK);
            // GlobalVariables.E_SkeletonGreenDead = Content.Load<Texture2D>(GlobalVariables.SKELETON_GREEN_DEAD);
            //GlobalVariables.E_GhostBlueWalk = Content.Load<Texture2D>(GlobalVariables.GHOST_BLUE_WALK);
            //GlobalVariables.E_BossWalk = Content.Load<Texture2D>(GlobalVariables.BOSS_WALK);
            GlobalVariables.blackRect = new Texture2D(GraphicsDevice, 1, 1);
            GlobalVariables.blackRect.SetData(new[] { Color.Black } );

        }
        // TODO: use this.Content to load your game content here
    

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            //GlobalVariables.blackRect.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            gsm._Update();
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
            base.Draw(gameTime);
            gsm._Draw(spriteBatch);
            
            spriteBatch.End();
            
        }
    }
}
