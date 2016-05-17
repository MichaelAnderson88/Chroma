using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chroma.GameClasses.Utilities.Projectiles
{
    /// <summary>
    /// Abstract class that will contain all the base functions and variables
    /// required for Projectile objects.
    /// </summary>
    public abstract class Projectile : CollisionObject
    {
        //Physical variables
        protected Vector2 velocity;
        protected float speed;
        public Vector2 position;
        protected Vector2 targetDirection;
        protected Vector2 dimenions;

        //Aesthetic variables
        protected Texture2D texture;

        //Game play statistical variables
        public float damage;

        //Removal Flag
        public bool remove = false;

        /// <summary>
        /// Sets the projectile's Color value.
        /// </summary>
        public void setColor(Color c)
        {
            color = c;
        }

        /// <summary>
        /// Sets the projectile's texture
        /// </summary>
        public void setTexture(Texture2D tex)
        {
            texture = tex;
        }

        /// <summary>
        /// Sets the velocity of the projectile
        /// </summary>
        public void setVelocity()
        {
            velocity = targetDirection;
            velocity.Normalize();
            velocity *= speed;
        }

        /// <summary>
        /// Draws the projectile to the screen
        /// </summary>
        public void draw(SpriteBatch sb)
        {
            sb.Draw(texture, hitBox, color);
        }

        //Abstract functions that every projectile type requires
        abstract public void update(GameTimer timer);
        abstract protected void animation();
        abstract public void checkRemoval();
    }
}
