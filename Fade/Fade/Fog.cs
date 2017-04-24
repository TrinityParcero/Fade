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
    class Fog
    {
        public double Speed { get; set; }
        public int enemiesConsumed { get; set; }

        public Texture2D sprite { get; set; }
        public Rectangle location;
        public Rectangle bounds;

        public Fog(Texture2D texture, Rectangle loc, Rectangle bound, double speed, int consumed)
        {
            sprite = texture;
            location = loc;
            bounds = bound;
            Speed = speed;
            enemiesConsumed = consumed;
        }

        public void Move(Player player)
        {
            //move at about half player speed by default
            if (bounds.Intersects(player.location))
            {
                Speed = 1;
                //for now the fog stops but i want to change this
                location.X += (int)Speed / 2;
                bounds.X += (int)Speed / 2;
            }
            else
            {
                location.X += (int)Speed;
                bounds.X += (int)Speed;
            }

        }


        public void damagePlayer(Player player)
        {
            //will take 6 ticks to kill

            if (player.location.X <= location.X + 600)
            {
                if (player.invincibilityFrame <= 0)
                {
                    player.isHit = true;
                    player.invincibilityFrame = 90;
                    player.takeDamage(0.5);
                }
            }
            if (player.invincibilityFrame > 0)
            {
                player.invincibilityFrame--;
            }
            else
            {
                player.color = Color.White;
            }

        }
        public void consumeEnemy(Enemy enemy)
        {
            //enemy.isDead = true;
            if (enemy.isDead == false)
            {
                Speed += 0.25;
            }
            
        }
    }
}
