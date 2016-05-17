using Chroma.GameClasses.Utilities;
using Chroma.GameClasses.Utilities.FrontendBackend;
using Chroma.GameClasses.Utilities.Screen_Input;
using Chroma.GameClasses.Utilities.Shields;
using Chroma.GameClasses.Utilities.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chroma.GameClasses.PlayerSystem
{
    /// <summary>
    /// This class will essentially be the Player Controlled Ship. The class will
    /// store information about the player's ship sub-classes such as Shields and 
    /// Weapons. The class will also track it's position in the game world, and will
    /// contain a draw call that will be called in later classes.
    /// </summary>
    public class PlayerShip : CollisionObject
    {

        //Lists of Sub-Systems that the player has access to.
        public List<Shield> shields;
        public List<Weapon> weapons;

        //Flag to determine if the player is alive or not
        public bool isAlive = true;
        public bool exploded = false;

        //Physical Variables
        public Vector2 position;
        Vector2 speed;
        Vector2 dimensions;

        //Aesthetic Variables
        public List<Texture2D> textures;
        public List<Texture2D> explosionList;
        public int currentTexture = 0;
        public int animationCounter = 1;
        private int animationAdder = 1;

        //Player Statistical Variables
        public float maxHitPoints;
        public float hitPoints;
        float fireRate;

        /// <summary>
        /// Default constructor for the PlayerShip class.
        /// </summary>
        public PlayerShip(DataPackage data)
        {
            //Declares and initializes Physical Variables
            dimensions = new Vector2(40, 45);
            position = new Vector2((SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width / 2) - (dimensions.X / 2),
                (SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height / 2) + 100);
            hitBox = new Rectangle((int)position.X, (int)position.Y, 40, 45);
            speed = new Vector2(300, -300);

            //Default amount of hitpoints
            maxHitPoints = 1000;
            hitPoints = 1000;

            //Default fire rate
            fireRate = 0.4f;

            weapons = new List<Weapon>();
            shields = new List<Shield>();

            //Starting weapon for the player
            Weapon startingWeapon = new PulseCannon(fireRate);
            startingWeapon.setProjectileColor(data.projectile1);
            weapons.Add(startingWeapon);

            //Starting shield for the player
            Shield startingShield = new BasicShield(position, dimensions);

            //Sets the default color of the shield
            startingShield.setColor(data.shield1);
            color = data.shield1;
            shields.Add(startingShield);
        }

        /// <summary>
        /// Loads the texture in from the controller
        /// </summary>
        public void loadTexture(List<Texture2D> playerTextures, Texture2D proj, Texture2D shield)
        {
            textures = playerTextures;

            weapons[0].setProjectileTexture(proj);
            shields[0].setTexture(shield);
        }

        /// <summary>
        /// Draw function for the PlayerShip sprite
        /// </summary>
        public void draw(SpriteBatch sb)
        {
            if (isAlive)
            {
                //Calls the current shield's draw function
                shields[0].draw(sb);
            }

            //Draws the ship's sprite
            sb.Draw(textures[currentTexture], hitBox, Color.White);
        }

        /// <summary>
        /// Update function for the PlayerShip sprite
        /// </summary>
        public void update(GameTimer timer, Joystick pad)
        {
            if (isAlive)
            {
                //Updates the player's position based on the values
                position.Y += speed.Y * pad.stick.Y * (float)timer.UpdateInterval.TotalSeconds;
                position.X += speed.X * pad.stick.X * (float)timer.UpdateInterval.TotalSeconds;

                //Keeps the ship in the bounds of the screen
                if (position.X < 0)
                {
                    position.X = 0;
                }
                else if (position.X > SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width - dimensions.X)
                {
                    position.X = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width - dimensions.X;
                }

                if (position.Y < 0)
                {
                    position.Y = 0;
                }
                else if (position.Y > SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height - 200 - dimensions.Y)
                {
                    position.Y = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height - 200 - dimensions.Y;
                }

                //Updates the position of the ship
                hitBox = new Rectangle((int)position.X, (int)position.Y, 40, 45);

                //Updates the weapon systems to allow for shooting
                weapons[0].update(timer, new Vector2(0, -800), position + (new Vector2((dimensions.X / 2), 0)));

                //Updates the current shield
                shields[0].update(timer, position);

            }
            //Animates the ship
            animation(pad);
        }

        /// <summary>
        /// Function that will run through a series of sprites to animate the player ship
        /// </summary>
        private void animation(Joystick pad)
        {
            if (isAlive)
            {
                if (pad.stick.X == 0) //If the ship flies straight
                {
                    //Changes the base texture in case the texture is not within the texture range for this animation set.
                    if (currentTexture > 1)
                    {
                        currentTexture = 0;
                    }

                    //Checks to see if the texture in the animation should be changed
                    if (animationCounter % 10 == 0)
                    {
                        if (currentTexture == 1)
                        {
                            animationAdder = -1;
                            currentTexture += animationAdder;
                        }
                        else if (currentTexture == 0)
                        {
                            animationAdder = 1;
                            currentTexture += animationAdder;
                        }
                    }
                }
                else if (pad.stick.X > 0) //If the ship flies to the right
                {
                    //Changes the base texture in case the texture is not within the texture range for this animation set.
                    if (currentTexture < 2 || currentTexture > 3)
                    {
                        currentTexture = 2;
                    }

                    //Checks to see if the texture in the animation should be changed
                    if (animationCounter % 10 == 0)
                    {
                        if (currentTexture == 3)
                        {
                            animationAdder = -1;
                            currentTexture += animationAdder;
                        }
                        else if (currentTexture == 2)
                        {
                            animationAdder = 1;
                            currentTexture += animationAdder;
                        }
                    }
                }
                else if (pad.stick.X < 0) //If the ship flies to the Left
                {
                    //Changes the base texture in case the texture is not within the texture range for this animation set.
                    if (currentTexture < 4)
                    {
                        currentTexture = 4;
                    }

                    //Checks to see if the texture in the animation should be changed
                    if (animationCounter % 10 == 0)
                    {
                        if (currentTexture == 5)
                        {
                            animationAdder = -1;
                            currentTexture += animationAdder;
                        }
                        else if (currentTexture == 4)
                        {
                            animationAdder = 1;
                            currentTexture += animationAdder;
                        }
                    }
                }

            }
            else
            {
                //Runs explosion animation
                if ((animationCounter % 5 == 0))
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
            }

            //Increment counter
            animationCounter++;
        }
    }
}
