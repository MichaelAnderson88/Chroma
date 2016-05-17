using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chroma.GameClasses.Utilities.Projectiles
{
    /// <summary>
    /// The Pulse Projectile is the most basic projectile in the game. The projectile
    /// will have a relatively small hitbox and do a small amount of damage when it impacts.
    /// </summary>
    class Pulse : Projectile
    {
        /// <summary>
        /// Default 0 argument constructor.
        /// </summary>
        public Pulse() { }

        /// <summary>
        /// Constructor for Pulse objects. Sets default values of physical variables. Takes a Vector2
        /// value for the starting position of the projectile that will be relative to the ship.
        /// </summary>
        public Pulse(Vector2 pos, Vector2 dirV)
        {
            //Initializes physics variables
            speed = 300;
            velocity = new Vector2();
            dimenions = new Vector2(3, 10);
            position = new Vector2(pos.X - (dimenions.X / 2), pos.Y);
            targetDirection = dirV;

            hitBox = new Rectangle((int)position.X, (int)position.Y, (int)dimenions.X, (int)dimenions.Y);

            setVelocity();
        }

        /// <summary>
        /// Overridden update function specific to Pulse projectiles. This function will update the
        /// position of the projectile, run the animation engine and manage the removal flag.
        /// </summary>
        override public void update(GameTimer timer)
        {
            if (!remove)
            {
                checkRemoval();

                position.X += velocity.X * (float)timer.UpdateInterval.TotalSeconds;
                position.Y += velocity.Y * (float)timer.UpdateInterval.TotalSeconds;

                hitBox = new Rectangle((int)position.X, (int)position.Y, (int)dimenions.X, (int)dimenions.Y);

                animation();
            }
            else
            {
                hitBox = Rectangle.Empty;
            }
        }

        /// <summary>
        /// Checks to see if the projectile should be flagged for removal
        /// </summary>
        override public void checkRemoval()
        {
            if (position.X > 530 || position.X < -50 || position.Y > 600 || position.Y < -50)
            {
                remove = true;
            }
        }

        /// <summary>
        /// Overridden animation function specific to Pulse projectiles. This function will run a small
        /// animation system that will swap between textures for a particular shot. Aesthetic only.
        /// </summary>
        override protected void animation()
        {

        }
    }
}
