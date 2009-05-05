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
    public class LevelRocks : BaseLevelShips
    {
        List<AutomatedSprite> rocks = new List<AutomatedSprite>();

        Texture2D enemyRock;
        Texture2D enemyRockMedium;
        Texture2D enemyRockLarge;

        protected int minRockDelay = 1000;
        protected int maxRockDelay= 2500;
        protected int rockDelay = 1000;

        int rockType = 0;

        

        public LevelRocks(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            
        }
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }
        protected override void LoadContent()
        {
            enemyRock = Game.Content.Load<Texture2D>(@"Images/Blue hills");
            enemyRockMedium = Game.Content.Load<Texture2D>(@"Images/Blue hills");
            enemyRockLarge = Game.Content.Load<Texture2D>(@"Images/Blue hills");

            rocks.Add(new AutomatedSprite(enemyRock, new Vector2(((Game1)Game).rnd.Next(0, 500),
                0),new Point(50,50),new Point(5,5), 0,5,new Vector2(0, 5)));

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            rockDelay -= gameTime.ElapsedGameTime.Milliseconds;

            if (rockDelay <= 0)
            {
                rockDelay = ((Game1)Game).rnd.Next(minRockDelay, maxRockDelay);
                rockType = ((Game1)Game).rnd.Next(0, 2);
                

                switch (rockType)
                {
                    case 0:
                        rocks.Add(new AutomatedSprite(enemyRock, new Vector2(((Game1)Game).rnd.Next(0, 500),
                 0), new Point(50, 50), new Point(5, 5), 0, 5, new Vector2(0, 8)));
                        break;
                    case 1:
                        rocks.Add(new AutomatedSprite(enemyRockMedium, new Vector2(((Game1)Game).rnd.Next(0, 500),
                0), new Point(50, 50), new Point(5, 5), 0, 5, new Vector2(0, 6)));

                        break;
                    case 2:
                        rocks.Add(new AutomatedSprite(enemyRockLarge, new Vector2(((Game1)Game).rnd.Next(0, 500),
                0), new Point(50, 50), new Point(5, 5), 0, 5, new Vector2(0, 4)));

                        break;
                }

                
            }



            for (int i = 0; i < rocks.Count; ++i)
            {
                rocks[i].Update(gameTime, Game.Window.ClientBounds);

                if (rocks[i].collisionRect.Intersects(player.collisionRect))
                {
                    rocks[i].textureImage = explosion;
                    rocks[i].startDestroy();

                    DeadThings.Add(rocks[i]);
                    rocks.RemoveAt(i);
                    --i;

                    //decrease user lives
                    --intPlayerLives;
                }

                for (int b = 0; b < GoodBulletList.Count && i >= 0; ++b)
                {

                    if (GoodBulletList[b].collisionRect.Intersects(rocks[i].collisionRect))
                    {
                        GoodBulletList.RemoveAt(b);
                        --b;

                        rocks[i].textureImage = explosion;
                        rocks[i].startDestroy();

                        DeadThings.Add(rocks[i]);
                        
                        rocks.RemoveAt(i);
                        --i;
                    }

                }

            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
                SpriteSortMode.FrontToBack, SaveStateMode.None);

            foreach (AutomatedSprite r in rocks)
                r.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
        public override void Increase()
        {
            minRockDelay -= 50;
            maxRockDelay -= 50;

            enemyNum += enemyIncreaseRate;
            if (enemyNum > enemyMax)
                ((Game1)Game).LevelUp(1);
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
