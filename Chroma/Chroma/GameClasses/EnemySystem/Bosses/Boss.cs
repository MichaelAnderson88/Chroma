using Chroma.GameClasses.Controllers;
using Chroma.GameClasses.EnemySystem.Bosses;
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
    /// This abstract class will contain the properties that make up a Boss in the game. It will also run an AI class that
    /// will be used to control it's movement and attacking patterns. This class will come into play when the Boss
    /// state of the game is enabled. Bosses may be made up of multiple parts and sections so each Boss will have it's own
    /// unique class extended from this base class. At the very least, each boss will have the properties listed in this class.
    /// </summary>
    public abstract class Boss : CollisionObject
    {
        //Sub-System Variables
        public List<Shield> shields;
        public List<Weapon> weapons;

        //Aesthetic Variables
        protected List<Texture2D> textures;

        //Removal Flag
        public bool remove = false;
        
        //State machine for boss
        public AI ai;

        //Type of Boss
        protected EnemyType type;

        //Bundle of Boss Data
        public BossData data;

        /// <summary>
        /// Returns the Enemy's type.
        /// </summary>
        public EnemyType getType()
        {
            return type;
        }

        /// <summary>
        /// Sets the Boss's texture.
        /// </summary>
        public void setTexture(List<Texture2D> textures)
        {
            this.textures = textures;
        }

        /// <summary>
        /// Returns the time which the Boss will be spawned
        /// </summary>
        public float getSpawnTime()
        {
            return data.spawnTime;
        }

        /// <summary>
        /// Sets the time which the Boss will be spawned
        /// </summary>
        public void setSpawnTime(float st)
        {
            data.spawnTime = st;
        }

        /// <summary>
        /// Returns if the removal flag has been set
        /// </summary>
        /// <returns></returns>
        public bool checkRemoval()
        {
            return remove;
        }

        abstract public void draw(SpriteBatch sb);
        abstract public void update(GameTimer timer);
        abstract public void setProjectileTextures(Texture2D tex);
        abstract public void setShieldTextures(Texture2D tex);
    }
}
