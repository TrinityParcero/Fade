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
    enum EnemyState
    {
        FaceRight,
        FaceLeft
    }
    class Enemy
    {
        public double Damage { get; set; }

        public double Health { get; set; }

        public bool isDead { get; set; }

        public int Speed { get; set; }

        public Color color { get; set; }

        public Texture2D sprite { get; set; }

        public Rectangle location;
        public EnemyState eState = EnemyState.FaceLeft;

        //make a property to check if enemies should be moving
        //public bool shouldMove { get; set; } = true;

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
            if (Health <= 0)
            {
                isDead = true;
            }
            if (p.location.Intersects(location))
            {
                if (p.invincibilityFrame <= 0)
                {
                    p.isHit = true;
                    p.takeDamage(Damage);
                    p.invincibilityFrame = 200;
                }

            }
            if (p.invincibilityFrame > 0)
            {
                p.invincibilityFrame--;
            }

            else
            {
                p.color = Color.White;
            }
            //basic idea of motion
            if (location.X > p.location.X)
            {
                location.X -= Speed;
                eState = EnemyState.FaceLeft;
            }
            else if (location.X < p.location.X - 20)
            {
                location.X += Speed;
                eState = EnemyState.FaceRight;
            }
        }

        public void takeDamage(double dmg)
        {
            if (Health <= 0)
            {
                isDead = true;
            }
            else
            {
                Health -= dmg;
                color = Color.Red;
            }
        }
    }
}
