using Chroma.GameClasses.Controllers;
using Chroma.GameClasses.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chroma.GameClasses.EnemySystem
{
    /// <summary>
    /// Abstract class for enemy ships. Each enemy ship will be derived from this base class.
    /// </summary>
    public abstract class Enemy : CollisionObject
    {
        //Sub-System Variables
        public Shield shield;
        public Weapon weapon;

        //Physical Variables
        protected Vector2 dimensions;
        protected Vector2 position;
        protected Vector2 speed;
        protected float spawnTime;

        //Aesthetic Variables
        public List<Texture2D> textures;
        public List<Texture2D> explosionList;
        public int currentTexture = 0;
        public int animationCounter = 0;
        protected int animationAdder = 1;
        public bool exploded = false;
        
        //Statistic Variables
        protected EnemyType type;
        protected float fireRate;
        public float hitPoints;

        //Removal Flag
        public bool remove = false;

        /// <summary>
        /// Returns the Enemy's type.
        /// </summary>
        public EnemyType getType()
        {
            return type;
        }

        /// <summary>
        /// Sets the Enemy's texture.
        /// </summary>
        public void setTextures(List<Texture2D> textures)
        {
            this.textures = textures;
        }

        /// <summary>
        /// Returns the time which the ship will be spawned
        /// </summary>
        public float getSpawnTime()
        {
            return spawnTime;
        }

        /// <summary>
        /// Sets the time which the ship will be spawned
        /// </summary>
        public void setSpawnTime(float st)
        {
            spawnTime = st;
        }

        /// <summary>
        /// Draws the Enemy, their shield and their projectiles on the screen.
        /// </summary>
        public void draw(SpriteBatch sb)
        {
            if (!remove)
            {
                //Draws the shield first so it gets drawn over by the enemy itself
                shield.draw(sb);

                //Draws the projectiles
                weapon.drawProjectiles(sb);
            }

            //Draws the enemy
            sb.Draw(textures[currentTexture], hitBox, Color.White);
        }

        /// <summary>
        /// Returns if the removal flag has been set
        /// </summary>
        /// <returns></returns>
        public bool checkRemoval()
        {
            return remove;
        }

        /// <summary>
        /// Sets the explosion texture list locally
        /// </summary>
        /// <param name="explosions"></param>
        public void setExplosions(List<Texture2D> explosions)
        {
            explosionList = explosions;
        }

        //Abstract functions that will be overridden
        abstract public void update(GameTimer timer);
        abstract public void setProjectileTexture(Texture2D tex);
        abstract public void setShieldTexture(Texture2D tex);
        abstract public void animations();
    }
}
