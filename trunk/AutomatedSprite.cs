using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter
{
    //Sprite that moves on its own
    public class AutomatedSprite:Sprite
    {
        //constructors
        public AutomatedSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, Point collisionOffset, int currentFrame, int sheetSize,
            Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed)
        {
        }
        public AutomatedSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, Point collisionOffset, int currentFrame, int sheetSize,
            Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
        }
        //returns the speed
        public override Vector2 direction
        {
            get { return speed; }
        }
        
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //moves the sprite
            position += direction;
             
            base.Update(gameTime, clientBounds);
        }

        
    }
}
