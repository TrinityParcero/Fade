using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using Microsoft.Xna.Framework.Audio;

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
        public int Damage { get; set; }

        public double Health { get; set; }

        public bool isDead { get; set; }

        public int Speed { get; set; }

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

        public Rectangle swordBox { get; set; }

        //have a bool for the air attack
        public bool AirAttack { get; set; }
        bool bouncing = false;

        int MAX_HEIGHT = 100;
        int jumpSpeed = 0;
        int startY = 300;

        public int invincibilityFrame = 180;
        SoundEffect DeathSound;
        SoundEffect DmgSound;

        //attributes for jumping


        int g;
        int jumpIncrement;
        int ground;


        //Slow down frame animation
        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame = 0;

        public Player(Texture2D asset, Rectangle loc, SoundEffect damage, SoundEffect death)
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
            AirAttack = false;
            jumping = false;
            jumpIncrement = 5;

            DmgSound = damage;
            DeathSound = death;

            ground = location.Y;
        }

        public void Attack(Enemy enemy, Game1 game)
        {
            attacking = true;
            int colFrame = 0;
            //if char is in attack pose-check for it
            //if enemy is in hitbox while char is attacking-deal damage
            if (game.swordFrame == 1)
            {
                colFrame = 1;
                swordBox = new Rectangle((int)game.swordPos.X, (int)game.swordPos.Y, 30, 102);
                if (swordBox.Intersects(enemy.location))
                {
                    if (enemy.isDead == false)
                    {
                        enemy.takeDamage(Damage);
                        enemy.Death.Play();
                    }

                }
            }

            if (colFrame == 1)
            {
                colFrame = 2;
                swordBox = new Rectangle((int)game.swordPos.X + 60, (int)game.swordPos.Y + 40, 100, 80);
                if (swordBox.Intersects(enemy.location))
                {
                    if (enemy.isDead == false)
                    {
                        enemy.takeDamage(Damage);
                        enemy.Death.Play();
                    }
                }
            }

            if (colFrame == 2)
            {
                colFrame = 1;
                swordBox = new Rectangle((int)game.swordPos.X + 60, ((int)game.swordPos.Y + 90), 110, 30);
                if (swordBox.Intersects(enemy.location))
                {
                    if (enemy.isDead == false)
                    {
                        enemy.takeDamage(Damage);
                        enemy.Death.Play();
                    }
                }
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

        //Air Attack
        public void airAttack(Enemy enemy, Texture2D jumpAttack)
        {
            //bool bouncing = false;
            //the air attack bool does absolutley nothing right now
            AirAttack = true;
            if (invincibilityFrame <= 0)
            {
                //isHit = true;
                //takeDamage(Damage);
                invincibilityFrame = 400;
            }

            if (invincibilityFrame > 0)
            {
                invincibilityFrame--;
            }

            else
            {
                color = Color.White;
            }

            Rectangle swordBox = new Rectangle(location.X, location.Y, 100, 200);
            //set the animaiton, the sword should aim down, smash bros link down smash
            //color = Color.Blue;
            //if the enemy is touched when the player touched them, then the enemy takes twice the damage
            if (swordBox.Intersects(enemy.hitBox) && enemy.isDead == false && location.Y < 330)
            {
                if (enemy.isDead == false)
                {
                    enemy.takeDamage(3 * Damage);
                    enemy.Death.Play();
                }
                
                if (bouncing == false)
                {
                    //JumpUpdate();
                    location.Y -= 100;
                }
                bouncing = true;
                //the player should also bounce, the jumpUpdate code should still continue working by moving the player up and/ or down
                //location.Y -= 50;


            }
            bouncing = false;

        }


        /// <summary>
        /// jump update will check to see if the player is jumping and/or falling, if it is nit falling, and not jumping then it is on ground, if it is jumping then it is  incrementing by 5(this is the rate), once the player reaches the max height(280 above the player height, which is 300, which in monogame is 20)
        /// jumping is set to false, and falling is set to true, if the player is then falling the location is moved down by the same amount as it was raised, then if the location of the character is greater than or equal to the ground (since the ground is 300, and anything below the ground would be more than 300)
        /// then the player is not falling or jumping, and the location of the player is set to the ground again
        /// </summary>
        public void JumpUpdate()
        {
            //set a certain bool to true if the player attacks and have it be true until the player is not falling and is not jumping 
            ///if((jumping == true || falling == true)&&(attack==true))
            ///{
            ///    //include this below pseudo code in its own method and call it
            ///        then certain bool for air attack = true;
            ///        //and then the visual change would occur for the sword, swordSprite should turn invisible
            ///        //also possibly move the swords rectangle in this spot
            ///        if(player intersect with enemy)
            ///        {
            ///          deal damage, possibly twice as much as normal
            ///          and player y goues up, but since its monogame y - some increment and then continue to fall
            ///        }
            ///}
            ///



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
                AirAttack = false;
            }

        }


        KeyboardState previous = new KeyboardState();
        public void Run(Rectangle fogBounds)
        {
            if (Health <= 0)
            {
                isDead = true;
            }
            KeyboardState keystate = Keyboard.GetState();
            var ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.A) && ks.IsKeyUp(Keys.D))
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
            if (ks.IsKeyDown(Keys.D) && ks.IsKeyDown(Keys.A))
            {
                playerState = PlayerState.FaceRight;
            }


            if (ks.IsKeyDown(Keys.D) && ks.IsKeyUp(Keys.A))
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
                DmgSound.Play();
                color = Color.Red;
            }
        }
    }
}
