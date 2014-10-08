using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace _1942Remake
{
    public class SoundManager
    {
        public SoundEffect playerShootSound;
        public SoundEffect explodeSound;
        public SoundEffect bgMusic;

        //Constructor
        public SoundManager()
        {
            playerShootSound = null;
            explodeSound = null;
            bgMusic = null;
        }

        public void LoadContent(ContentManager Content)
        {
            playerShootSound = Content.Load<SoundEffect>("playershoot");
            explodeSound = Content.Load<SoundEffect>("explode");
            bgMusic = Content.Load<SoundEffect>("BGMusic");
            
        }
    }
}
