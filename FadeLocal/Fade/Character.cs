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
    interface Character
    {
        double Health { get; set; }
        int Damage { get; set; }
        int Speed { get; set; }
        bool isDead { get; set; }
        Texture2D sprite { get; set; }

        void Run(GameTime gameTime);
        void Jump();
        void Attack();
        void takeDamage(double dmg);
    }
}
