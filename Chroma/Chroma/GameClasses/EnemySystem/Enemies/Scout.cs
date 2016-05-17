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
    /// Scout Enemy Class. Scouts are very small and fast moving ships that do minimal damage to the 
    /// player. Their main role is for pestering the player in swarms and distracting the player from
    /// larger Enemy targets.
    /// </summary>
    class Scout : Enemy
    {
        //Location of the weapon on the unit. Used for starting projectile position
        Vector2 weaponLocation;

        private bool evasiveLeft, evasiveRight;

        /// <summary>
        /// Overload Constructor for the Scout Enemy object. Sets properties unique to the Scout 
        /// Enemy.
        /// </summary>
        public Scout(Vector2 spawnloc)
        {
            //Default color of the Scout. Temporarily used to identify scouts until textures are created
            color = Color.YellowGreen;

            type = EnemyType.Scout;

            dimensions = new Vector2(20, 30);
            position = new Vector2(spawnloc.X - (dimensions.X/2), spawnloc.Y);
            speed = new Vector2(0, 100);
            hitPoints = 250;

            //Sets the weapon's location to the front of the ship
            weaponLocation = new Vector2((dimensions.X / 2), dimensions.Y);

            hitBox = new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y);

            fireRate = 0.5f;

            weapon = new PulseCannon(fireRate);
            shield = new BasicShield(position, dimensions);
            shield.setColor(Color.Red);

            evasiveLeft = false;
            evasiveRight = true;
        }

        /// <summary>
        /// Updates the Enemy's position or hitpoints. Sets a flag if the Enemy is off the screen or is destroyed.
        /// </summary>
        override public void update(GameTimer timer)
        {
            if (!remove)
            {
                //Updates the position of the Enemy based on speed.
                position.Y += speed.Y * (float)timer.UpdateInterval.TotalSeconds;
                position.X += speed.X * (float)timer.UpdateInterval.TotalSeconds;

                //Updates the visual position of the Enemy
                hitBox = new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y);

                //Updates the ship's movement patterns
                if (evasiveRight)
                {
                    speed.X += (float)(60.0 * timer.UpdateInterval.TotalSeconds);

                    if (speed.X > 70)
                    {
                        evasiveLeft = true;
                        evasiveRight = false;
                    }
                }
                else if (evasiveLeft)
                {
                    speed.X -= (float)(60.0 * timer.UpdateInterval.TotalSeconds);

                    if (speed.X < -70)
                    {
                        evasiveLeft = false;
                        evasiveRight = true;
                    }
                }

                //Check if the ship runs off the screen
                if (position.Y > 600)
                {
                    remove = true;
                }

                //update the Weapon
                weapon.update(timer, new Vector2(0, 810), (position + weaponLocation));

                //updates the shield
                shield.update(timer, position);

            }
            animations();
        }

        /// <summary>
        /// Sets the Scout's Weapon's projectile texture
        /// </summary>
        override public void setProjectileTexture(Texture2D tex)
        {
            weapon.setProjectileTexture(tex);
        }

        /// <summary>
        /// Sets the Scout's Sheild texture
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
            if (animationCounter % 5 == 0 && !remove)
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
