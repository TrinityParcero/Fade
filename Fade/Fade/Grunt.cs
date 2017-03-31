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
    class Grunt : Enemy
    {
        //one health
        //same speed as player
        //int gMaxHeight (the largest height the grunt will ever get to)
        //int ground
        //bool isSpawn
        public Grunt(Texture2D asset, Rectangle loc, int speed, double hp, double dmg) : base(asset,loc,speed,hp,dmg)
        {
          //isSpawn = false;
          //gMaxHeight = 1/2 of playerHeight
          //ground = the same ground as the one the player starts at


        }

        ///public void move()
        ///{
        /// if(isSpawn == false)
        /// {
        ///   isSpawn = true;
        ///   
        /// }
        /// 
        ///}
        ///
        /// 
        /// public void moveUpdate()
        /// {
        ///    if(isSpawn == true)
        ///    {
        ///    gruntLocation.Y -= gruntJumpIncrement
        ///    
        ///    }
        ///    if(gruntLoction.Y less than or equal to max grunt height)
        ///    {
        ///    bool for grunt jump = false
        ///    bool for grunt fall = true
        ///    }
        ///    if(bool for grunt fall == true)
        ///    {
        ///    gruntLocation.Y += gruntJumpIncrement
        ///    }
        ///    if(gruntLocation.Y >= ground value)
        ///    {
        ///    bool for grunt jump = false
        ///    bool for grunt fall = false
        ///    }
        ///    
        ///    if(playerLocation.X less than or equal to gruntLocation.X)
        ///    {
        ///    gruntState = gruntRight
        ///    gruntLocation.X -= whatever distance grunt travels
        ///    }
        ///    else if(playerLocation.X >= gruntLocation.X)
        ///    {
        ///    gruntState = grnutLeft
        ///    gruntLocation.X += whatever distance the grunt travels
        ///    }
        /// 
        /// }
        ///


    }
}
