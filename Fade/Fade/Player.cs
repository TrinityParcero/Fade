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
    enum PlayerState
    {
        FaceRight,
        WalkRight,
        FaceLeft,
        WalkLeft
    }
    class Player : Character
    {
        public int Damage{ get; set; }

        public double Health{ get; set; }

        public bool isDead{ get; set; }

        public int Speed{ get; set; }

        public Texture2D sprite { get; set; }

        public Rectangle location;

        public int currentX { get; set; }

        PlayerState playerState = PlayerState.FaceRight;

        private int currentFrame;
        private int totalFrames;

        //Slow down frame animation
        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame = 0;

        public Player(Texture2D asset, Rectangle loc)
        {
            Damage = 1;
            Health = 3.0;
            isDead = false;
            Speed = 1;
            sprite = asset;
            location = loc;
            currentX = loc.X;
            currentFrame = 0;
            totalFrames = loc.X * loc.Y;
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

        KeyboardState previous = new KeyboardState();
        public void Run(GameTime gameTime)
        {

            KeyboardState keystate = Keyboard.GetState();

            //Idle animation
            if (keystate.GetPressedKeys().Length == 0)
                currentFrame++;
            timeSinceLastFrame = 0;
            if (currentFrame == 2)
                currentFrame = 0;

            var ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.A))
            {
                if (location.X < 100)
                {

                }
                else
                {
                    playerState = PlayerState.WalkLeft;
                    currentX -= 2;
                    location.X -= 2;

                }
                
                
            }
            if (ks.IsKeyUp(Keys.A) && previous.IsKeyUp(Keys.D) && playerState == PlayerState.WalkLeft)
            {
                playerState = PlayerState.FaceLeft;
            }


            if (ks.IsKeyDown(Keys.D))
            {
                playerState = PlayerState.WalkRight;
                currentX += 2;
                location.X += 2;
            }
            if (ks.IsKeyUp(Keys.A) && previous.IsKeyUp(Keys.D) && playerState == PlayerState.WalkRight)
            {
                playerState = PlayerState.FaceRight;
            }

            // PRACTICE EXERCISE: Get the current keyboard state here


            // PRACTICE EXERCISE: Add your finite state machine code (switch statement) here
            previous = ks;
            //hit wasd to go!
        }

        public void takeDamage(double dmg)
        {
            //if enemy is in hitbox, take const damage
            //if enemy is in attack animation and youre in hitbox- damage
            if (Health <= 0)
            {
                isDead = true;
            }
            else
            {
                Health -= dmg;
            }
        }
    }
}
