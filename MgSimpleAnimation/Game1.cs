#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace MgSimpleAnimation2
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Rectangle guiRectangle;
        MainMenu main = new MainMenu();
        Player player;
        Balloons balloon;
        Random rndPos = new Random();
        Random rndColor = new Random();
        List<Balloons> listBalloons;       
        private Texture2D guiTexture;
        private double spawnTime;
        private int score;
        private int stage = 1;
        private int lives= 4;       
        private int balloonsNumber = 1;
        private int fireNumber = 5;
        SoundEffect hitToPlayer;
        SoundEffect hitToBalloon;        
        SoundEffect gameOver;
        SoundEffect theme;
        SoundEffectInstance instanceTheme;
        


        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 500;

            graphics.PreferredBackBufferWidth = 720;
            base.Initialize();
        }

        protected override void LoadContent()
        {

            if (main.gameState != MainMenu.GameState.inGame)
            {
                main = new MainMenu();
            }
            
            
            if (stage > 1 && lives > 0)
            {
                main.gameState = MainMenu.GameState.inGame;
            }

            
            RefreshVariables();                  
            
            player = new Player(new Vector2(100, 100));
         
            balloonsNumber = stage * 3;
            fireNumber = stage * 2;
            main.LoadContent(Content, new Size(720, 500));            
            
            guiRectangle = new Rectangle(0, 0, 720, 500);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            guiTexture = Content.Load<Texture2D>("001marioBg");
                      
            listBalloons = new List<Balloons>();
            player.sPosition = new Vector2(100, 100);

            player.LoadContent(Content);
            
            CreateBalloons();
            CreateFire();

            hitToPlayer = Content.Load<SoundEffect>("Sound/HitToPlayer");
            hitToBalloon = Content.Load<SoundEffect>("Sound/HitToBalloon");            
            gameOver = Content.Load<SoundEffect>("Sound/GameOver");
            theme = Content.Load<SoundEffect>("Sound/Theme2");
            instanceTheme = theme.CreateInstance();
            instanceTheme.Play();
            
        }    

        protected void CreateBalloons()
        {           
            int xPosBalloon=0;
            int yPosBalloon = 0;
           
            for (int i = 0; i < balloonsNumber; i++)
            {
                xPosBalloon = rndPos.Next(20, 700);
                yPosBalloon = rndPos.Next(500, 1000);
                balloon = new Balloons(new Vector2(xPosBalloon, yPosBalloon));
                balloon.indexColor = rndColor.Next(0, 12);
                balloon.mySpeed += 10*stage;
                balloon.animation = balloon.bColor[balloon.indexColor];
                listBalloons.Add(balloon);
                listBalloons[i].LoadContent(Content);
               
            }
      
        }

        protected void CreateFire()
        {
           
            int xPosBalloon = 0;
            int yPosBalloon = 0;
            
            for (int i = 0; i < fireNumber; i++)
            {
                xPosBalloon = rndPos.Next(20, 700);
                yPosBalloon = rndPos.Next(500, 1000);
                balloon = new Balloons(new Vector2(xPosBalloon, yPosBalloon));
                balloon.indexColor =12;
                balloon.mySpeed += 20*stage;
                balloon.animation = balloon.bColor[balloon.indexColor];
                
                listBalloons.Add(balloon);
                listBalloons[i+balloonsNumber].LoadContent(Content);
                
            }
        }
       
        protected override void UnloadContent()
        {
            
        }

             
        protected void RefreshVariables()
        {
            if (main.playCliked && lives==0)
                {                    
                    lives = 4;
                    score = 0;
                    stage = 1;         
                }
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
           

            base.Update(gameTime);            
            main.Update();

            if (main.gameState==MainMenu.GameState.inGame)
            {
                RefreshVariables();
                player.Update(gameTime);               

                foreach (var item in listBalloons)
                {
                    item.Update(gameTime);
                }
                UpdateCollision(Keyboard.GetState());        

                this.Window.Title = "YOUR SCORE: " + score.ToString() + "     LIVES: " + lives + "     STAGE: " + stage;
            }
        }

        public void UpdateCollision(KeyboardState keyState)
        {
            Rectangle rectangle1;
            Rectangle rectangle2;
            
            rectangle1 = new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Width, player.Height);

            for (int i = 0; i < listBalloons.Count; i++)
            {

                rectangle2 = new Rectangle((int)listBalloons[i].Position.X, (int)listBalloons[i].Position.Y, listBalloons[i].Width, listBalloons[i].Height);

                if (rectangle1.Intersects(rectangle2) && (keyState.IsKeyDown(Keys.Space) || Mouse.GetState().LeftButton == ButtonState.Pressed))
                {                 
                    if (listBalloons[i].animation != "Fire")
                    {
                        if (listBalloons[i].animation != "Explosion")
                        {
                            score += 100;
                            hitToBalloon.Play();
                        }
                        
                        listBalloons[i].animation = "Explosion";                      
                    }                   
                }
                
                if(!listBalloons[i].active)
                {

                    listBalloons.RemoveAt(i);
                    if ((listBalloons.Count)<= fireNumber)
                    {
                        stage++;
                        instanceTheme.Stop();
                        LoadContent();                        
                    }
                }
            }


            for (int j = 0; j < listBalloons.Count; j++)
            {

                rectangle2 = new Rectangle((int)listBalloons[j].Position.X, (int)listBalloons[j].Position.Y, listBalloons[j].Width, listBalloons[j].Height);

                if (rectangle1.Intersects(rectangle2))
                {                    
                    if (listBalloons[j].animation == "Fire")
                    {
                        listBalloons.RemoveAt(j);
                        
                        if (lives > 0)
                        {
                            lives -= player.damage;
                            fireNumber -= player.damage;
                            hitToPlayer.Play();
                        }               
                    }
                }

                if (lives == 0 && main.playCliked)
                {
                    player.animation = "Explosion";
                    gameOver.Play();
                    main.playCliked = false;
                }

                if (!player.active)
                {
                   main.gameState = MainMenu.GameState.gameOver;
                    
                    instanceTheme.Stop();
                    LoadContent();                   
                }
            }
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            
            spriteBatch.Draw(guiTexture, guiRectangle, Color.White);
            main.Draw(spriteBatch);
            if (main.gameState==MainMenu.GameState.inGame)
            {
                spawnTime += gameTime.ElapsedGameTime.TotalSeconds;
                foreach (var item in listBalloons)
                {                   
                    item.Draw(spriteBatch);
                }

                player.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public Matrix scalingMatrix { get; set; }
    }
}