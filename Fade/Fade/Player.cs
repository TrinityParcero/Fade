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
    class Player : Character
    {
        public int Damage{ get; set; }

        public int Health{ get; set; }

        public bool isDead{ get; set; }

        public int Speed{ get; set; }

        public int XPos { get; set; }

        public int YPos { get; set; }

        public Texture2D sprite { get; set; }


        public Player(Texture2D asset, int x, int y)
        {
            Damage = 1;
            Health = 3;
            isDead = false;
            Speed = 1;
            sprite = asset;
            XPos = x;
            YPos = y;
        }

        public void Attack()
        {
            throw new NotImplementedException();
            //if char is in attack pose-check for it
            //if enemy is in hitbox while char is attacking-deal damage
        }

        public void Jump()
        {
            throw new NotImplementedException();
            //jumping over tank is absolute limit of jump distance
        }

        public void RunLeft()
        {
            //hit wasd to go!
            XPos -= 1;
        }

        public void RunRight()
        {
            throw new NotImplementedException();
            //hit wasd to go!

        }

        public void takeDamage()
        {
            throw new NotImplementedException();
            //if enemy is in hitbox, take const damage
            //if enemy is in attack animation and youre in hitbox- damage
        }
    }
}
