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
    enum TankState
    {
        TankFaceRight,
        TankFaceLeft,
        TankBRight,
        TankbLeft
    }

    class Tank : Enemy
    {
       
        //two or three health
        //half player speed
        Rectangle foglocation;
        public Tank(Texture2D asset, Rectangle loc, int speed, double hp, double dmg) : base(asset,loc,speed,hp,dmg)
        {

            //call run method from enemy class
            ///Run(foglocation, );
           ///{
           ///  if(tank location.x less than player location)
           ///  {
           ///     TankState = TankFaceRight;
           ///     tanklocation.x += speed
           ///  }
           ///  if(tank location.x > player location)
           ///  {
           ///     TankState = TankFaceLeft;
           ///     tanklocation.x -= speed;
           ///  }
           /// if (playerlocation.x - tanklocation.x == absolute value of 5)
           /// {
           ///   if(tanklocation.x less than playerlocation.x)
           ///   {
           ///     tanklocation.x += 0; or doesnt move
           ///     tankstate = tankBright;
           ///   }
           ///   if(tanklocation.x > playerlocation.x)
           ///   {
           ///     tanklocation += 0;
           ///     tankstate = tankBleft;
           ///   }
           /// }
           ///}
          






          




        }
    }
}
