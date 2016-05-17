using Chroma.GameClasses.Utilities.Projectiles;
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
    /// required for Weapon objects. Weapon Classes will act as projectile controller
    /// classes. They will manage lists of projectiles, how fast projectiles can be shot
    /// and the drawing/updating/deconstruction of projectiles.
    /// </summary>
    public abstract class Weapon
    {
        //Projectiles Associated with the weapon
        public List<Projectile> projectiles;

        //Physical Properties
        protected float rateOfFire;
        protected float rateCounter = 0;
        protected Vector2 position;

        //Weapon attributes
        protected Color colorOfProjectiles;
        protected float damageModifier;

        //Aesthetic properties
        protected Texture2D projectileTexture;

        /// <summary>
        /// Changes the color of the projectiles.
        /// </summary>
        public void setProjectileColor(Color pc)
        {
            colorOfProjectiles = pc;
        }

        /// <summary>
        /// Sets the texture of the projectiles fired by this weapon.
        /// </summary>
        public void setProjectileTexture(Texture2D tex)
        {
            projectileTexture = tex;
        }

        /// <summary>
        /// This update method is largely for managing the projectile list. If projectiles
        /// have been active for their lifespan, then they are removed. This is also where
        /// each projectile's update will be called. This update also updates the position of the 
        /// weapon which is relative to the ship.
        /// </summary>
        public void update(GameTimer timer, Vector2 dirV, Vector2 pos)
        {
            //Updates the weapon's position
            position = pos;

            //Creates a new projectile if the fireRate determines another is shot.
            fireProjectile(dirV, position, timer);
        }

        /// <summary>
        /// Draws all the projectiles in the List
        /// </summary>
        public void drawProjectiles(SpriteBatch sb)
        {
            foreach (Projectile p in projectiles)
            {
                p.draw(sb);
            }
        }

        /// <summary>
        /// Setter for rate of Fire
        /// </summary>
        public void setRate(float newRate)
        {
            rateOfFire = newRate;
        }

        //Abstract as different weapons may fire different projectiles
        abstract public void fireProjectile(Vector2 dirV, Vector2 pos, GameTimer gameTimer);
    }
}
