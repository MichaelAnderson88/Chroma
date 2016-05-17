using Chroma.GameClasses.Utilities.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chroma.GameClasses.Utilities.Weapons
{
    class LaserCannon : Weapon
    {
        Vector2 shipSpeed;

        /// <summary>
        /// Default 0 argument constructor
        /// </summary>
        public LaserCannon() { }

        /// <summary>
        /// Overloaded constructor to take in the fireRate of the weapon
        /// </summary>
        public LaserCannon(float rate, Vector2 speed)
        {
            projectiles = new List<Projectile>();
            rateOfFire = rate;

            shipSpeed = speed;

            //Default color of all projectiles
            colorOfProjectiles = Color.Orange;
        }

        /// <summary>
        /// Override method for fireProjectile. This weapon fires Pulse projectiles.
        /// The projectile is created which then gets updated from the weapon's update loop.
        /// The directional vector for the projectile's travel path will be set here.
        /// </summary>
        override public void fireProjectile(Vector2 dirV, Vector2 pos, GameTimer timer)
        {
            //First check to see if it's time to shoot a projectile
            rateCounter += (float)timer.UpdateInterval.TotalSeconds;

            if (rateCounter > rateOfFire)
            {
                //Creates the projectile. Passes the position of the weapon.
                Laser projectile = new Laser(pos, shipSpeed);

                //Sets the projectile's texture
                projectile.setTexture(projectileTexture);

                //Sets the color of the projectile
                projectile.setColor(colorOfProjectiles);

                //Adds the projectile to the list
                projectiles.Add(projectile);

                rateCounter = 0;
            }
        }
    }
}
