using Chroma.GameClasses.Controllers;
using Chroma.GameClasses.Utilities.Shields;
using Chroma.GameClasses.Utilities.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chroma.GameClasses.EnemySystem.Enemies
{
    /// <summary>
    /// Grunts Enemy Class. Grunts are slow moving ships equipped with dual pulse weapons. Shots are highly
    /// inaccurate as they only fire straight. Used as a fodder ship that can also zone the player to specific 
    /// regions of space. Grunts slowly move into the screen and slowly make their way to the bottom of the
    /// screen where they are de-spawned if they are not destroyed before hand.
    /// </summary>
    class Grunt : Enemy
    {
        Vector2 weaponLocation;

        /// <summary>
        /// Overload Constructor for the Grunt Enemy object. Sets properties unique to the Grunt 
        /// Enemy.
        /// </summary>
        public Grunt(Vector2 spawnloc)
        {
            type = EnemyType.Grunt;

            hitPoints = 500;

            dimensions = new Vector2(64, 37);
            position = new Vector2(spawnloc.X - (dimensions.X / 2), spawnloc.Y);

            speed = new Vector2(0, 40);

            //Sets the weapon's location to the front of the ship
            weaponLocation = new Vector2((dimensions.X / 2), dimensions.Y);

            //Default rate of fire
            fireRate = 2;

            hitBox = new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y);

            //Initialize default weapon
            weapon = new LaserCannon(fireRate, speed);

            //Initialize default shield
            shield = new BasicShield(position, dimensions);
        }

        /// <summary>
        /// Overload Update function. Updates the Enemy's position, hitpoints, and weapon. Sets a flag if the Enemy is off the 
        /// screen or is destroyed.
        /// </summary>
        override public void update(GameTimer timer)
        {
            //If the ship runs off the screen, flag for removal
            if (position.Y > 600)
            {
                remove = true;
            }

            //If the ship isn't flagged for removal, update as normal
            if (!remove)
            {
                //Updates the position of the Enemy based on speed.
                position.Y += speed.Y * (float)timer.UpdateInterval.TotalSeconds;

                //update the Weapon
                weapon.update(timer, new Vector2(0, 810), (position + weaponLocation));

                //update the Shield
                shield.update(timer, position);
            }

            //Updates the visual position of the Enemy
            hitBox = new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y);

            animations();
        }

        /// <summary>
        /// Sets the Grunt's Weapon's projectile texture
        /// </summary>
        override public void setProjectileTexture(Texture2D tex)
        {
            weapon.setProjectileTexture(tex);
        }

        /// <summary>
        /// Sets the Grunt's Sheild texture
        /// </summary>
        override public void setShieldTexture(Texture2D tex)
        {
            shield.setTexture(tex);
        }

        /// <summary>
        /// Function that runs the sprite animation for this enemy
        /// </summary>
        public override void animations()
        {
            //Checks to see if the texture in the animation should be changed
            if ((animationCounter % 5 == 0) && !remove)
            {
                if (currentTexture == 3)
                {
                    animationAdder = -1;
                }
                else if (currentTexture == 0)
                {
                    animationAdder = 1;
                }

                currentTexture += animationAdder;
            }

            //Runs explosion animation
            if ((animationCounter % 5 == 0) && remove)
            {
                if (currentTexture >= 3)
                {
                    exploded = true;
                    currentTexture -= 1;
                }
                else
                {
                    currentTexture += 1;
                }
            }

            animationCounter++;
        }
    }
}
