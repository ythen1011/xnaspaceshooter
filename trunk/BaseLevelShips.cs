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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class BaseLevelShips : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;
        //create a player
        protected UserControlledSprite player;
        //create an array to manage: good bullets, enemies, dead things
        List<Sprite> goodBulletList = new List<Sprite>();
        protected List<EnemySprite> enemySpriteList = new List<EnemySprite>();
        List<Sprite> deadThings = new List<Sprite>();

        //variables to manage bullets
        public int bulletDelayMilli = 200;
        public int timeSinceLastBullet =0;
        Texture2D goodBullet;

        //explosion texture
        protected Texture2D explosion;

        //variables to manage the creation of enemies
        protected Texture2D enemySprite;
		protected int enemyNum = 4;
		protected int enemyIncreaseRate = 2;
        protected int enemyMax = 8;

        protected int intPlayerLives = 5;

        public BaseLevelShips(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            
        }

        protected List<Sprite> GoodBulletList
        {
            get
            { return goodBulletList; }
            set
            { goodBulletList = value;}
        }

        public List<Sprite> DeadThings
        {
            get
            { return deadThings; }
            set
            { deadThings = value; }
            
        }
        public UserControlledSprite Player
        {
            get
            { return player; }
            set
            { player = value; }

        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //load all images into textures
            goodBullet = Game.Content.Load<Texture2D>(@"Images/GoodBullet");
            enemySprite = Game.Content.Load<Texture2D>(@"Images/Enemy");
            explosion = Game.Content.Load<Texture2D>(@"Images/Explosion");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
           

            //construct the player
            player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Images/Player"),
                new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height - 75),
                new Point(68, 63), new Point(0,0), 6, 1, new Vector2(5, 0), 5);
			
            //constructs the initial enemies
			for (int i = 0; i < enemyNum ; ++i)
			{
				enemySpriteList.Add(new EnemySprite(enemySprite,
                new Vector2(((Game1)Game).rnd.Next(0, Game.Window.ClientBounds.Width - 70), ((Game1)Game).rnd.Next(0, Game.Window.ClientBounds.Height - 120)),
                new Point(68, 37), new Point(0,0), 0, 10, new Vector2(((Game1)Game).rnd.Next(2, 6), ((Game1)Game).rnd.Next(2, 6))));
			}
			
            
            			
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            
            //update all "dead things" and if the are completely destroyed
            //then remove from memory
            for (int i=0; i< deadThings.Count; ++i)
            {
                deadThings[i].Destroy(gameTime);
                if (deadThings[i].IsDestroyed())
                    deadThings.RemoveAt(i);
            }

            //update player
            player.Update(gameTime,Game.Window.ClientBounds);

            //fires bullets from the player
            timeSinceLastBullet += gameTime.ElapsedGameTime.Milliseconds;
            if (Keyboard.GetState().IsKeyDown(Keys.Space) &&
                timeSinceLastBullet >= bulletDelayMilli)
            {
                FireBullet();
                timeSinceLastBullet = 0;
            }

            //updates bullets and removes if they are off the screen
            for (int b = 0; b < goodBulletList.Count; ++b)
            {
                goodBulletList[b].Update(gameTime, Game.Window.ClientBounds);
                if (goodBulletList[b].IsOffScreen(Game.Window.ClientBounds))
                {
                    goodBulletList.RemoveAt(b);
                    --b;
                }
            }
            
            //collision detection for bullets and enemies
            //moves the collided enemies to the dead things
            for (int i = 0; i < enemySpriteList.Count ; ++i)
            {
                    enemySpriteList[i].Update(gameTime, Game.Window.ClientBounds);

                    for (int b = 0; b < goodBulletList.Count && i >= 0; ++b)
                    {

                        if (goodBulletList[b].collisionRect.Intersects(enemySpriteList[i].collisionRect))
                        {
                            goodBulletList.RemoveAt(b);
                            --b;

                            enemySpriteList[i].textureImage = explosion;
                            enemySpriteList[i].startDestroy();
                            deadThings.Add(enemySpriteList[i]);
                                               
                            enemySpriteList.RemoveAt(i);
                            --i;
                        }

                    }

            }
            //increase enemy amount and create them
            if (enemySpriteList.Count == 0)
                Increase();
            


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //Draw all sprites

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
                SpriteSortMode.FrontToBack, SaveStateMode.None);

            foreach (Sprite s in deadThings)
                s.Draw(gameTime, spriteBatch);

            // Draw the player
            player.Draw(gameTime, spriteBatch);

            foreach (Sprite s in goodBulletList)
                s.Draw(gameTime, spriteBatch);

            foreach (EnemySprite e in enemySpriteList)
                e.Draw(gameTime, spriteBatch);
           
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void FireBullet()
        {
            //adds bullets to the array to be managed
            goodBulletList.Add(new AutomatedSprite(goodBullet, player.PositionCenter,
                new Point(40, 20), new Point(15,0), 0, 10, new Vector2(0, -5), 50));
            
        }

        public virtual void Increase()
        {
            //increased enemynum and creates them on screen or goes to the next
            //game level
            enemyNum += enemyIncreaseRate;
            if (enemyNum > enemyMax)
                ((Game1)Game).LevelUp(0, intPlayerLives);
            for (int i = 0; i < enemyNum; ++i)
            {
                enemySpriteList.Add(new EnemySprite(enemySprite,
                new Vector2(((Game1)Game).rnd.Next(0, Game.Window.ClientBounds.Width - 70), ((Game1)Game).rnd.Next(0, Game.Window.ClientBounds.Height - 120)),
                new Point(68, 37), new Point(0,0), 0, 10, new Vector2(((Game1)Game).rnd.Next(2, 6), ((Game1)Game).rnd.Next(2, 6))));
            }
        }

        public void KillPlayer()
        {
            //resets player to the center and decreases their lives
            //Ends the game if the Player has no lives
            intPlayerLives--;
            player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Images/Player"),
                new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height - 75),
                new Point(68, 63), new Point(0, 0), 6, 1, new Vector2(5, 0), 5);
            if (intPlayerLives < 0)
                ((Game1)Game).LevelUp(-1, intPlayerLives);
        }
    }
}