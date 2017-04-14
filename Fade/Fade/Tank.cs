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

    //tank states
    
    enum TankState
    {
      
      ChargeUpR,
      ChargeUpL
     }
     
    class Tank : Enemy
    {
        //two or three health
        //half player speed
        Rectangle tankRect;
        Texture2D tankSprite;
        int ground;
        TankState tState = TankState.ChargeUpL;
        public Tank(Texture2D asset, Rectangle loc, int speed, double hp, double dmg) : base(asset,loc,speed,hp,dmg)
        {
            //save off the tank's current x position this will be used in
            int tankCurrLoc = tankRect.Y;
            
            ground = location.Y;
            tankRect = loc;
            tankSprite = asset;
           
        }

        
     
        ///define a stopping distance = player location.x - a length of the tank himself;
        public void chargeCheck(int chargeSpeed, int playerX)
        {
        //if(tankRect.X - playerlocation.x == some distance we want to charge from )
        ///  some bool = true;
        /// 
        /// 
        /// 
        ///}
        ///
        ///public void chargeUpdate
        ///{
        ///  if(some bool == true)
        ///  {
        ///    tanklocation.x -= cahrgeSpeed;
        ///  }
        ///  if(tanklocation.X == stopping distance)
        ///  {
        ///   some bool = false;
        ///  }
        /// 
        }












    }
}
