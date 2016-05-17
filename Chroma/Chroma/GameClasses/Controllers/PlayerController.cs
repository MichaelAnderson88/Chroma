using Chroma.GameClasses.PlayerSystem;
using Chroma.GameClasses.Utilities;
using Chroma.GameClasses.Utilities.FrontendBackend;
using Chroma.GameClasses.Utilities.Projectiles;
using Chroma.GameClasses.Utilities.Screen_Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
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

namespace Chroma.GameClasses.Controllers
{
    /// <summary>
    /// Class that will handle everything to do with the Player's ship. User input to move the ship and fire weapons will be
    /// handled here.
    /// </summary>
    public class PlayerController
    {
        //Playership object that will contain 
        public PlayerShip player;

        //Game Joystick interface for movement controls
        public Joystick js;

        //User interface
        public UserInterface ui;

        //List of projectiles fired by the player
        public List<Projectile> activeProjectiles;

        //Local SoundManager Object
        public SoundManager soundManager;

        //Health bar variables
        Texture2D pixel;
        Texture2D hbText;
        Rectangle health;
        Rectangle healthBar;

        public List<Texture2D> explosionList;

        /// <summary>
        /// Default constructor for the PlayerController class. Initializes a new instance of the PlayerShip class that will act as the
        /// player's controllable ship.
        /// </summary>
        public PlayerController(DataPackage data, SoundManager sm)
        {
            //Sets local SoundManager
            soundManager = sm;

            //Player's controllable ship
            player = new PlayerShip(data);

            //User interface
            ui = new UserInterface();

            //Sets up Health bar Rectangles.
            healthBar = new Rectangle(ui.overlayRect.X + 140, ui.overlayRect.Y + 20, 200, 10);
            health = healthBar;

            explosionList = new List<Texture2D>();
            activeProjectiles = new List<Projectile>();
        }

        /// <summary>
        /// Calls the player and the dpad draw functions
        /// </summary>
        public void drawPlayer(SpriteBatch sb)
        {
            player.draw(sb);
            ui.draw(sb);
            js.Draw(sb);

            //Draw health bar
            sb.Draw(pixel, healthBar, Microsoft.Xna.Framework.Color.Black);
            sb.Draw(pixel, health, Microsoft.Xna.Framework.Color.Red);
            sb.Draw(hbText, healthBar, Microsoft.Xna.Framework.Color.White);

            //Draws Projectiles
            foreach (Projectile p in activeProjectiles)
            {
                p.draw(sb);
            }
        }

        /// <summary>
        /// Loads textures associated with the player
        /// </summary>
        public void loadPlayerAssets(ContentManager content)
        {
            //Stores pixel to be sent to various locations
            pixel = content.Load<Texture2D>("Textures/Utilities/pixel");

            //Player Textures Loaded and added to the list
            List<Texture2D> textures = new List<Texture2D>();
            Texture2D mid1 = content.Load<Texture2D>("Textures/PlayerShip/PlayerMid1");
            Texture2D mid2 = content.Load<Texture2D>("Textures/PlayerShip/PlayerMid2");
            Texture2D right1 = content.Load<Texture2D>("Textures/PlayerShip/PlayerRight1");
            Texture2D right2 = content.Load<Texture2D>("Textures/PlayerShip/PlayerRight2");
            Texture2D left1 = content.Load<Texture2D>("Textures/PlayerShip/PlayerLeft1");
            Texture2D left2 = content.Load<Texture2D>("Textures/PlayerShip/PlayerLeft2");

            //Loads shield texture
            Texture2D shield = content.Load<Texture2D>("Textures/Utilities/Shield");

            //Loads Projectile Texture
            Texture2D pulse = content.Load<Texture2D>("Textures/Utilities/PlayerPulse");

            //Loads Healthbar Texture
            hbText = content.Load<Texture2D>("Textures/Utilities/HealthBar");

            //Loads spritefont
            SpriteFont tempFont = content.Load<SpriteFont>("SpriteFont1");

            textures.Add(mid1);
            textures.Add(mid2);
            textures.Add(right1);
            textures.Add(right2);
            textures.Add(left1);
            textures.Add(left2);

            player.explosionList = this.explosionList;

            //Sends the list of textures to the player object
            player.loadTexture(textures, pulse, shield);

            //Loads interface components
            js = new Joystick(content.Load<Texture2D>("Textures/UserControls/ThumbBase"), content.Load<Texture2D>("Textures/UserControls/ThumbStick"));
            ui.load(content.Load<Texture2D>("Textures/UserControls/InterfaceOverlay"), content.Load<Texture2D>("Textures/UserControls/Button"), tempFont); 
        }

        /// <summary>
        /// Updates the dpad and updates the player's position based on information collected 
        /// from the dpad
        /// </summary>
        public void updatePlayer(GameTimer timer, TouchCollection touches)
        {
            //Checks to see if the player has collided with an Enemy projectile
            if (player.hasCollided)
            {
                int red = 0;
                int green = 0;
                int blue = 0;

                //Compute damage taken from collision based on color of projectile
                if ((player.color.R - player.collisionColor.R) < 0)
                    red = (player.color.R - player.collisionColor.R) * -1;

                if ((player.color.G - player.collisionColor.G) < 0)
                    green = (player.color.G - player.collisionColor.G) * -1;

                if ((player.color.B - player.collisionColor.B) < 0)
                    blue = (player.color.B - player.collisionColor.B) * -1;

                int damageTaken = red + green + blue;

                player.hitPoints -= damageTaken;

                health = new Rectangle(healthBar.X, healthBar.Y, (int)((player.hitPoints / player.maxHitPoints) * 200), 10);
                player.hasCollided = false;

                //If the player's hitpoints is less than 0, the player is dead
                if (player.hitPoints <= 0)
                {
                    player.isAlive = false;
                    player.textures = player.explosionList;
                    player.currentTexture = 0;
                    player.animationCounter = 0;
                    player.hitBox.Width = 50;
                    player.hitBox.Height = 50;
                    player.collidable = false;
                }
            }

            if (player.isAlive || !player.exploded)
            {

                //Checks to see if the Player object has fired any projectiles
                if (player.weapons[0].projectiles.Count > 0)
                {
                    soundManager.playSound(2);
                    //if it has, add to activeProjectile List
                    foreach (Projectile p in player.weapons[0].projectiles)
                    {
                        activeProjectiles.Add(p);
                    }

                    player.weapons[0].projectiles.Clear();
                }

                //Once projectiles have been handled, update the game paddle, then the player 
                ui.update(player, touches);
                js.Update(touches);
                player.update(timer, js);
            }

            //Updates the projectiles
            foreach (Projectile p in activeProjectiles)
            {
                p.update(timer);
            }

            //Check to see if projectiles need to be removed
            checkProjectileRemoval();
        }

        /// <summary>
        /// Checks to see if any Projectile objects in the active List are flagged for removal.
        /// </summary>
        public void checkProjectileRemoval()
        {
            //Initializes a new removal list
            List<Projectile> removalList = new List<Projectile>();

            //Checks all projectiles in the Projectile List for the removal flag
            foreach (Projectile p in activeProjectiles)
            {
                if (p.remove)
                {
                    //If found, add to removalList
                    removalList.Add(p);
                }
            }

            //Remove all projectiles that were put in removalList
            if (removalList.Count > 0)
            {
                foreach (Projectile p in removalList)
                {
                    activeProjectiles.Remove(p);
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
