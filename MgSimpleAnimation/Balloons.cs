using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MgSimpleAnimation2
{
    class Balloons : AnimatedSprite
    {

        public float mySpeed = 70;        
        public string[] bColor = new string[14] { "Right", "Green1", "White", "Black", "Green2", "Blue1", "Blue2", "Green3", "Red2", "Yellow", "Orange", "Red3", "Fire", "Explosion" };        
        public int indexColor;       
        private int sumSideY=0;        
        public Vector2 Position;
        public int Width
        {
            get { return FrameWidth; }
        }

        public int Height
        {
            get { return FrameHeight; }
        }
            
        public Balloons(Vector2 position)
            : base(position)
        {
            Position = position;
            FramesPerSecond = 10;
         
            AddAnimation(1, 756, 1304, 0, "Right", 32, 39, new Vector2(200, 300));
            AddAnimation(1, 788, 1304, 0, "Green1", 32, 39, new Vector2(0, 520));
            AddAnimation(1, 820, 1304, 0, "White", 32, 39, new Vector2(0, 520));
            AddAnimation(1, 852, 1304, 0, "Black", 32, 39, new Vector2(0, 520));
            AddAnimation(1, 884, 1304, 0, "Green2", 32, 39, new Vector2(0, 520));
            AddAnimation(1, 918, 1304, 0, "Blue1", 32, 39, new Vector2(0, 520));
            AddAnimation(1, 950, 1304, 0, "Blue2", 32, 39, new Vector2(0, 520));
            AddAnimation(1, 983, 1304, 0, "Green3", 32, 39, new Vector2(0, 520));
            AddAnimation(1, 1015, 1304, 0, "Red2", 32, 39, new Vector2(0, 520));
            AddAnimation(1, 1048, 1304, 0, "Yellow", 32, 39, new Vector2(0, 520));
            AddAnimation(1, 1081, 1304, 0, "Orange", 32, 39, new Vector2(0, 520));
            AddAnimation(1, 1115, 1304, 0, "Red3", 32, 39, new Vector2(0, 520));
            AddAnimation(3, 744, 91, 0, "Fire", 20, 18, new Vector2(0, 520));
            AddAnimation(19, 0, 1348, 0, "Explosion", 39, 34, new Vector2(0, 520));

        }

        public void LoadContent(ContentManager content)
        {
            sTexture = content.Load<Texture2D>("001mario");          

        }

        public override void Update(GameTime gameTime)
        {
            sDirection = Vector2.Zero;
            HandleInput();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            sDirection *= mySpeed;
            sPosition += (sDirection * deltaTime);
            Position.X = sPosition.X;
            Position.Y = sPosition.Y;
            base.Update(gameTime);
            
        }

        private void HandleInput()
        {
            int sideY=0;
            if (animation!="Explosion")
            {
                sideY = -1;
                if (sPosition.Y <= -40)
                {
                    sPosition.Y = 540;
                    sPosition.X = generateRandomBlock(20,700);
                }
            }
            else sideY = 0;                
                sumSideY += sideY;
                sDirection += new Vector2(0, sideY);
                lastMove = "Up";
                PlayAnimation(animation);          

        }

        public int generateRandomBlock(int first, int secound)
        {
            Random rnd = new Random();
            int value = rnd.Next(first, secound);
            return value;
        }       
    }
}
