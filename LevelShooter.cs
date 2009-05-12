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
    public class LevelShooter : LevelRocks
    {
        //enemy bullet variables
        Texture2D enemyBullet;
        List<AutomatedSprite> enemyBullets = new List<AutomatedSprite>();

        int timeSinceLastEnemyBullet = 0;
        int bulletDelay = 1000;

        public LevelShooter(Game game, int lives)
            : base(game, lives)
        {
            // TODO: Construct any child components here
            intPlayerLives = lives;
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
            //load sprite
            enemyBullet = Game.Content.Load<Texture2D>(@"Images/BadBullet");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
           
            //Make enemies shoot if delay time is up and they are within a certain
            //distance of the player
            timeSinceLastEnemyBullet += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastEnemyBullet > bulletDelay)
            {
                foreach (Sprite s in enemySpriteList)
                    if (s.collisionRect.Center.X > player.collisionRect.Center.X - 100
                    && s.collisionRect.Center.X < player.collisionRect.Center.X + 100)
                    {
                        FireEnemyBullet(s.collisionRect.Center);
                        timeSinceLastEnemyBullet = 0;
                    }
            }
            //collision detection for the bullets and player
            for (int i = 0; i < enemyBullets.Count; ++i)
            {
                enemyBullets[i].Update(gameTime, Game.Window.ClientBounds);
                if (enemyBullets[i].IsOffScreen(Game.Window.ClientBounds) == true)
                {
                    enemyBullets.RemoveAt(i);
                    if (i != 0)
                         --i;
                }
                if (enemyBullets.Count != 0)
                    if (enemyBullets[i].collisionRect.Intersects(player.collisionRect))
                    {
                        --intPlayerLives;
                    }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //draw the enemy bullets
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
                SpriteSortMode.FrontToBack, SaveStateMode.None);

            foreach (Sprite s in enemyBullets)
                s.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
        public void FireEnemyBullet(Point bulletStart)
        {
            //Fires a bullet by adding it to the managed array
            enemyBullets.Add(new AutomatedSprite(enemyBullet,new Vector2(bulletStart.X,bulletStart.Y),
                new Point(40,20),new Point(12,0),0,5,new Vector2(0, 5),50));
        }

        public override void Increase()
        {
            //increases enemy num, decreases rock respawn time and decreases bullet
            //delay after the max enemies have been reached

            minRockDelay -= 50;
            maxRockDelay -= 50;

            enemyNum += enemyIncreaseRate;
            if (enemyNum > enemyMax)
            {
                bulletDelay -= 50;
                enemyNum = 4;
            }
            for (int i = 0; i < enemyNum; ++i)
            {
                enemySpriteList.Add(new EnemySprite(enemySprite,
                new Vector2(((Game1)Game).rnd.Next(0, Game.Window.ClientBounds.Width - 70), ((Game1)Game).rnd.Next(0, Game.Window.ClientBounds.Height - 120)),
                new Point(68, 37), new Point(0, 0), 0, 10, new Vector2(((Game1)Game).rnd.Next(2, 6), ((Game1)Game).rnd.Next(2, 6))));
            }
            //base.Increase();
        }
    }
}