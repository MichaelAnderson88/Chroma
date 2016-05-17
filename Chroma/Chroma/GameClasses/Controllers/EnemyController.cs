using Chroma.GameClasses.EnemySystem;
using Chroma.GameClasses.EnemySystem.Bosses;
using Chroma.GameClasses.EnemySystem.Enemies;
using Chroma.GameClasses.LevelSystem;
using Chroma.GameClasses.Utilities;
using Chroma.GameClasses.Utilities.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Chroma.GameClasses.Controllers
{
    /// <summary>
    /// Enumeration used to determine the type of enemy and set variables associated with that
    /// enemy. This Enum includes Boss types.
    /// </summary>
    public enum EnemyType
    {
        Scout = 0,
        Grunt = 1,
        BossTheShip = 2
    }

    public class EnemyController
    {
        // List of Enemy and Boss objects
        public List<Enemy> enemies;
        public List<Enemy> activeEnemies;
        public List<Boss> bosses;
        public List<Boss> activeBosses;
        public List<Projectile> activeProjectiles;

        public List<Texture2D> explosionList;

        //Numeric Score Variable
        public int score;

        // List of Spawn Locations
        public List<Vector2> spawns;

        // Timer used for spawning. Starts when the Controller is created (At the start of a Level)
        float gameTimer = 0;

        //Local SoundManager Object
        public SoundManager soundManager;

        public EnemyController(SoundManager sm)
        {
            //Sets local SoundManager
            soundManager = sm;

            //Sets default score
            score = 0;

            // Initializes the list of Enemy Objects
            enemies = new List<Enemy>();
            activeEnemies = new List<Enemy>();
            bosses = new List<Boss>();
            activeBosses = new List<Boss>();
            activeProjectiles = new List<Projectile>();
            explosionList = new List<Texture2D>();

            // Initializes the spawn location list and populates it with the spawning locations
            spawns = new List<Vector2>();
            Vector2 spawn1 = new Vector2(48, 0);
            Vector2 spawn2 = new Vector2(144, 0);
            Vector2 spawn3 = new Vector2(240, 0);
            Vector2 spawn4 = new Vector2(336, 0);
            Vector2 spawn5 = new Vector2(432, 0);
            spawns.Add(spawn1);
            spawns.Add(spawn2);
            spawns.Add(spawn3);
            spawns.Add(spawn4);
            spawns.Add(spawn5);
        }

        /// <summary>
        /// Creates enemies based on data read in from Level file
        /// </summary>
        /// <param name="loadedEnemies">Each index of the array represents a specific enemy,
        /// while the value at that index is amount of that enemy type</param>
        public void populateEnemyList(Level loadedEnemies)
        {
            // foreach loop for creating enemies. Enemy objects are created then added to the list.
            foreach (Chroma.GameClasses.LevelSystem.Level.Enemydata e in loadedEnemies.enemies)
            {
                // Scouts
                if (e.name.Equals("Scout"))
                {
                    Scout temp = new Scout(spawns[e.spawnLoc]);
                    temp.setSpawnTime(e.spawnTime);
                    enemies.Add(temp);
                }

                // Grunts
                if (e.name.Equals("Grunt"))
                {
                    Grunt temp = new Grunt(spawns[e.spawnLoc]);
                    temp.setSpawnTime(e.spawnTime);
                    enemies.Add(temp);
                }

                //Boss - The Ship
                if (e.name.Equals("BossTheShip"))
                {
                    TheShip temp = new TheShip(spawns[e.spawnLoc]);
                    temp.setSpawnTime(e.spawnTime);
                    bosses.Add(temp);
                }
            }
        }

        public void loadEnemyAssets(ContentManager content)
        {
            // Loads all Enemy textures into temporary memory.
            Texture2D pixel = content.Load<Texture2D>("Textures/Utilities/pixel");
            
            //Boss Texture Group
            List<Texture2D> bossTextures = new List<Texture2D>();
            Texture2D bossShip1 = content.Load<Texture2D>("Textures/BossShip/BossShip1");
            Texture2D bossShip2 = content.Load<Texture2D>("Textures/BossShip/BossShip2");
            Texture2D bossShip3 = content.Load<Texture2D>("Textures/BossShip/BossShip3");
            Texture2D bossShip4 = content.Load<Texture2D>("Textures/BossShip/BossShip4");
            Texture2D bossShip5 = content.Load<Texture2D>("Textures/BossShip/BossShip5");
            Texture2D bossShip6 = content.Load<Texture2D>("Textures/BossShip/BossShip6");
            bossTextures.Add(bossShip1);
            bossTextures.Add(bossShip2);
            bossTextures.Add(bossShip3);
            bossTextures.Add(bossShip4);
            bossTextures.Add(bossShip5);
            bossTextures.Add(bossShip6);

            //Scout Texture Group
            List<Texture2D> scoutTextures = new List<Texture2D>();
            Texture2D scout1 = content.Load<Texture2D>("Textures/Enemies/Scout1");
            Texture2D scout2 = content.Load<Texture2D>("Textures/Enemies/Scout2");
            Texture2D scout3 = content.Load<Texture2D>("Textures/Enemies/Scout3");
            Texture2D scout4 = content.Load<Texture2D>("Textures/Enemies/Scout4");
            scoutTextures.Add(scout1);
            scoutTextures.Add(scout2);
            scoutTextures.Add(scout3);
            scoutTextures.Add(scout4);

            //Grunt Texture Group
            List<Texture2D> gruntTextures = new List<Texture2D>();
            Texture2D grunt1 = content.Load<Texture2D>("Textures/Enemies/Grunt1");
            Texture2D grunt2 = content.Load<Texture2D>("Textures/Enemies/Grunt2");
            Texture2D grunt3 = content.Load<Texture2D>("Textures/Enemies/Grunt3");
            Texture2D grunt4 = content.Load<Texture2D>("Textures/Enemies/Grunt4");
            gruntTextures.Add(grunt1);
            gruntTextures.Add(grunt2);
            gruntTextures.Add(grunt3);
            gruntTextures.Add(grunt4);

            //Loads Projectile Texture
            Texture2D pulseShot = content.Load<Texture2D>("Textures/Utilities/EnemyPulse");

            //Loads Shield Texture
            Texture2D shield = content.Load<Texture2D>("Textures/Utilities/Shield");

            // Switch statement to determine which textures are loaded to which enemies
            foreach (Enemy e in enemies)
            {
                e.setExplosions(explosionList);
                // Switches based on the Enemy's type. Sets associated texture.
                switch (e.getType())
                {
                    case EnemyType.Scout:
                        e.setTextures(scoutTextures);
                        e.setProjectileTexture(pulseShot);
                        e.setShieldTexture(shield);
                        break;
                    case EnemyType.Grunt:
                        e.setTextures(gruntTextures);
                        e.setProjectileTexture(pulseShot);
                        e.setShieldTexture(shield);
                        break;
                }
            }

            //Checks to see if there is a boss to be loaded
            if (bosses.Count != 0)
            {
                foreach (Boss b in bosses)
                {
                    //Switches based on the Boss's type and sets up necessary textures.
                    switch (b.getType())
                    {
                        case EnemyType.BossTheShip:
                            b.setTexture(bossTextures);
                            //TODO these will need to be able to set multiple textures
                            b.setProjectileTextures(pulseShot);
                            b.data.laserTex = pulseShot;
                            b.setShieldTextures(shield);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Draws the enemies and projectiles in thier active lists.
        /// </summary>
        public void draw(SpriteBatch sb)
        {
            // Draws Enemies
            foreach (Enemy e in activeEnemies)
            {
                e.draw(sb);
            }

            //Draw active Bosses
            foreach (Boss b in activeBosses)
            {
                b.draw(sb);
            }

            // Draws Projectiles
            foreach (Projectile p in activeProjectiles)
            {
                p.draw(sb);
            }
        }

        /// <summary>
        /// Base Update for the EnemyController class. This function will run sub-update functions that will update
        /// the position, hitpoints, removal of Enemy objects as well as the Enemy spawning algorithm.
        /// </summary>
        /// <param name="timer"></param>
        public void update(GameTimer timer)
        {
            // First, check to see if new Enemy objects become active
            spawner();

            // Then the positions of the remaining Enemy objects are updated
            updateEnemy(timer);

            // Update the activeProjectiles
            updateProjectiles(timer);

            // Finally, the Enemy list checks for Enemy objects flagged for removal.
            checkRemoval();

            // Updates the total elapsed time
            gameTimer += 1.0f * (float)timer.UpdateInterval.TotalSeconds;
        }

        /// <summary>
        /// Updates the projectiles in the activeProjectiles List
        /// </summary>
        /// <param name="timer"></param>
        public void updateProjectiles(GameTimer timer)
        {
            foreach (Projectile p in activeProjectiles)
            {
                p.update(timer);
            }
        }

        /// <summary>
        /// Checks for active projectiles and calls update on Enemy objects
        /// </summary>
        public void updateEnemy(GameTimer timer)
        {
            foreach (Enemy e in activeEnemies)
            {
                // Checks to see if the Enemy object has fired any projectiles
                if (e.weapon.projectiles.Count > 0)
                {
                    // if it has, add to activeProjectile List
                    foreach (Projectile p in e.weapon.projectiles)
                    {
                        activeProjectiles.Add(p);
                    }

                    e.weapon.projectiles.Clear();
                }

                //Checks to see if the player has collided with an Enemy projectile
                if (e.hasCollided)
                {
                    int red = 0;
                    int green = 0;
                    int blue = 0;

                    //Compute damage taken from collision based on color of projectile
                    if ((e.shield.color.R - e.collisionColor.R) < 0)
                        red = (e.shield.color.R - e.collisionColor.R) * -1;

                    if ((e.shield.color.G - e.collisionColor.G) < 0)
                        green = (e.shield.color.G - e.collisionColor.G) * -1;

                    if ((e.shield.color.B - e.collisionColor.B) < 0)
                        blue = (e.shield.color.B - e.collisionColor.B) * -1;

                    int damageTaken = red + green + blue;

                    e.hitPoints -= damageTaken;

                    e.hasCollided = false;

                    //If the player's hitpoints is less than 0, the player is dead
                    if (e.hitPoints <= 0)
                    {
                        score += (int)(e.hitPoints * -1);
                        e.remove = true;
                        e.textures = e.explosionList;
                        e.currentTexture = 0;
                        e.animationCounter = 0;
                        e.hitBox.Width = 50;
                        e.hitBox.Height = 50;
                        e.collidable = false;
                    }
                }

                // Then update then Enemy
                e.update(timer);
            }

            foreach (Boss b in activeBosses)
            {
                // Checks to see if the Boss has fired any projectiles
                foreach(Weapon w in b.data.weapons)
                {
                    if (w.projectiles.Count > 0)
                    {
                        foreach (Projectile p in w.projectiles)
                        {
                            activeProjectiles.Add(p);
                        }

                        w.projectiles.Clear();
                    }
                }

                //Checks to see if the player has collided with an Enemy projectile
                if (b.hasCollided)
                {
                    int red = 0;
                    int green = 0;
                    int blue = 0;

                    //Compute damage taken from collision based on color of projectile
                    if ((b.data.shields[0].color.R - b.collisionColor.R) < 0)
                        red = (b.data.shields[0].color.R - b.collisionColor.R) * -1;

                    if ((b.data.shields[0].color.G - b.collisionColor.G) < 0)
                        green = (b.data.shields[0].color.G - b.collisionColor.G) * -1;

                    if ((b.data.shields[0].color.B - b.collisionColor.B) < 0)
                        blue = (b.data.shields[0].color.B - b.collisionColor.B) * -1;

                    int damageTaken = red + green + blue;

                    b.data.hitpoints -= damageTaken;

                    b.hasCollided = false;

                    //If the player's hitpoints is less than 0, the player is dead
                    if (b.data.hitpoints <= 0)
                    {
                        score += (int)(100 * b.data.hitpoints * -1);
                        b.remove = true;
                    }
                }

                //Then update the Boss
                b.update(timer);
            }
        }

        /// <summary>
        /// Checks to see if any Enemy objects in the active List are flagged for removal.
        /// </summary>
        public void checkRemoval()
        {
            // Used to keep track of indices
            List<Enemy> removalIndices = new List<Enemy>();
            List<Boss> removalBossIndices = new List<Boss>();

            foreach (Enemy e in activeEnemies)
            {
                // If the flag has been set
                if (e.checkRemoval() && e.exploded)
                {
                    // Add the index of the Enemy to a List
                    removalIndices.Add(e);
                }
            }

            // If at least one Enemy has been flagged for removal
            if (removalIndices.Count > 0)
            {
                // Iterate through the index list
                foreach (Enemy ri in removalIndices)
                {
                    // Remove the objects in the removal List
                    activeEnemies.Remove(ri);
                }
            }

            //Check for Boss removal
            foreach (Boss b in activeBosses)
            {
                // if flagged for removal
                if (b.checkRemoval())
                {
                    removalBossIndices.Add(b);
                }
            }

            // If at least one Boss has been flagged for removal
            if (removalBossIndices.Count > 0)
            {
                // Iterate through the index list
                foreach (Boss br in removalBossIndices)
                {
                    // Remove the objects in the removal List
                    activeBosses.Remove(br);
                }
            }

            // Initializes a new removal list
            List<Projectile> removalList = new List<Projectile>();

            // Checks all projectiles in the Projectile List for the removal flag
            foreach (Projectile p in activeProjectiles)
            {
                if (p.remove)
                {
                    // If found, add to removalList
                    removalList.Add(p);
                }
            }

            // Remove all projectiles that were put in removalList
            if (removalList.Count > 0)
            {
                foreach (Projectile p in removalList)
                {
                    activeProjectiles.Remove(p);
                }
            }
        }

        /// <summary>
        /// Enemy Spawning system. Will move enemies from the base list to the active list.
        /// </summary>
        /// <param name="timer">Game timer</param>
        public void spawner()
        {
            // Used to keep track of indices
            List<Enemy> removalIndices = new List<Enemy>();
            List<Boss> removalBossIndices = new List<Boss>();

            // Iterates through the base Enemy List to check for Enemy objects that need to be spawned
            foreach (Enemy e in enemies)
            {
                if (e.getSpawnTime() < gameTimer)
                {
                    // Adds the Enemy object to the active List
                    activeEnemies.Add(e);

                    // Flags the index for removal
                    removalIndices.Add(e);
                }
            }

            // Iterates through the base Boss List to check for Boss objects that need to be spawned
            foreach (Boss b in bosses)
            {
                if (b.getSpawnTime() < gameTimer)
                {
                    // Adds the Boss object to the active List
                    activeBosses.Add(b);

                    // Flags the index for removal
                    removalBossIndices.Add(b);
                }
            }

            // If at least one Enemy has been flagged for removal
            if (removalIndices.Count > 0)
            {
                // Iterate through the index list
                foreach (Enemy ri in removalIndices)
                {
                    // Remove the objects in the removal List
                    enemies.Remove(ri);
                }
            }

            // If at least one Boss has been flagged for removal
            if (removalBossIndices.Count > 0)
            {
                // Iterate through the index list
                foreach (Boss br in removalBossIndices)
                {
                    // Remove the objects in the removal List
                    bosses.Remove(br);
                }
            }
        }

        //Sets the explosion list for this controller
        public void setExplosions(List<Texture2D> explosions)
        {
            explosionList = explosions;
        }
    }
}
