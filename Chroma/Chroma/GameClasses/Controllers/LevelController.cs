using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Chroma.GameClasses.Utilities.Projectiles;
using Chroma.GameClasses.EnemySystem;
using Chroma.GameClasses.LevelSystem;
using System.Collections.Generic;
using Chroma.GameClasses.Utilities.QuadTree;
using Chroma.GameClasses.PlayerSystem;
using Chroma.GameClasses.Utilities.Particle_System;
using Chroma.GameClasses.Utilities.FrontendBackend;
using Chroma.GameClasses.Utilities;
using Microsoft.Xna.Framework.Input.Touch;
using System.Windows.Navigation;

namespace Chroma.GameClasses.Controllers
{
    /// <summary>
    /// Enumeration used to determine the current state of the game. Will be used to run the state machine.
    /// </summary>
    public enum GameState
    {
        Level = 0, // Basic time during level play
        Victory = 1, // The time after a level has been beaten to show score or if the player dies
        Pause = 2, // The time that the game should freeze update
        GameOver = 3 // The end of the game
    }

    /// <summary>
    /// Enumeration that will be used to sort the CollisionObjects that are sent into the quad tree. This enum will 
    /// also allow for proper ignoring of collisions between Projectiles fired by Enemies that collide with Enemies.
    /// </summary>
    public enum CollisionObjectType
    {
        Player = 0,
        Boss = 1,
        Enemy = 2,
        PlayerProjectile = 3,
        EnemyProjectile = 4
    }

    /// <summary>
    /// Main Gameplay Class. This class will run the core aspects of the game and send whatever needed information
    /// to respective controller classes or the GamePage for draws.
    /// </summary>
    public class LevelController
    {
        //DataPackage object sent from the frontend pages
        DataPackage data;

        //Level loading class
        LevelLoader loader;

        //Controller classes
        PlayerController playerControl;
        EnemyController enemyControl;

        //Pixel Asset that will also work as a cache testing asset
        Texture2D pixel;

        //Locally stores the ContentManager for use later
        ContentManager content;

        // Stores the current level of the game
        int currentlevel = 1;

        // The current state of the game
        public GameState state;

        //QuadTree Object
        QTNode quadTree;

        //Collidable objects
        List<CollisionObject> objects;

        //Particle Pools
        List<ParticlePool> pools;

        //Touch control
        TouchCollection touches;

        //Local SoundManager object
        public SoundManager soundManager;

        //Highscore variable
        HighScore hs;

        //FPS variables
        SpriteFont font;
        int total_frames = 0;
        float elapsed_time = 0.0f;
        int fps = 0;

        //Screen Variables
        Rectangle fadeout;
        Rectangle pausescreen;
        Rectangle menuButton;
        Rectangle resumeButton;
        Rectangle pauseIcon;
        String scoreString;
        Texture2D menuTex;
        Texture2D resumeTex;
        Texture2D pauseBackground;
        Texture2D pauseButTex;
        Texture2D nextTex;
        Texture2D restartTex;

        //Explosion texture list
        List<Texture2D> explosionList;


        //Nav flag
        public bool mainMenu = false;

        /// <summary>
        /// Default constructor for the LevelController class. Will create instances of the other
        /// game controllers.
        /// </summary>
        public LevelController(ContentManager content, DataPackage frontendData, SoundManager sm)
        {
            //Stores data from the front end locally
            data = frontendData;

            //Sets up the HS object
            hs = new HighScore();

            //Inits the pause screen for use later
            initPauseScreen();

            //Sets default state
            state = GameState.Level;

            currentlevel = data.level;

            //Stores ContentManager locally
            this.content = content;

            //Initializes the LevelLoader
            loader = new LevelLoader();

            //Stores SoundManager locally
            soundManager = sm;

            //Initializes the game controllers
            playerControl = new PlayerController(data, soundManager);
            enemyControl = new EnemyController(soundManager);

            //Passes the list of file paths to the LevelLoader which will parse these files
            //and create a list of Level objects based on the extracted values.
            loader.loadLevels(gatherFilenames());

            objects = new List<CollisionObject>();

            //Initialize the QuadTree System
            quadTree = new QTNode(new Vector2(0, 0), new Vector2(SharedGraphicsDeviceManager.Current.PreferredBackBufferWidth, SharedGraphicsDeviceManager.Current.PreferredBackBufferHeight), null, 1, 4);

            //Particle Pools
            pools = new List<ParticlePool>();

            //Explosion list
            explosionList = new List<Texture2D>();
        }

