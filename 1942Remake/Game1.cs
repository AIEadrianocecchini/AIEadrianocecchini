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
using Microsoft.Xna.Framework.Media;
#endregion

namespace _1942Remake
{
   //main
    public class Game1 : Game
    {
        //state enum
        public enum State
        {
            Menu, 
            Game,
            Gameover
        }

        SoundEffectInstance BGSongSFXIns;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random = new Random();
        public int enemyBulletDamage;
        public Texture2D menuImage, menuImage2;
        public Texture2D gameOverImage;

        //Asteroid List + Enemy List + Explosion List
        List<Asteroid> asteroidList = new List<Asteroid>();
        List<Enemy> enemyList = new List<Enemy>();
        List<Explosion> explosionList = new List<Explosion>();
        
        //Instantianting our Player and Terrain objects
        Player p = new Player();
        TerrainBG TBG = new TerrainBG();
        HUD hud;
        SoundManager sm = new SoundManager();
               
        // First State
        State gameState = State.Menu;
        
        //constructor
        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 600;
            this.Window.Title = " 1942 Remake ";
            Content.RootDirectory = "Content";
            enemyBulletDamage = 10;
            menuImage = null;
            menuImage2 = null;
            gameOverImage = null;

        }

        // init
        protected override void Initialize()
        {            
            base.Initialize();
        }

       // Load Content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
                        
            p.LoadContent(Content);
            TBG.LoadContent(Content);
            hud = new HUD(p);
            hud.LoadContent(Content);
            sm.LoadContent(Content);
            menuImage = Content.Load<Texture2D>("menu6");
            menuImage2 = Content.Load<Texture2D>("Menu3");
            gameOverImage = Content.Load<Texture2D>("gamerover1");

