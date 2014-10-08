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
    public class Asteroid
    {
        public Rectangle boundingBox;
        public Texture2D texture;
        public Vector2 position;
        public Vector2 origin;
        public float rotationAngle;
        public int speed;

        public bool isVisible;
        Random random = new Random();
        public float randX, randY;
        

        //constructor
        public Asteroid(Texture2D newTexture, Vector2 newPosition)
        {
            position = newPosition;
            texture = newTexture;
            speed = 4;
            isVisible = true;

            // where the asteriod will spawn
            randX = random.Next(0, 600);
            randY = random.Next(-150, -150);
            
        }

        // Load Content
        public void LoadContent(ContentManager Content)
        {
            
            
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // set bounding box for collision
            boundingBox = new Rectangle((int)position.X, (int)position.Y, 45, 45);

            // updating origin for rotation
            origin.X = texture.Width;
            origin.Y = texture.Height;

            // Update Movement of Asteroid
            position.Y = position.Y + speed;
            if (position.Y >= 600)
                position.Y = -50;

            //rotation asteroid
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            rotationAngle += elapsed;
            float circle = MathHelper.Pi * 2;
            rotationAngle = rotationAngle % circle;


        }

        //Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
                //spriteBatch.Draw(texture, position, Color.White);
                spriteBatch.Draw(texture, position, null, Color.White, rotationAngle, origin, 1.0f, SpriteEffects.None, 0f);
        }
    }
}
