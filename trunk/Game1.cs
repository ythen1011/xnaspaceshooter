using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SpaceShooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //declare game components(levels)
        BaseLevelShips baseLevel;
        LevelRocks levelRocks;
        LevelShooter levelShooter;
		public Random rnd;

        //player lives
        public int goodLives = 3;

        //Game over variable
        bool blnGameOver = false;
        
		
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

            //seed random gen. and start the BaseLevelShips
			rnd = new Random();
            baseLevel = new BaseLevelShips(this);
            Components.Add(baseLevel);
           
            base.Initialize();
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
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            //If the user presses enter, 
            if (blnGameOver)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    baseLevel = new BaseLevelShips(this);
                    Components.Add(baseLevel);
                    blnGameOver = false;
                }
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            if (blnGameOver)
            {
            }
            base.Draw(gameTime);
        }

        public void LevelUp(int oldRound, int lives)
        {

            //moves to the next level when the procedure is called
            switch (oldRound)
            {
                case -1:
                    Components.Clear();
                    blnGameOver = true;
                    break;
                case 0:
                    levelRocks = new LevelRocks(this, lives);
                    Components.Add(levelRocks);
                    Components.Remove(baseLevel);
                    baseLevel = null;
                    break;
                case 1:
                    levelShooter = new LevelShooter(this, lives);
                    Components.Add(levelShooter);
                    Components.Remove(levelRocks);
                    levelRocks = null;
                    break;
            }

        }

    }
}
