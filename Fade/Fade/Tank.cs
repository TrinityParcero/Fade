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
    /// <summary>
    /// enum TankState
    /// {
    ///  FaceRight
    ///  FaceLeft
    ///  ChargeUpR
    ///  ChargeUpL
    /// }
    /// </summary>
    class Tank : Enemy
    {
        //two or three health
        //half player speed

        //int ground;

        public Tank(Texture2D asset, Rectangle loc, int speed, double hp, double dmg) : base(asset,loc,speed,hp,dmg)
        {

        }
    }
}
