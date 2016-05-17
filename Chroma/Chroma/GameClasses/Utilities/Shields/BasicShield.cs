using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chroma.GameClasses.Utilities.Shields
{
    /// <summary>
    /// The most basic type of shield in the game. This shield will protect
    /// the ship from damage based on the color of the shield.
    /// </summary>
    class BasicShield : Shield
    {
        /// <summary>
        /// Default 0 argument constructor.
        /// </summary>
        public BasicShield() { }

        public BasicShield(Vector2 shipPos, Vector2 shipSize)
        {
            if (shipSize.X > shipSize.Y)
            {
                offset = shipSize.X * 0.2f;
            }
            else
            {
                offset = shipSize.Y * 0.2f;
            }

            position.X = shipPos.X - (offset / 2);
            position.Y = shipPos.Y - (offset / 2);
            size.X = shipSize.X + offset;
            size.Y = shipSize.Y + offset;

            hitBox = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);

            //Default color of the shield will be Cornflower blue
            color = Color.CornflowerBlue;
        }

        /// <summary>
        /// Overridden update method specific to the BasicShield Shield type
        /// </summary>
        override public void update(GameTimer timer, Vector2 shipPos)
        {
            position.X = shipPos.X - (offset / 2);
            position.Y = shipPos.Y - (offset / 2);

            hitBox = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }
    }
}
