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
        /// 
        /// }
        ///


    }
}
