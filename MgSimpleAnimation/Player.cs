using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MgSimpleAnimation2
{
    class Player : AnimatedSprite    {

        float mySpeed = 100;
        public Vector2 Position;     
        public int damage=1;
        SoundEffect hammerHit;

        public int Width
        {
            get { return FrameWidth; }
        }

        public int Height
        {
            get { return FrameHeight; }
        }

        public Player(Vector2 position) : base(position) 
        {
            Position = position;
            FramesPerSecond = 10;
           
            AddAnimation(6, 34, 1186,0, "Right", 29, 39, new Vector2(0, 0));
            AddAnimation(6, 34, 1230, 0, "Left", 29, 39, new Vector2(0, 0));
            AddAnimation(4, 572, 1290, 0, "Down", 29, 39, new Vector2(0, 0));
            AddAnimation(4, 1022, 585, 0, "Up", 33, 39, new Vector2(0, 0));
            AddAnimation(4, 88, 363, 0, "HitRight", 32, 51, new Vector2(0, 0));
            AddAnimation(4, 868, 1235, 0, "HitLeft", 34, 51, new Vector2(0, 0));
            AddAnimation(5, 34, 235, 0, "HitDown", 30, 58, new Vector2(0, 0));
            AddAnimation(5, 34, 474, 0, "HitUp", 30, 51, new Vector2(0, 0));
            AddAnimation(10, 351, 217, 0, "Explosion", 24, 36, new Vector2(0, 0));          
           
        }

        public void LoadContent(ContentManager content)
        {
            sTexture = content.Load<Texture2D>("001mario");
            hammerHit = content.Load<SoundEffect>("Sound/HammerHit");
        }

        public override void Update(GameTime gameTime)
        {
            sDirection = Vector2.Zero;
            HandleInput(Keyboard.GetState());
            
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            sDirection *= mySpeed;
            sPosition += (sDirection*deltaTime);
            Position.X = sPosition.X;
            Position.Y = sPosition.Y;
            base.Update(gameTime);
            UpdateDisappear();
            
        }

        private void UpdateDisappear()
        {
            if (sPosition.X > 715)
            {
                sPosition.X = -20;
            }
            
            else if (sPosition.X < -21)
            {
                sPosition.X = 715;
            }
            
            else if (sPosition.Y > 495)
            {
                sPosition.Y = -20;
            }

            else if (sPosition.Y < -21)
            {
                sPosition.Y = 495;
            }
        }


        private void HandleInput(KeyboardState keyState)
        {
            if (animation != "Explosion" && (active))
            {
                if ((keyState.IsKeyDown(Keys.W) || keyState.IsKeyDown(Keys.Up)) && (keyState.IsKeyUp(Keys.Space) && Mouse.GetState().LeftButton == ButtonState.Released))
                {
                    //Move up
                    sDirection += new Vector2(0, -1);
                    lastMove = "Up";
                    PlayAnimation("Up");
                }
                if ((keyState.IsKeyDown(Keys.A) || keyState.IsKeyDown(Keys.Left)) && (keyState.IsKeyUp(Keys.Space) && Mouse.GetState().LeftButton == ButtonState.Released))
                {
                    //Move left
                    sDirection += new Vector2(-1, 0);
                    lastMove = "Left";
                    PlayAnimation("Left");
                }
                if ((keyState.IsKeyDown(Keys.S) || keyState.IsKeyDown(Keys.Down)) && (keyState.IsKeyUp(Keys.Space) && Mouse.GetState().LeftButton == ButtonState.Released))
                {
                    //Move down
                    sDirection += new Vector2(0, 1);
                    lastMove = "Down";
                    PlayAnimation("Down");
                }
                if ((keyState.IsKeyDown(Keys.D) || keyState.IsKeyDown(Keys.Right)) && (keyState.IsKeyUp(Keys.Space) && Mouse.GetState().LeftButton == ButtonState.Released))
                {
                    //Move right
                    sDirection += new Vector2(1, 0);
                    lastMove = "Right";
                    PlayAnimation("Right");
                }
                if (keyState.IsKeyDown(Keys.Space) || Mouse.GetState().LeftButton == ButtonState.Pressed)
                {

                    hammerHit.Play();
                    PlayAnimation("Hit" + lastMove);

                }
                if (keyState.IsKeyUp(Keys.Space) && Mouse.GetState().LeftButton == ButtonState.Released)
                {
                    PlayAnimation(lastMove);
                }

                if (keyState.IsKeyDown(Keys.RightControl) || Mouse.GetState().RightButton == ButtonState.Pressed)
                {
                    mySpeed = 250;
                }

                if (keyState.IsKeyUp(Keys.RightControl) && Mouse.GetState().RightButton == ButtonState.Released)
                {
                    mySpeed = 100;
                }
            }
            else PlayAnimation("Explosion");
        }
    }
}
