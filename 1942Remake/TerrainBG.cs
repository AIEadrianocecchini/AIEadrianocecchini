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
    public class TerrainBG
    {
        public Texture2D texture;
        public Vector2 bgPos1, bgPos2;
        public int speed;

        //constructor
        public TerrainBG()
        {
            texture = null;
            bgPos1 = new Vector2(0, 0);
            bgPos2 = new Vector2(0, -600);
            speed = 5;
        }


        //Load Content
        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("space");
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bgPos1, Color.White);
            spriteBatch.Draw(texture, bgPos2, Color.White);
        }

        //Update
        public void Update(GameTime gameTime)
        {
            //setting speed for background scroll
            bgPos1.Y = bgPos1.Y + speed;
            bgPos2.Y = bgPos2.Y + speed;

            // Scrolling background (Repeating)
            if (bgPos1.Y >= 600)
            {
                bgPos1.Y = 0;
                bgPos2.Y = -600;
            }
        }

    }
}
