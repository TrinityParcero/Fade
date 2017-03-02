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
    class Enemy : Character
    {
        public int Damage{ get; set; }

        public int Health{ get; set; }

        public bool isDead{ get; set; }

        public int Speed{ get; set; }

        public Texture2D sprite { get; set; }

        public Rectangle location;

        public Enemy(Texture2D texture, Rectangle rect)
        {
            sprite = texture;
            location = rect;
        }

        public void Attack()
        {
            throw new NotImplementedException();
        }

        public void Jump()
        {
            throw new NotImplementedException();
        }

        public void Run(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void takeDamage()
        {
            throw new NotImplementedException();
        }
    }
}