            BGSongSFXIns = sm.bgMusic.CreateInstance();
            BGSongSFXIns.Volume = 0.5f;
            BGSongSFXIns.IsLooped = true;

            
        }

      // Unload Content
        protected override void UnloadContent()
        {
            
        }

        // Update
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

          

            // Updating Playing State
            switch (gameState)
            {
                case State.Game:
                    {
                        if (gameState == State.Game)
                            BGSongSFXIns.Play();
                        else
                            BGSongSFXIns.Stop();

                        // Updating Enemies and checking Collision of enemy ship to player ship                       
                        TBG.speed = 9;
                        foreach (Enemy e in enemyList)
                        {
                            //check if enemy ship is colliding with player
                            if (e.boundingBox.Intersects(p.boundingBox))
                            {
                                SoundEffectInstance ExplodeSFXIns = sm.explodeSound.CreateInstance();
                                ExplodeSFXIns.Volume = 0.3f;
                                ExplodeSFXIns.Play();
                                explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(e.position.X, e.position.Y)));
                                p.health -= 40;
                                e.isVisible = false;
                            }

                            // Check enemyBullet collison with player ship
                            for (int i = 0; i < e.bulletList.Count; i++)
                            {
                                if (p.boundingBox.Intersects(e.bulletList[i].boundingBox))
                                {

                                    p.health -= enemyBulletDamage;
                                    e.bulletList[i].isVisible = false;
                                }
                            }

                            // Checking player bullet collision to enemy ship
                            for (int i = 0; i < p.bulletList.Count; i++)
                            {
                                if (p.bulletList[i].boundingBox.Intersects(e.boundingBox))
                                {
                                    SoundEffectInstance ExplodeSFXIns = sm.explodeSound.CreateInstance();
                                    ExplodeSFXIns.Volume = 0.3f;
                                    ExplodeSFXIns.Play();
                                    hud.playerScore += 20;
                                    explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(e.position.X, e.position.Y)));
                                    p.bulletList[i].isVisible = false;
                                    e.isVisible = false;
                                }
                            }

                            e.Update(gameTime);

                        }

                        //update explosions
                        foreach (Explosion ex in explosionList)
                        {
                            ex.Update(gameTime);
                        }


                        // For each asteroid in our asteroidlist, update it and check for collisions
                        foreach (Asteroid a in asteroidList)
                        {
                            // Check to see if any of the asteroids are colliding with our player ship , if they are, set invisible to false (remove from asteroid list)
                            if (a.boundingBox.Intersects(p.boundingBox))
                            {
                                SoundEffectInstance ExplodeSFXIns = sm.explodeSound.CreateInstance();
                                ExplodeSFXIns.Volume = 0.3f;
                                ExplodeSFXIns.Play();
                                explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(a.position.X, a.position.Y)));
                                p.health -= 20;
                                a.isVisible = false;
                            }

                            //Iterate through our bullet list (located in player class) if any asteroids come in contact with bullets then set visibilty to bullets/asteroids to false
                            for (int i = 0; i < p.bulletList.Count; i++)
                            {
                                if (a.boundingBox.Intersects(p.bulletList[i].boundingBox))
                                {
                                    SoundEffectInstance ExplodeSFXIns = sm.explodeSound.CreateInstance();
                                    ExplodeSFXIns.Volume = 0.3f;
                                    ExplodeSFXIns.Play();
                                    explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(a.position.X, a.position.Y)));
                                    hud.playerScore += 5;
                                    a.isVisible = false;
                                    p.bulletList[i].isVisible = false;
                                }
                            }

                            a.Update(gameTime);
                        }



                        //hud.Update(gameTime);  
                        
                        //if player health = 0 then go to gameover state
                        if (p.health <= 0)
                            gameState = State.Gameover;

                        p.Update(gameTime);
                        TBG.Update(gameTime);
                        ManageExplosions();
                        LoadAsteroids();
                        LoadEnemies();
                        break;
                    }
                           
                    //Updating Menu State
                case State.Menu:
                        {
                            //get keyboard state
                            KeyboardState keyState = Keyboard.GetState();

                            if (keyState.IsKeyDown(Keys.Enter))
                            {
                                gameState = State.Game;                                
                                BGSongSFXIns.Volume = 0.5f;
                                BGSongSFXIns.IsLooped = true;
                                BGSongSFXIns.Play();
                                
                            }
                            TBG.Update(gameTime);
                            TBG.speed = 1;
                            break;
                            
                        }

                  //updating gamveover state                
                case State.Gameover:
                        {
                            //get keyboard state
                            KeyboardState keyState = Keyboard.GetState();

                            //if in the game over screen and player hits escape, returns player to main menu
                            if (keyState.IsKeyDown(Keys.B))
                            {
                                p.position = new Vector2(280, 900);
                                enemyList.Clear();
                                asteroidList.Clear();
                                p.health = 200;
                                hud.playerScore = 0;
                                gameState = State.Menu; 
                            }
                                                      
                            break;
                        }
            }          

            base.Update(gameTime);
        }

       // Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            switch (gameState)
            {
                //drawing playing state
                case State.Game:
                    {
                        TBG.Draw(spriteBatch);

                        p.Draw(spriteBatch);
                        foreach (Explosion ex in explosionList)
                        {
                            ex.Draw(spriteBatch);
                        }

                        foreach (Asteroid a in asteroidList)
                        {
                            a.Draw(spriteBatch);
                        }

                        foreach (Enemy e in enemyList)
                        {
                            e.Draw(spriteBatch);
                        }

                        hud.Draw(spriteBatch);
                       
                        break;
                    }

                    //drawing main menu
                case State.Menu:
                   {
                       TBG.Draw(spriteBatch);
                       spriteBatch.Draw(menuImage, new Vector2(-40,100), Color.White);
                       spriteBatch.DrawString(hud.playerScoreFont, "Press ENTER to play!", new Vector2(170, 500), Color.Red);


                       break;
                   }
                    
                        
                  
                    //drawing game over
                case State.Gameover:
                    {
                        spriteBatch.Draw(gameOverImage, new Vector2(-80, 100), Color.White);
                        spriteBatch.DrawString(hud.playerScoreFont, "Your Final Score was - " + hud.playerScore.ToString(), new Vector2(150, 400), Color.Red);

                        break;
                    }
            }
            

           


            spriteBatch.End();
          
            base.Draw(gameTime);
        }

        //Load Asteroids
        public void LoadAsteroids()
        {

            //creating random variables for our X and Y axis of our asteroids
            int randY = random.Next(-500, -50);
            int randX = random.Next(0, 500);

            //if there are less than 3 asteroids on the screen then create more until there is 5 again
            if (asteroidList.Count < 3)
            {
                asteroidList.Add(new Asteroid(Content.Load<Texture2D>("asteroid"), new Vector2(randX, randY)));
            }

            // if any of the asteroids in the list where destroyed or invisible then remove them from the list
            for (int i = 0; i < asteroidList.Count; i++)
            {
                if (!asteroidList[i].isVisible)
                {
                    asteroidList.RemoveAt(i);
                    i--;
                }
                
            }
              
        }

        //Load Enemies
        public void LoadEnemies()
        {

            //creating random variables for our X and Y axis of our asteroids
            int randY = random.Next(-500, -50);
            int randX = random.Next(0, 500);

            //if there are less than 3 enemies on the screen then create more until there is 3 again
            if (enemyList.Count < 3)
            {
                enemyList.Add(new Enemy(Content.Load<Texture2D>("enemyShip"), new Vector2(randX, randY), Content.Load<Texture2D>("EnemyBullet")));
            }

            // if any of the enemies in the list where destroyed or invisible then remove them from the list
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (!enemyList[i].isVisible)
                {
                    enemyList.RemoveAt(i);
                    i--;
                }

            }

        }

        // Manage Explosions
        public void ManageExplosions()
        {
            for (int i = 0; i < explosionList.Count; i++)
            {
                if (!explosionList[i].isVisible)
                {
                    explosionList.RemoveAt(i);
                    i--;
                }
            }
        }

    }
}
