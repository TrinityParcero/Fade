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
    class Enemy
    {
        public double Damage{ get; set; }

        public double Health{ get; set; }

        public bool isDead{ get; set; }

        public int Speed{ get; set; }

        public Texture2D sprite { get; set; }

        public Rectangle location;

        public Enemy(Texture2D texture, Rectangle loc, int speed, double hp, double dmg)
        {
            sprite = texture;
            location = loc;
            isDead = false;
            Speed = speed;
            Health = hp;
            Damage = dmg;
        }

        public void Attack()
        {
            throw new NotImplementedException();
        }

        public void Jump()
        {
            throw new NotImplementedException();
        }

        public void Run(Rectangle fogBounds, Player p)
        {
            //basic idea of motion
            if(location.X > p.location.X)
            {
                location.X -= Speed;
            }
            else if(location.X < p.location.X-50)
            {
                location.X += Speed;
            }
        }

        public void takeDamage(double dmg)
        {
            throw new NotImplementedException();
        }
    }
}
