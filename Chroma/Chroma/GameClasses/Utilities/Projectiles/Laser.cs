using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chroma.GameClasses.Utilities.Projectiles
{
    /// <summary>
    /// Class for the Laser projectile. Extends projectile. This projectile will go from the ship to edge of screen
    /// for a short period of time. This projectile will not disappear when it collides with the player but will after
    /// a short period of time.
    /// </summary>
    class Laser : Projectile
    {
        int laserSize = 5;
        float duration = 3;
        float counter = 0;
        Vector2 shipSpeed;

        /// <summary>
        /// Default 0 argument constructor.
        /// </summary>
        public Laser() { }

        /// <summary>
        /// Constructor for Laser objects. Sets default values of physical variables. Takes a Vector2
        /// value for the starting position of the projectile that will be relative to the ship.
        /// </summary>
        public Laser(Vector2 pos, Vector2 shipSpeed)
        {
            //Initializes physics variables
            speed = 0; //Lasers stay relative to the ship until it's at full length
            velocity = new Vector2();
            position = pos;
            targetDirection = Vector2.Zero;
            this.shipSpeed = shipSpeed;

            hitBox = new Rectangle((int)position.X, (int)position.Y, 2, laserSize);

            setVelocity();
        }

        /// <summary>
        /// Overridden update function specific to Laser projectiles. This function will update the
        /// position of the projectile, run the animation engine and manage the removal flag.
        /// </summary>
        override public void update(GameTimer timer)
        {
            //Checks to see if the projectile needs to be removed
            checkRemoval();

            //Updates the position of the projectile
            position.X += shipSpeed.X * (float)timer.UpdateInterval.TotalSeconds;
            position.Y += shipSpeed.Y * (float)timer.UpdateInterval.TotalSeconds;

            //Checks to see if the laser is at full size
            if (laserSize < 50)
            {
                //Increase the size if not
                laserSize += 8;
            }
            else //Increase the speed of the projectile after it's at full size by it's growth speed
            {
                position.Y += 8;
            }

            //Counter updated to remove laser after a certain amount of time
            counter += 1.0f * (float)timer.UpdateInterval.TotalSeconds;

            hitBox = new Rectangle((int)position.X - 1, (int)position.Y - 1, 2, laserSize);

            //Updates the animation of the laser.
            animation();
        }

        /// <summary>
        /// Checks to see if the projectile should be flagged for removal
        /// </summary>
        override public void checkRemoval()
        {
            //Checks to see if the projectile has been fired long enough
            if (counter > duration)
            {
                remove = true;
                counter = 0;
            }
        }

        /// <summary>
        /// Overridden animation function specific to Laser projectiles. This function will run a small
        /// animation system that will swap between textures for a particular shot. Aesthetic only.
        /// </summary>
        override protected void animation()
        {

        }
    }
}
