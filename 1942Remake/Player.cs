using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace _1942Remake
{
    //Main
    public class Player
    {
        public Texture2D texture, bulletTexture;
        public Vector2 position, healthBarPosition;
        public int speed, health;
        public float bulletDelay;
        public bool isColliding;
        public Rectangle boundingBox;
        public List<Bullet> bulletList;
        SoundManager sm = new SoundManager();


        //constructor
        public Player()
        {
            bulletList = new List<Bullet>();
            texture = null;
            position = new Vector2(280, 600);
            bulletDelay = 1;
            speed = 10;
            isColliding = false;
            //healthBarPosition = new Vector2(50, 50);

            health = 200;

        }

        //Load Content
        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("player");
            bulletTexture = Content.Load<Texture2D>("laserRedShot");
            //healthTexture = Content.Load<Texture2D>("healthbar");
            sm.LoadContent(Content);

            
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            //spriteBatch.Draw(healthTexture, healthRectangle, Color.White);
            foreach (Bullet b in bulletList)
                b.Draw(spriteBatch);
        }

        //Update
        public void Update(GameTime gameTime)
        {
            // Getting Keyboard State
            KeyboardState keyState = Keyboard.GetState();

            //Bounding Box for our player ship
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            // Set Rectangle for healthBar
            //healthRectangle = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, health, 25);

           
            // Fire Bullets
            if (keyState.IsKeyDown(Keys.Space))
            {
                SoundEffectInstance playershootSFXIns = sm.playerShootSound.CreateInstance();
                playershootSFXIns.Volume = 0.5f;
                playershootSFXIns.Play();
                Shoot();
            }

            UpdateBullets();

            // Ship Controls
            if (keyState.IsKeyDown(Keys.Up))
                position.Y = position.Y - speed;
           
            if (keyState.IsKeyDown(Keys.Left))
                position.X = position.X - speed;
            
            if (keyState.IsKeyDown(Keys.Right))
                position.X = position.X + speed;
           
            if (keyState.IsKeyDown(Keys.Down))
                position.Y = position.Y + speed;

            // keep player ship in screen bounds
            if (position.X <= 0) position.X = 0;
           
            if (position.X >= 600 - texture.Width) position.X = 600 - texture.Width;
            
            if (position.Y <= 0) position.Y = 0;
            
            if (position.Y >= 600 - texture.Height) position.Y = 600 - texture.Height;


            
        }

        //shoot method (used to set starting pos of our bullets)
        public void Shoot()
        {
            // Shoot only if the bullet delay resets
            if (bulletDelay >= 0)
                bulletDelay--;

            // if bullet delay is at 0 then create a new bullet at player pos and make it visible on the screen, then add the bullet to the list
            if (bulletDelay <= 0)
            {
                sm.playerShootSound.Play();
                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.position = new Vector2(position.X + 32 - newBullet.texture.Width / 2 - 8, position.Y + -0.5f);

                // making your bullets visible
                newBullet.isVisible = true;

                if (bulletList.Count() < 20)
                    bulletList.Add(newBullet);
                
            }

            // reset bullet delay
            if (bulletDelay == 0)
                bulletDelay = 10;
        }

        // Update bullets
        public void UpdateBullets()
        {
            // for each bullet in our bullet list update the movement and if the bullet hits the top of the screen, remove it from the list
            foreach (Bullet b in bulletList)
            {
                //BoundingBox for our bullets, for every bullet in our bullet list
                b.boundingBox = new Rectangle((int)b.position.X, (int)b.position.Y, b.texture.Width, b.texture.Height);

                //set movement for bullet
                b.position.Y = b.position.Y - b.speed;

                // if the bullet hits the top of the screen makes it invisible
                if (b.position.Y <= 0)
                    b.isVisible = false;
            }
            
            // Iterate through bulletList and see if any of the bullets are not visible, if they arent then remove that bullet from our bulletList
            for (int i = 0; i < bulletList.Count; i++)
            {
                if (!bulletList[i].isVisible)
                {
                    bulletList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