        /// <summary>
        /// Loads in all the game's assets into memory. This will be called each time a level is loaded
        /// but will be ignored if called and a level does not need to be loaded.
        /// </summary>
        public void Load(int level)
        {
            //Pixel is checked to see if it has already been loaded into memory. If it
            //hasn't then the assets are loaded. If it has then the load calls are skipped
            if(pixel == null)
            {
                //Loads the Enemy list with Enemies stored in the particular level passed into the Load function
                enemyControl.populateEnemyList(loader.levels[level]);

                //Acts as a way of testing if textures require loading.
                pixel = content.Load<Texture2D>("Textures/Utilities/pixel");

                //Loads Pause Menu Textures
                pauseButTex = content.Load<Texture2D>("Textures/Utilities/pauseButton");
                menuTex = content.Load<Texture2D>("Textures/Utilities/MenuPanel");
                resumeTex = content.Load<Texture2D>("Textures/Utilities/ResumePanel");
                pauseBackground = content.Load<Texture2D>("Textures/Utilities/pauseBackground");
                nextTex = content.Load<Texture2D>("Textures/Utilities/NextPanel");
                restartTex = content.Load<Texture2D>("Textures/Utilities/RestartPanel");

                //Load the game font file
                font = content.Load<SpriteFont>("SpriteFont1");

                //Loads the explosion textures
                Texture2D tempexplosion1 = content.Load<Texture2D>("Textures/Explosions/explosion1");
                Texture2D tempexplosion2 = content.Load<Texture2D>("Textures/Explosions/explosion2");
                Texture2D tempexplosion3 = content.Load<Texture2D>("Textures/Explosions/explosion3");
                Texture2D tempexplosion4 = content.Load<Texture2D>("Textures/Explosions/explosion4");
                explosionList.Add(tempexplosion1);
                explosionList.Add(tempexplosion2);
                explosionList.Add(tempexplosion3);
                explosionList.Add(tempexplosion4);

                //Sends the explosion list to the controllers
                playerControl.setExplosions(explosionList);
                enemyControl.setExplosions(explosionList);

                //Loads textures for Player and Enemy objects
                playerControl.loadPlayerAssets(content);
                enemyControl.loadEnemyAssets(content);

                //Set up particle pools
                if (pools.Count == 0)
                {
                    loadPools();
                }

                //Starts game music
                soundManager.playBGM(currentlevel);
            }
        }

        /// <summary>
        /// This function will be called when a new Level is to be loaded and will null everything.
        /// The function then calls the Load function passing in level that needs to be loaded.
        /// </summary>
        private void Reset(int level)
        {
            //Nulls the controllers so they be reloaded with information on the new level
            enemyControl = null;
            playerControl = null;

            //Nulls pixel to signal that loading needs to be done.
            pixel = null;

            //Resets the controllers
            playerControl = new PlayerController(data, soundManager);
            enemyControl = new EnemyController(soundManager);

            //*** This would be a good spot to start a score screen

            //Loads the new level
            Load(level);
        }

