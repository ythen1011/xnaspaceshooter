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

        Texture2D enemyBullet;
        List<AutomatedSprite> enemyBullets = new List<AutomatedSprite>();

        int timeSinceLastEnemyBullet = 0;
        int bulletDelay = 1000;

        public LevelShooter(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
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
            enemyBullet = Game.Content.Load<Texture2D>(@"Images/BadBullet");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            //Buggy method to make the enemies shoot
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
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
                SpriteSortMode.FrontToBack, SaveStateMode.None);

            foreach (Sprite s in enemyBullets)
                s.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
        public void FireEnemyBullet(Point bulletStart)
        {
            enemyBullets.Add(new AutomatedSprite(enemyBullet,new Vector2(bulletStart.X,bulletStart.Y),
                new Point(40,20),new Point(12,0),0,5,new Vector2(0, 5),50));
        }

        public override void Increase()
        {
            bulletDelay -= 15;

            minRockDelay -= 50;
            maxRockDelay -= 50;

            enemyNum += enemyIncreaseRate;
            if (enemyNum > enemyMax)
                ((Game1)Game).LevelUp(2);
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