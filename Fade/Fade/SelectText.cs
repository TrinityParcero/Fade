using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fade
    //Trinity
    //selectible menu options
{
    class SelectText
    {
        //PROPS
        public bool IsSelected { get; set; }
        public Color DefaultColor { get; set; }
        public Color SelectColor { get; set; }

        //CONSTRUCTOR
        public SelectText()
        {
            IsSelected = false;
            DefaultColor = Color.White;
            SelectColor = Color.Black;
        }

        public SelectText(bool select, Color dColor, Color sColor)
        {
            IsSelected = select;
            DefaultColor = dColor;
            SelectColor = sColor;
        }

        //METHODS
        public void DrawSelectText(SpriteBatch sb, SpriteFont sf, string text, Vector2 position)
        {
            if (IsSelected)
            {
                sb.DrawString(sf, text, position, SelectColor);
            }
            else
            {
                sb.DrawString(sf, text, position, DefaultColor);
            }

        }
    }
}
