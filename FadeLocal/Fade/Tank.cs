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
    class Tank : Enemy
    {
        //two or three health
        //half player speed
        public Tank(Texture2D asset, Rectangle loc) : base(asset,loc)
        {

        }
    }
}
