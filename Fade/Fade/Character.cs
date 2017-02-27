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
        int XPos { get; set; }
        int YPos { get; set; }

        void RunLeft();
        void RunRight();
        void Jump();
        void Attack();
        void takeDamage();
    }
}
