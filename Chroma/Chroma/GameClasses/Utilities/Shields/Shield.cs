using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chroma.GameClasses.Utilities
{
    /// <summary>
    /// Abstract class that will contain all the base functions and variables
    /// required for Shield objects.
    /// </summary>
    public abstract class Shield
    {
        //Physical variables
        public Rectangle hitBox;
        protected Vector2 position;
        protected Vector2 size;

        //Aethetic variables
        protected Texture2D texture;

        //Statistical variables
        public Color color;

        protected float offset = 0;

        /// <summary>
        /// Sets the shield's texture
        /// </summary>
        public void setTexture(Texture2D tex)
        {
            texture = tex;
        }

        /// <summary>
        /// Sets the color of the Shield
        /// </summary>
        public void setColor(Color c)
        {
            color = c;
        }

        /// <summary>
        /// Draws the shield to the screen
        /// </summary>
        public void draw(SpriteBatch sb)
        {
            sb.Draw(texture, hitBox, color * 0.75f);
        }

        //Abstract methods
        abstract public void update(GameTimer timer, Vector2 shipPos);
    }
}