        /// <summary>
        /// Main draw call. All of the game's draw calls will happen in this function which is the only draw call
        /// in the GamePage.xaml.cs class.
        /// </summary>
        public void drawGame(SpriteBatch sb)
        {
            //Updates FPS
            total_frames ++;

            //Draws the Enemy ships and their projectiles
            enemyControl.draw(sb);

            sb.DrawString(font, "HighScore: " + hs.scores[currentlevel], new Vector2(0, 10), Microsoft.Xna.Framework.Color.White);
            sb.DrawString(font, "Score: " + enemyControl.score, new Vector2(0, 30), Microsoft.Xna.Framework.Color.White);

            //Draws active particle pools for explosions
            foreach (ParticlePool p in pools)
            {
                if (p.isActive)
                {
                    p.draw(sb);
                }
            }

            //Draws the player's ship
            playerControl.drawPlayer(sb);

            //Draws the pause button
            sb.Draw(pauseButTex, pauseIcon, Microsoft.Xna.Framework.Color.White);
        }

        /// <summary>
        /// Main update for the game.
        /// </summary>
        public void updateGame(GameTimer timer)
        {
            //Checks for pause button touch
            if (touches.Count > 0 && touches[0].State == TouchLocationState.Pressed)
            {
                Microsoft.Xna.Framework.Point p = new Microsoft.Xna.Framework.Point((int)touches[0].Position.X, (int)touches[0].Position.Y);

                if (pauseIcon.Contains(p))
                {
                    state = GameState.Pause;
                }
            }

            if (!playerControl.player.isAlive && playerControl.player.exploded)
            {
                state = GameState.GameOver;
            }

            // Update
            elapsed_time += (float)timer.UpdateInterval.TotalMilliseconds;
 
            // 1 Second has passed
            if (elapsed_time >= 1000.0f)
            {
                fps = total_frames;
                total_frames = 0;
                elapsed_time = 0;
            }

            //Checks for collisions
            collisionDetection();

            enemyControl.update(timer);
            playerControl.updatePlayer(timer, touches);

            //Updates the particle pools
            foreach (ParticlePool p in pools)
            {
                if (p.isActive)
                {
                    p.update(timer);
                }
            }

            //Checks to see if everything with the level has been completed
            if (enemyControl.activeEnemies.Count == 0 && enemyControl.enemies.Count == 0 && currentlevel < loader.levels.Count && enemyControl.activeBosses.Count == 0 && enemyControl.bosses.Count == 0)
            {
                //If the score is greater than the highscore, save the highscore
                if (enemyControl.score > hs.scores[currentlevel])
                {
                    hs.saveNewHighscore(enemyControl.score, currentlevel);
                }

                state = GameState.Victory;
            }
        }

        /// <summary>
        /// Updates the game based on State.
        /// </summary>
        public void Update(GameTimer timer)
        {
            touches = TouchPanel.GetState();

            switch (state)
            {
                case GameState.Level:
                    {
                        updateGame(timer);
                        break;
                    }
                case GameState.Pause:
                    {
                        updatePauseScreen(timer);
                        break;
                    }
                case GameState.GameOver:
                    {
                        updateGameOverScreen(timer);
                        break;
                    }
                case GameState.Victory:
                    {
                        updateLevelCompleteScreen(timer);
                        break;
                    }
                default:
                        break;
            }
        }

