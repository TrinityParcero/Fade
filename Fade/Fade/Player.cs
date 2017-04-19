using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace Fade
{
    enum PlayerState
    {
        FaceRight,
        WalkRight,
        FaceLeft,
        WalkLeft,
        JumpRight,
        JumpLeft
    }

    enum HealthState
    {
        ThreeFull,
        FiveHalves,
        TwoFull,
        ThreeHalves,
        OneFull,
        OneHalf

    }

    class Player
    {
        public int Damage{ get; set; }

        public double Health{ get; set; }

        public bool isDead { get; set; }

        public int Speed{ get; set; }

        public Texture2D sprite { get; set; }

        public Texture2D sword { get; set; }
        //We'll use a seperate sprite for the sword, this will get make it easier to detect collisions with enemy hitboxes

        public bool isHit { get; set; }
        //will be used for iframes

        public Rectangle location;

        public Color color { get; set; }

        public int currentX { get; set; }

        public bool jumping { get; set; }

        public bool falling { get; set; }
        
        public bool attacking { get; set; }

        public HealthState healthState = HealthState.ThreeFull;

        public PlayerState playerState = PlayerState.FaceRight;

        public PlayerState prevPlayerState;

        private int currentFrame;

        private int totalFrames;

        int MAX_HEIGHT = 150;
        int jumpSpeed = 0;
        int startY = 300;

        public int invincibilityFrame = 180;

        //attributes for jumping


         int g;
         int jumpIncrement;
         int ground;

       
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
            color = Color.White;

            isHit = false;
            
            jumping = false;
            jumpIncrement = 5;
            
            ground = location.Y;
        }

        public void Attack(Enemy enemy, Game1 game)
        {
            if (attacking)
            {
                Rectangle swordBox;
                //if char is in attack pose-check for it
                //if enemy is in hitbox while char is attacking-deal damage
                if(game.swordFrame == 1)
                {
                    swordBox = new Rectangle((int)game.swordPos.X, (int)game.swordPos.Y, 30, 102);
                }
                else if(game.swordFrame == 2)
                {
                    swordBox = new Rectangle((int)game.swordPos.X, (int)game.swordPos.Y, 30, 102);
                }
                else if(game.swordFrame == 3)
                {
                    swordBox = new Rectangle((int)game.swordPos.X, (int)game.swordPos.Y, 30, 102);
                }
                else
                {
                    swordBox = new Rectangle((int)game.swordPos.X, (int)game.swordPos.Y, 30, 102);
                }

                if (swordBox.Intersects(enemy.location))
                {
                    //deal damage
                }
                
                attacking = true;
            }

        }

        public void Jump()
        {

            //jumping over tank is absolute limit of jump distance
            
            
            
            if (!jumping && !falling && playerState == PlayerState.FaceLeft || playerState == PlayerState.WalkLeft)
            {
                playerState = PlayerState.JumpLeft;
         
            }
            else if (!jumping && !falling && playerState == PlayerState.FaceRight || playerState == PlayerState.WalkRight)
            {
                playerState = PlayerState.JumpRight;

            }

            jumping = true;

        }



        /// <summary>
        /// jump update will check to see if the player is jumping and/or falling, if it is nit falling, and not jumping then it is on ground, if it is jumping then it is  incrementing by 5(this is the rate), once the player reaches the max height(280 above the player height, which is 300, which in monogame is 20)
        /// jumping is set to false, and falling is set to true, if the player is then falling the location is moved down by the same amount as it was raised, then if the location of the character is greater than or equal to the ground (since the ground is 300, and anything below the ground would be more than 300)
        /// then the player is not falling or jumping, and the location of the player is set to the ground again
        /// </summary>
        public void JumpUpdate()
        {
            if (jumping == true)
            {
                location.Y -= jumpIncrement;
                
              
                /*
                g -= -(i * i) + 10 * i;
                i += 1;

                location.Y = g;*/

                //if we want a parbola then we will need an attribute, which we will change with another attribute, to increment the player height
            }

            if (location.Y <= MAX_HEIGHT)
            {
                jumping = false;
                //falling will be set once the player jumps
                falling = true;

            }
            if (falling)
            {
                
                location.Y += 5;
            }

            if (location.Y >= ground)
            {
                jumping = false;
                falling = false;
                location.Y = ground;
            }

        }


        KeyboardState previous = new KeyboardState();
        public void Run(Rectangle fogBounds)
        {
            KeyboardState keystate = Keyboard.GetState();
            var ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.A))
            {
                if (fogBounds.Intersects(location))
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
            
            previous = ks;
            prevPlayerState = playerState;
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
                color = Color.Red;
            }
        }
    }
}
