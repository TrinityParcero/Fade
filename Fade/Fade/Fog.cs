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
        public int Speed { get; set; }
        public int enemiesConsumed { get; set; }

        public Texture2D sprite { get; set; }
        public Rectangle location;
        public Rectangle bounds;

        public Fog(Texture2D texture, Rectangle loc, Rectangle bound, int speed, int consumed)
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
            if(bounds.Intersects(player.location))
            {
                //for now the fog stops but i want to change this
                location.X += Speed/2;
                bounds.X += Speed/2;
            }
            else
            {
                location.X += Speed;
                bounds.X += Speed;
            }
            
        }


        public void damagePlayer(Player player)
        {
            //will take 6 ticks to kill

            if (player.location.Intersects(bounds))
            {
                if (player.invincibilityFrame <= 0)
                {
                    player.isHit = true;
                    player.invincibilityFrame = 150;
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
            if(location.Intersects(enemy.location))
            {
                enemiesConsumed += 1;
                enemy.isDead = true;
            }
            Speed += enemiesConsumed;
        }
    }
}
