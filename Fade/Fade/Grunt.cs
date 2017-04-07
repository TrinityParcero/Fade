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
        public Rectangle location;// rectangle for the grunt's location
        public Texture2D sprite { get; set; }
        public bool jumping { get; set; }//bool to test for grunt jumping
        public bool falling { get; set; }//bool to test for grunt falling
        public GruntState gruntState = GruntState.WalkLeft;//initialized grunt state
        public GruntState prevGruntState;



        //note: when spawning the grunt make sure to pass in rectangle of player
        //constructor for grunt class
        public Grunt(Texture2D asset, Rectangle loc, int speed, double hp, double dmg, Rectangle grd) : base(asset,loc,speed,hp,dmg)
        {
            isSpawn = false;
            gMaxHeight = grd.Height / 2;
            gruntJI = 1;
            location.Y = loc.Y;
            ground = location.Y;
            sprite = asset;
            jumping = false;
            falling = false;


        }
        /// <summary>
        /// the grunt starts to move as soon as it spawns
        /// </summary>
        public void move1()
        {
            if(isSpawn == false)
            {
            isSpawn = true;
          
            }
         
        }
        
        /// <summary>
        /// once the grunt is moving, it should move like this
        /// </summary>
                 
         public void move(Player p)
         {
            if(isSpawn == true)
            {
                location.Y -= gruntJI;
                jumping = true;
            }
            if(location.Y <= gMaxHeight)
            {
                jumping = false;
                falling = true;
            }
            if(falling == true)
            {
                location.Y += gruntJI;
            }
            if(location.Y >= ground)
            {
                jumping = false;
                falling = false;
            }
            
            if(p.location.X <= location.X)
            {
                gruntState = GruntState.WalkRight;
                location.X -= Speed;
            }
            else if(p.location.X >= location.X)
            {
                gruntState = GruntState.WalkLeft;
                location.X += Speed;
            }
         
         }
        


    }
}
