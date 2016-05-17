using Chroma.GameClasses.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chroma.GameClasses.EnemySystem.Bosses
{
    /// <summary>
    /// The AI associated with the Bosses of the game can make many changes to the variables of that boss. This is a class that
    /// bundles that information allowing it to be passed to the AI and returned with changed values easily.
    /// </summary>
    public class BossData
    {

        //These are the variables that could be changed in the AI state machine
        //Physical Variables
        public Vector2 position;
        public Vector2 speed;
        public float spawnTime;
        public Vector2 dimensions;

        //Statistic Variables
        public float hitpoints;
        public float fireRate;

        //Current Texture index to be drawn
        public int currentTexture;

        //List of Weapons
        public List<Weapon> weapons;

        //List of Shields
        public List<Shield> shields;

        //Texture for laser
        public Texture2D laserTex;

        /// <summary>
        /// This constructor sets default values for the variables in this object.
        /// </summary>
        public BossData()
        {
            position = new Vector2();
            speed = new Vector2();
            dimensions = new Vector2();

            weapons = new List<Weapon>();
            shields = new List<Shield>();

            fireRate = 0;
            spawnTime = 0;
            hitpoints = 0;
            currentTexture = 0;
        }
    }
}
