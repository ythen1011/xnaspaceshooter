using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter
{

    //abstract class that has a collision rect, 
    public abstract class Sprite
    {
        public Texture2D textureImage;
        protected Vector2 position;
        protected Point frameSize;
        protected Point collisionOffset;
        protected int currentFrame;
        int sheetSize;
        protected int timeSinceLastFrame = 0;
        protected int millisecondsPerFrame;
        protected Vector2 speed;
        const int defaultMillisecondsPerFrame = 16;

     
        public abstract Vector2 direction
        {
            get;
        }

        public Vector2 PositionCenter
        {
            get { return new Vector2(position.X + (frameSize.X /2 -20), (int)position.Y); }
        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X + collisionOffset.X,
                    (int)position.Y + collisionOffset.Y,
                    frameSize.X - (collisionOffset.X * 2),
                    frameSize.Y - (collisionOffset.Y * 2));
            }
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            Point collisionOffset, int currentFrame, int sheetSize, Vector2 speed)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, defaultMillisecondsPerFrame)
        {
        }
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            Point collisionOffset, int currentFrame, int sheetSize, Vector2 speed,
            int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //Update animation frame
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame;
                if (currentFrame >= sheetSize)
                    currentFrame = 0;
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Draw the sprite
            
            spriteBatch.Draw(textureImage,
                position,
                new Rectangle(currentFrame * frameSize.X,
                    0,
                    frameSize.X, frameSize.Y),
                Color.White, 0, Vector2.Zero,
                1f, SpriteEffects.None, 0);
        }

        public bool IsOffScreen(Rectangle clientBounds)
        {
            if (position.Y > clientBounds.Height || position.Y < 0)
                return true;
            else
                return false;
        }
        public void startDestroy()
        {
            frameSize = new Point(65, 65);
            sheetSize = 8;
            currentFrame = 0;
            timeSinceLastFrame = 0;
            millisecondsPerFrame = 100;
        }
        public void Destroy(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame;
                
            }
        }

        public bool IsDestroyed()
        {
            if (currentFrame >= sheetSize)
                return true;
            else
                return false;
        }
    }
}
