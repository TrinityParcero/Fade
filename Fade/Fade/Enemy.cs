using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fade
{
    class Enemy : Character
    {
        public int Damage{ get; set; }

        public int Health{ get; set; }

        public bool isDead{ get; set; }

        public int Speed{ get; set; }

        public int XPos { get; set; }

        public int YPos { get; set; }

        public void Attack()
        {
            throw new NotImplementedException();
        }

        public void Jump()
        {
            throw new NotImplementedException();
        }

        public void RunLeft()
        {
            throw new NotImplementedException();
        }

        public void RunRight()
        {
            throw new NotImplementedException();
        }

        public void takeDamage()
        {
            throw new NotImplementedException();
        }
    }
}
