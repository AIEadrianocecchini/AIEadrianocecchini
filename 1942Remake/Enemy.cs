using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace _1942Remake
{
    public class Enemy
    {
        public Rectangle boundingBox;
        public Texture2D texture, bulletTexture;
        public Vector2 position;
        public int health, speed, bulletDelay, currentDifficultyLevel;
        public bool isVisible;
        public List<Bullet> bulletList;

        //Constructor
        public Enemy(Texture2D newTexture, Vector2 newPosition, Texture2D newBulletTexture)
        {
            bulletList = new List<Bullet>();
            texture = newTexture;
            bulletTexture = newBulletTexture;
            health = 5;
            position = newPosition;
            currentDifficultyLevel = 1;
            bulletDelay = 40;
            speed = 2;
            isVisible = true;
         }

        //Update
        public void Update(GameTime gameTime)
        { 
            //Update Collision Rectangle
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            //Update Enemy movement
            position.Y += speed;

            //Move Enemy back to top of the screen if he flys off bottom
            if (position.Y >= 650)
                position.Y = -75;

            EnemyShoot();
            UpdateBullets();
         }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            //draw enemyShip
            spriteBatch.Draw(texture, position, Color.White);

            //draw enemy bullets
            foreach (Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }

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
                b.position.Y = b.position.Y + b.speed;

                // if the bullet hits the top of the screen makes it invisible
                if (b.position.Y >= 650)
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

        //Enemyshoot Function
        public void EnemyShoot()
        {
            //shoot only if bullet delay resets
            if (bulletDelay >= 0)
                bulletDelay--;

            if (bulletDelay <= 0)
            {
                // Spawn new bullet + position it infront of enemy ship
                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.position = new Vector2 (position.X + texture.Width / 2 - newBullet.texture.Width / 2, position.Y + 30);
                
                newBullet.isVisible = true;

                if (bulletList.Count() < 20)
                    bulletList.Add(newBullet);
            }

            //reset bullet delay
            if (bulletDelay == 0)
                bulletDelay = 40;

        }
    }
}
