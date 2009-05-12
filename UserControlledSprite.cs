using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter
{
    public class UserControlledSprite:Sprite
    {

        enum playerdirection {left, right, stop};
        
        //constructors
        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, Point collisionOffset, int currentFrame, int sheetSize,
            Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed)
        {
        }
        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, Point collisionOffset, int currentFrame, int sheetSize,
            Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
        }

       

        public override Vector2 direction
        {
            get
            {
                //Return direction based on input from mouse and gamepad
                Vector2 inputDirection = Vector2.Zero;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    inputDirection.X -= 1;
                    
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    inputDirection.X += 1;
                

                GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
                if (gamepadState.ThumbSticks.Left.X != 0)
                    inputDirection.X += gamepadState.ThumbSticks.Left.X;
                if (gamepadState.ThumbSticks.Left.Y != 0)
                    inputDirection.Y += gamepadState.ThumbSticks.Left.Y;
                return inputDirection * speed;
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Move the sprite according to the direction property
                       
            position += direction;
            if (direction.X < 0)
                MoveDirection(playerdirection.left, gameTime);
            else if (direction.X > 0)
                MoveDirection(playerdirection.right, gameTime);
            else
                MoveDirection(playerdirection.stop, gameTime);

                
            // If the sprite is off the screen, put it back in play
            if (position.X < 0)
                position.X = 0;
            if (position.X > clientBounds.Width - frameSize.X)
                position.X = clientBounds.Width - frameSize.X;
            
            //base.Update(gameTime, clientBounds);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
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
        private void MoveDirection(playerdirection direction, GameTime gameTime)
        {
            //animates the ships left and right movement so that it leans
            //according to the direction
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                switch (direction)
                {
                    case playerdirection.left:
                        --currentFrame;
                        if (currentFrame <= 0)
                            currentFrame = 0;
                        break;
                    case playerdirection.right:
                        ++currentFrame;
                        if (currentFrame >= 12)
                            currentFrame = 12;
                        break;
                    case playerdirection.stop:
                        if (currentFrame > 6)
                            --currentFrame;
                        else if (currentFrame < 6)
                            ++currentFrame;
                        break;
                }

                
            }
        }

    }
}
