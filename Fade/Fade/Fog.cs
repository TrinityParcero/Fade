using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fade
{
    class Fog
    {
        public int Speed { get; set; }
        public int enemiesConsumed { get; set; }

        public void Move()
        {
            //move at about half player speed by default
        }
        public void damagePlayer()
        {
            //will take 6 ticks to kill
        }
        public void consumeEnemy()
        {
            //
        }
    }
}
