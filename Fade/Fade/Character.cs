using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fade
{
    interface Character
    {
        int Health { get; set; }
        int Damage { get; set; }
        int Speed { get; set; }
        bool isDead { get; set; }

        void Run();
        void Jump();
        void Attack();
        void takeDamage();
    }
}