        /// <summary>
        /// Draws the game based on State.
        /// </summary>
        public void Draw(SpriteBatch sb)
        {
            switch (state)
            {
                case GameState.Level:
                    {
                        drawGame(sb);
                        break;
                    }
                case GameState.Pause:
                    {
                        drawGame(sb);
                        drawPauseScreen(sb);
                        break;
                    }
                case GameState.GameOver:
                    {
                        drawGame(sb);
                        drawGameOverScreen(sb);
                        break;
                    }
                case GameState.Victory:
                    {
                        drawGame(sb);
                        drawLevelVictoryScreen(sb);
                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// This function will detect and handle any collisions that occure between the various objects in the game.
        /// </summary>
        public void collisionDetection()
        {
            //Compile the CollisionObject list
            makeCollisionObjectList();

            //Puts active CollisionObjects into a list for the QuadTree system.
            quadTree.setList(objects);

            //Run the Quadtree System
            quadTree.update();
            quadTree.dumpList();

            //Set the changed objects back into the local list.
            objects = quadTree.getList();

            //Update the objects in thier proper lists
            recreatLists();

            //Runs particle pools on collided projectiles
            makeExplosions();

            //Empty the CollisionObject List
            objects.Clear();
        }

        /// <summary>
        /// Compiles a CollisionObject List that can be used in the Quad Tree System.
        /// </summary>
        private void makeCollisionObjectList()
        {
            //Adds the Active Enemy objects to the list
            foreach (Enemy e in enemyControl.activeEnemies)
            {
                e.cType = CollisionObjectType.Enemy;
                objects.Add(e);
            }

            //Adds the Active Enemy Projectiles to the list
            foreach (Projectile p in enemyControl.activeProjectiles)
            {
                p.cType = CollisionObjectType.EnemyProjectile;
                objects.Add(p);
            }

            //Adds the Active Player to the list
            playerControl.player.cType = CollisionObjectType.Player;
            objects.Add(playerControl.player);

            //Adds the Active Player Projectiles to the list
            foreach (Projectile p in playerControl.activeProjectiles)
            {
                p.cType = CollisionObjectType.PlayerProjectile;
                objects.Add(p);
            }

            //Adds the Active Bosses to the list
            foreach (Boss b in enemyControl.activeBosses)
            {
                b.cType = CollisionObjectType.Boss;
                objects.Add(b);
            }
        }

        /// <summary>
        /// This function takes the objects out of the CollisionObject super list and puts them back into
        /// thier appropriate list.
        /// </summary>
        private void recreatLists()
        {
            //Temp lists that will be put back into the original
            List<Enemy> tempEnemies = new List<Enemy>();
            List<Projectile> tempEnemyProjectiles = new List<Projectile>();
            List<Boss> tempBosses = new List<Boss>();
            List<Projectile> tempPlayerProjectiles = new List<Projectile>();

            //Switch each CollisionObject to determine which list they need to be in
            foreach (CollisionObject c in objects)
            {
                switch (c.cType)
                {
                    case CollisionObjectType.Enemy:
                        tempEnemies.Add((Enemy)c);
                        break;
                    case CollisionObjectType.EnemyProjectile:
                        tempEnemyProjectiles.Add((Projectile)c);
                        break;
                    case CollisionObjectType.Player:
                        playerControl.player = (PlayerShip)c;
                        break;
                    case CollisionObjectType.PlayerProjectile:
                        tempPlayerProjectiles.Add((Projectile)c);
                        break;
                    case CollisionObjectType.Boss:
                        tempBosses.Add((Boss)c);
                        break;
                }
            }

            //Set the lists to the updated temp lists.
            enemyControl.activeEnemies = tempEnemies;
            enemyControl.activeProjectiles = tempEnemyProjectiles;
            enemyControl.activeBosses = tempBosses;
            playerControl.activeProjectiles = tempPlayerProjectiles;
        }

        /// <summary>
        /// Loads the particle pools into memory to be used later
        /// </summary>
        private void loadPools()
        {
            ParticlePool tempPool;

            //Creates particle pool objects and adds it the list
            for (int i = 0; i < 40; i++)
            {
                tempPool = new ParticlePool(pixel);
                pools.Add(tempPool);
            }
        }

        /// <summary>
        /// Runs particle pool objects to make graphical explosions where projectiles have collided with viable targets.
        /// </summary>
        private void makeExplosions()
        {
            foreach (Projectile p in enemyControl.activeProjectiles)
            {
                if (p.hasCollided)
                {
                    //Plays the explosion sound
                    soundManager.playSound(1);

                    foreach (ParticlePool pp in pools)
                    {
                        if (!pp.isActive)
                        {
                            pp.isActive = true;
                            pp.setOrigin(p.hitBox.Location);
                            p.remove = true;
                            break;
                        }
                    }
                }
            }

            //TODO comment
            foreach (Projectile p in playerControl.activeProjectiles)
            {
                if (p.hasCollided)
                {
                    //Plays the explosion sound
                    soundManager.playSound(1);

                    foreach (ParticlePool pp in pools)
                    {
                        if (!pp.isActive)
                        {
                            pp.isActive = true;
                            p.remove = true;
                            pp.setOrigin(p.hitBox.Location);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Function will put hardcoded String paths into a list to use for level loading.
        /// </summary>
        private List<String> gatherFilenames()
        {
            //Temporary List of file paths
            List<String> files = new List<String>();

            //Paths
            String level1 = "Levels//Level1.xml";
            String level2 = "Levels//Level2.xml";
            String level3 = "Levels//Level3.xml";

            //Adds paths to list
            files.Add(level1);
            files.Add(level2);
            files.Add(level3);

            //Returns the list which will be passed as a parameter for the LevelLoader's load function
            return files;
        }

        /// <summary>
        /// Initializes the Pause Screen variables
        /// </summary>
        private void initPauseScreen()
        {
            //Rectangle that fades the screen out slightly to but focus on the pause screen
            fadeout = new Rectangle(0, 0, 800, 800);

            //Rectangle that makes up the background of the pause window
            pausescreen = new Rectangle((SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width / 4) - 25, 
                (SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height / 5),
                (SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width / 2) + 50,
                (SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height / 2));

            //Rectangles for menu buttons
            menuButton = new Rectangle((pausescreen.X + 10), (pausescreen.Y + (pausescreen.Height - 90)), pausescreen.Width - 20, 80);
            resumeButton = new Rectangle((pausescreen.X + 10), (pausescreen.Y + (pausescreen.Height - 180)), pausescreen.Width - 20, 80);

            //Pause icon that will be displayed in the top corner of the screen
            pauseIcon = new Rectangle((SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width - 50), 0, 50, 50);

            scoreString = "Score: ";
        }

        /// <summary>
        /// Draws the pause screen when needed
        /// </summary>
        private void drawPauseScreen(SpriteBatch sb)
        {
            //Draws the fadeout over the current screen
            sb.Draw(pixel, fadeout, new Microsoft.Xna.Framework.Color(0,0,0, 200));
            
            //Draws the Pause Window
            sb.Draw(pauseBackground, pausescreen, Microsoft.Xna.Framework.Color.White);

            //Draws the Buttons on the Pause Window
            sb.Draw(menuTex, menuButton, Microsoft.Xna.Framework.Color.White);
            sb.Draw(resumeTex, resumeButton, Microsoft.Xna.Framework.Color.White);

            //Draws the Score
            sb.DrawString(font, scoreString + enemyControl.score, new Vector2(pausescreen.X + 10, pausescreen.Y + 70), Microsoft.Xna.Framework.Color.White);

            //Draws the Title string
            sb.DrawString(font, "Game Paused", new Vector2(pausescreen.X + 10, pausescreen.Y + 10), Microsoft.Xna.Framework.Color.White);
        }

        /// <summary>
        /// Draws the Game Over Screen when needed
        /// </summary>
        private void drawGameOverScreen(SpriteBatch sb)
        {
            //Draws the fadeout over the current screen
            sb.Draw(pixel, fadeout, new Microsoft.Xna.Framework.Color(0, 0, 0, 200));

            //Draws the Pause Window
            sb.Draw(pauseBackground, pausescreen, Microsoft.Xna.Framework.Color.White);

            //Draws the Buttons on the Pause Window
            sb.Draw(menuTex, menuButton, Microsoft.Xna.Framework.Color.White);
            sb.Draw(restartTex, resumeButton, Microsoft.Xna.Framework.Color.White);

            //Draws the Score
            sb.DrawString(font, scoreString + enemyControl.score, new Vector2(pausescreen.X + 10, pausescreen.Y + 70), Microsoft.Xna.Framework.Color.White);

            //Draws the Title string
            sb.DrawString(font, "Game Over", new Vector2(pausescreen.X + 10, pausescreen.Y + 10), Microsoft.Xna.Framework.Color.White);
        }

        /// <summary>
        /// Draws the Game Over Screen when needed
        /// </summary>
        private void drawLevelVictoryScreen(SpriteBatch sb)
        {
            //Draws the fadeout over the current screen
            sb.Draw(pixel, fadeout, new Microsoft.Xna.Framework.Color(0, 0, 0, 200));

            //Draws the Pause Window
            sb.Draw(pauseBackground, pausescreen, Microsoft.Xna.Framework.Color.White);

            //Draws the Buttons on the Pause Window
            sb.Draw(menuTex, menuButton, Microsoft.Xna.Framework.Color.White);

            if (!((currentlevel) >= loader.levels.Count - 1))
            {
                sb.Draw(nextTex, resumeButton, Microsoft.Xna.Framework.Color.White);
            }

            //Draws the Score
            sb.DrawString(font, scoreString + enemyControl.score, new Vector2(pausescreen.X + 10, pausescreen.Y + 70), Microsoft.Xna.Framework.Color.White);

            //Draws the Title string
            sb.DrawString(font, "Level Complete", new Vector2(pausescreen.X + 10, pausescreen.Y + 10), Microsoft.Xna.Framework.Color.White);
        }

        /// <summary>
        /// Updates the pause screen mostly checking for user input
        /// </summary>
        private void updatePauseScreen(GameTimer timer)
        {
            //Checks for pause button touch
            if (touches.Count > 0 && touches[0].State == TouchLocationState.Pressed)
            {
                Microsoft.Xna.Framework.Point p = new Microsoft.Xna.Framework.Point((int)touches[0].Position.X, (int)touches[0].Position.Y);

                if (menuButton.Contains(p))
                {
                    //Sets a flag to tell GamePage to Navigate to the Main Menu
                    mainMenu = true;
                }

                if (resumeButton.Contains(p))
                {
                    //Resume the game
                    state = GameState.Level;
                }
            }
        }

        /// <summary>
        /// Updates the pause screen mostly checking for user input
        /// </summary>
        private void updateGameOverScreen(GameTimer timer)
        {
            //Checks for touch
            if (touches.Count > 0 && touches[0].State == TouchLocationState.Pressed)
            {
                Microsoft.Xna.Framework.Point p = new Microsoft.Xna.Framework.Point((int)touches[0].Position.X, (int)touches[0].Position.Y);

                if (menuButton.Contains(p))
                {
                    //Sets a flag to tell GamePage to Navigate to the Main Menu
                    mainMenu = true;
                }

                if (resumeButton.Contains(p))
                {
                    //Restart the game
                    Reset(currentlevel);
                    state = GameState.Level;
                }
            }
        }

        /// <summary>
        /// Updates the pause screen mostly checking for user input
        /// </summary>
        private void updateLevelCompleteScreen(GameTimer timer)
        {
            //Checks for touch
            if (touches.Count > 0 && touches[0].State == TouchLocationState.Pressed)
            {
                Microsoft.Xna.Framework.Point p = new Microsoft.Xna.Framework.Point((int)touches[0].Position.X, (int)touches[0].Position.Y);

                if (menuButton.Contains(p))
                {
                    //Sets a flag to tell GamePage to Navigate to the Main Menu
                    mainMenu = true;
                }

                if (resumeButton.Contains(p))
                {
                    //Starts the next level if there is a level
                    if (!((currentlevel) >= loader.levels.Count - 1))
                    {
                        currentlevel++;
                        Reset(currentlevel);
                        state = GameState.Level;
                    }
                    else //Sends the user back to the main page
                    {
                        //Sets a flag to tell GamePage to Navigate to the Main Menu
                        mainMenu = true;
                    }
                }
            }
        }
    }
}
