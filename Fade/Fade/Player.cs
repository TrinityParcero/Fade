using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fade
{
    class Player : Character
    {
        public int Damage{ get; set; }

        public int Health{ get; set; }

        public bool isDead{ get; set; }

        public int Speed{ get; set; }

        public int XPos { get; set; }

        public int YPos { get; set; }

        public Texture2D sprite { get; set; }

        private int currentFrame;
        private int totalFrames;

        //Slow down frame animation
        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame = 0;


        public Player(Texture2D asset, int x, int y)
        {
            Damage = 1;
            Health = 3;
            isDead = false;
            Speed = 1;
            sprite = asset;
            XPos = x;
            YPos = y;
            currentFrame = 0;
            totalFrames = y * x;
        }

        public void Attack()
        {
            throw new NotImplementedException();
            //if char is in attack pose-check for it
            //if enemy is in hitbox while char is attacking-deal damage
        }

        public void Jump()
        {
            throw new NotImplementedException();
            //jumping over tank is absolute limit of jump distance
        }

        public void Run(GameTime gameTime)
        {

            KeyboardState keystate = Keyboard.GetState();

            //Idle animation
            if (keystate.GetPressedKeys().Length == 0)
                currentFrame++;
            timeSinceLastFrame = 0;
            if (currentFrame == 2)
                currentFrame = 0;

            //Walking Animation
            if (keystate.IsKeyDown(Keys.A))
            {
                XPos -= 1;
            }

            if(keystate.IsKeyDown(Keys.D))
            {
                XPos += 1;
            }
            //hit wasd to go!

            //Idle animation

            if (keystate.GetPressedKeys().Length == 0)
                currentFrame++;
            timeSinceLastFrame = 0;
            if (currentFrame == 2)
                currentFrame = 0;
        }

        public void takeDamage()
        {
            throw new NotImplementedException();
            //if enemy is in hitbox, take const damage
            //if enemy is in attack animation and youre in hitbox- damage
        }
    }
}
