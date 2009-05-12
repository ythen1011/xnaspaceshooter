using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter
{
    public class EnemySprite:AutomatedSprite
    {
        //automated sprite that bounces
        public EnemySprite(Texture2D textureImage, Vector2 position,
            Point frameSize, Point collisionOffset, int currentFrame, int sheetSize,
            Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed)
        {
        }
        public EnemySprite(Texture2D textureImage, Vector2 position,
            Point frameSize, Point collisionOffset, int currentFrame, int sheetSize,
            Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
        }

        public override Vector2 direction
        {
            get { return speed; }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //Bounces the sprites off the edges of the screen
            base.Update(gameTime, clientBounds);

            if (position.X < 0)
                speed.X *= -1;
            if (position.Y <= 0)
                speed.Y *= -1;
            if (position.X + frameSize.X - collisionOffset.X> clientBounds.Width )
                speed.X *= -1;
            if (position.Y + frameSize.Y - collisionOffset.Y > clientBounds.Height - 100)
                speed.Y *= -1;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch,Color tint )
        {
            //Draw the sprite with a tint (for higher health enemies)

            spriteBatch.Draw(textureImage,
                position,
                new Rectangle(currentFrame * frameSize.X,
                    0,
                    frameSize.X, frameSize.Y),
                tint, 0, Vector2.Zero,
                1f, SpriteEffects.None, 0);
        }
    }
}
