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
    public class HUD
    {               
        public Texture2D healthTexture;
        public int playerScore, screenWidth, screenHeight;
        public SpriteFont playerScoreFont;
        public Vector2 playerScorePos;
        public bool showHud;
        public Rectangle healthRectangle;
        public Vector2 healthBarPosition;

        private Player player;

        // Constructor
        public HUD(Player getplayer)
        {
            player = getplayer;
            playerScore = 0;
            showHud = true;
            screenHeight = 600;
            screenWidth = 600;
            playerScoreFont = null;
            playerScorePos = new Vector2(screenWidth / 2, 30);

            healthBarPosition = new Vector2(50, 50);

           
        }

        // Load Content
        public void LoadContent(ContentManager Content)
        {
            playerScoreFont = Content.Load<SpriteFont>("CopperplateGothicBold");
            healthTexture = Content.Load<Texture2D>("healthbar");
        }

        //Update
        public void Update(GameTime gameTime)
        {
            //Get Keyboard state
            KeyboardState keyState = Keyboard.GetState();
            healthRectangle = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, player.health, 25);
        }

        //draw
        public void Draw(SpriteBatch spriteBatch)
        {
            //if we are showing our HUD (if showHud = true - then display our HUD)
            if (showHud)
                spriteBatch.DrawString(playerScoreFont, "Score - " + playerScore, playerScorePos, Color.Red);
            spriteBatch.Draw(healthTexture, healthRectangle, Color.White);
        }
    }
}
