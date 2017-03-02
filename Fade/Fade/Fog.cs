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

        public Fog(Texture2D texture, Rectangle rec, int speed, int consumed)
        {
            sprite = texture;
            location = rec;
            Speed = speed;
            enemiesConsumed = consumed;
        }

        public void Move()
        {
            //move at about half player speed by default
            location.X += Speed;
        }
        public void damagePlayer(Player player)
        {
            //will take 6 ticks to kill
            if(location.Intersects(player.location))
            {
                player.Health -= 0.5;
            }
            
        }
        public void consumeEnemy(Enemy enemy)
        {
            if(location.Intersects(enemy.location))
            {
                enemiesConsumed += 1;
                enemy.isDead = true;
            }
        }
    }
}
