using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Fade
{
    //grunt states
    enum GruntState
    {
        WalkLeft,
        WalkRight,
        JumpLeft,
        JumpRight

    }
    class Grunt : Enemy
    {
       
      
        //one health
        //same speed as player
        int gMaxHeight; // the largest height the grunt will ever reach
        int ground; //the grunt as well as the lowest possible height the grunt will reach
        bool isSpawn;// states whether or not the grunt has spawned
        int gruntJI; // amount by which the grunt's jump height increments
        //public Rectangle location;// rectangle for the grunt's location
       // public Texture2D sprite { get; set; }
        public bool jumping { get; set; }//bool to test for grunt jumping
        public bool falling { get; set; }//bool to test for grunt falling
        public GruntState gruntState = GruntState.WalkLeft;//initialized grunt state
        public GruntState prevGruntState;


        //note: when spawning the grunt make sure to pass in rectangle of player
        //constructor for grunt class
        public Grunt(Texture2D asset, Rectangle loc, Rectangle hb,int speed, double hp, double dmg,SoundEffect sound) : base(asset,loc,hb,speed,hp,dmg,sound)
        {
            isSpawn = false;
            //gMaxHeight = loc.Height / 2;
            //gMaxHeight = 150;
            //gruntJI = 1;
            location = loc;
            //ground = location.Y;
            sprite = asset;
            color = Color.White;
            //jumping = false;
            //falling = false;
            Death = sound;
        }
        /// <summary>
        /// the grunt starts to move as soon as it spawns
        /// </summary>
        public void spawn(bool spawnFlag)
        {
            isSpawn = spawnFlag;
         
        }
        
        /// <summary>
        /// once the grunt is moving, it should move like this
        /// </summary>
                 
         public void move(Player p)
         {
            //if the grunt has spawned then its y location sould change by the grunt jump increment (gruntJI)
            // and jumping is true
            if (isSpawn == true)
            {
                while(location.Y >= gMaxHeight)
                {
                    location.Y -= gruntJI;
                    jumping = true;
                }
                
                //if the grunts y location has reached its maximum height
                // then it os no longer jumping and it is falling
                if (location.Y <= gMaxHeight)
                {
                    jumping = false;
                    falling = true;
                }
                //if its is falling then its location decrements by the gruntJumpIncrement
                if (falling == true)
                {
                    while(location.Y <= ground)
                    {
                        location.Y += gruntJI;
                    }
                    
                }
                //if the grunt has reached the ground
                //then it is no longer falling or jumping
                if (location.Y >= ground)
                {
                    jumping = false;
                    falling = false;
                }
                                                    //for an amount of speed, move distance of a few pixels
                if (p.location.X <= location.X)
                {
                    gruntState = GruntState.WalkRight;
                    location.X -= Speed;
                }
                else if (p.location.X >= location.X)
                {
                    gruntState = GruntState.WalkLeft;
                    location.X += Speed;
                }
            }
         
         }
    }
}
