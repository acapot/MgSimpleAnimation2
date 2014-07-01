using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MgSimpleAnimation2
{
    abstract class AnimatedSprite
    {
        public Vector2 sPosition;
        protected Texture2D sTexture;       
        public int frameIndex;
        private double timeElapsed;
        private double timeToUpdate;
        protected Vector2 sDirection = Vector2.Zero;
        public string currentAnimation= "Right";
        protected string lastMove= "Right";
        public int FrameWidth;
        public int FrameHeight;
        public bool active=true;
        public string animation;

        public AnimatedSprite(Vector2 position) 
        {
            sPosition = position;
        }

        public Dictionary<string, Rectangle[]> sAnimation = new Dictionary<string, Rectangle[]>();

        public int FramesPerSecond        
        {
            set{ timeToUpdate = (1f / value);}
    
        }
        public void AddAnimation(int frames, int xPos, int yPos, int xStartFrame, string name, int width,int height,Vector2 offset) 
        {

            FrameWidth = width;
            FrameHeight = height;
            Rectangle[] Rectangles = new Rectangle[frames];
            for (int i = 0; i < frames; i++)
            {
                Rectangles[i] = new Rectangle(((i + xStartFrame) * FrameWidth) + xPos, yPos, FrameWidth, FrameHeight);                
            }
            sAnimation.Add(name, Rectangles);
        }
        public virtual void Update(GameTime gametime) 
        {

           
                timeElapsed += gametime.ElapsedGameTime.TotalSeconds;
                if (timeElapsed > timeToUpdate)
                {
                    timeElapsed -= timeToUpdate;
                    if (frameIndex < sAnimation[currentAnimation].Length - 1)
                    {
                        frameIndex++;
                    }
                    else
                    {
                        if (currentAnimation=="Explosion")
                             active=false;

                        frameIndex = 0;
                    }
                }
        }

       
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sTexture, sPosition, sAnimation[currentAnimation][frameIndex], Color.White);
        }

        public void PlayAnimation(String name) 
        {
            if (currentAnimation != name) 
            {
                currentAnimation = name;
                frameIndex = 0;
            }
        }

    }
}
