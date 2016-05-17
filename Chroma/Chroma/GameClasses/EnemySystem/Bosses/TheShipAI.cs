using Chroma.GameClasses.Utilities.Weapons;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chroma.GameClasses.EnemySystem.Bosses
{
    /// <summary>
    /// AI associated with the Boss named The Ship. The main focus of this class is to define the state
    /// functions to make the boss unique to others.
    /// </summary>
    class TheShipAI : AI
    {

        //Counter to time for transition between states
        private int counter = 0;

        //Counter to deal with Animation transition
        private int animationCounter = 0;
        private int animationAdder = 1;

        //Flags for Special State
        private bool Xoff = false;
        private bool Yoff = false;
        private bool super = false;

        //BaseState point coordinates
        Vector2 p1, p2, p3, p4;

        //Critical state new weapon
        LaserCannon laser;

        /// <summary>
        /// The base state for this boss will rotate through a basic movement pattern and using the 
        /// boss' base weapons.
        /// </summary>
        override public BossData baseState(BossData data, GameTimer timer)
        {
            p1 = new Vector2((SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width * 0.9f) - data.dimensions.X,
                (SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height * 0.1f));
            p2 = new Vector2((SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width * 0.9f) - data.dimensions.X,
                (SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height * 0.5f) - 100 - data.dimensions.Y);
            p3 = new Vector2((SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width * 0.1f),
                (SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height * 0.5f) - 100 - data.dimensions.Y);
            p4 = new Vector2((SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width * 0.1f),
                (SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height * 0.1f));

            //Basic Square movement pattern between 100,100 and 200,200
            if (data.position.X < p1.X && data.position.Y < p1.Y)
            {
                data.speed.Y = 0.0f;
                data.speed.X += 10 * (float)timer.UpdateInterval.TotalSeconds;
            }
            else if (data.position.X > p2.X && data.position.Y < p2.Y)
            {
                data.speed.X = 0.0f;
                data.speed.Y += 10 * (float)timer.UpdateInterval.TotalSeconds;
            }
            else if (data.position.X > p3.X && data.position.Y > p3.Y)
            {
                data.speed.Y = 0.0f;
                data.speed.X -= 10 * (float)timer.UpdateInterval.TotalSeconds;
            }
            else if (data.position.X < p4.X && data.position.Y > p4.Y)
            {
                data.speed.X = 0.0f;
                data.speed.Y -= 10 * (float)timer.UpdateInterval.TotalSeconds;
            }

            //Checks the timer to see if its time to transition out of the BaseState
            if((counter % 1000 == 0) && counter != 0)
            {
                //Resets the speed of the boss
                data.speed.X = 0.0f;
                data.speed.Y = 0.0f;

                //Resets the timer
                counter = 0;

                //Changes the state to special
                currentState = BossState.Special;
            }

            //Increments the counter that determines when the boss goes into Special State
            counter++;

            //If the boss has low hp, enter critical state
            if (data.hitpoints < 1500)
            {
                currentState = BossState.Critical;

                //Dramatically increase firerate
                data.fireRate = 0.10f;

                //Add a laser weapon
                laser = new LaserCannon(0.5f, new Vector2(0, 40));
                data.weapons.Add(laser);
                laser.setProjectileTexture(data.laserTex);

                //Change Shield Color
                data.shields[0].color = Color.Violet;
            }

            return data;
        }

        /// <summary>
        /// This state occurs every so often during the Boss fight determined by an arbitrary number in the Base state.
        /// This state will make the boss start heading towards a particular position on the map, and when it reaches that
        /// position, it quickly moves across the screen firing it's weapons much quicker. After this strafe, the boss then resets
        /// to it's default position and resumes Base state patterns.
        /// </summary>
        override public BossData specialState(BossData data, GameTimer timer)
        {
            //If the super flag has not been set meaning the ship is not in position
            if (!super)
            {
                //Checks to see if the texture in the animation should be changed
                if (animationCounter % 5 == 0)
                {
                    if (data.currentTexture < 5)
                    {
                        data.currentTexture++;
                    }
                }

                //Increment Animation counter
                animationCounter++;

                //As long as the ship isnt in position update the X and Y until they reach the specified limit
                if (data.position.Y < p2.Y || data.position.X < p2.X)
                {
                    //If X position has not been reached
                    if (!Xoff)
                    {
                        data.speed.X += 10 * (float)timer.UpdateInterval.TotalSeconds;
                    }
                    //If Y position has not been reached
                    if (!Yoff)
                    {
                        data.speed.Y += 10 * (float)timer.UpdateInterval.TotalSeconds;
                    }
                }

                //If the Y position has been reached, set the flag and turn off speed in the Y axis
                if (data.position.Y > p2.Y)
                {
                    Yoff = true;
                    data.speed.Y = 0.0f;
                }

                //If the X position has been reached, set the flag and turn off speed in the X axis
                if (data.position.X > p2.X)
                {
                    Xoff = true;
                    data.speed.X = 0.0f;
                }

                //If both the X and Y positions have been reached, set the flag to start super mode.
                if (Xoff && Yoff)
                {
                    super = true;
                }
            }

            //If super mode has been engaged and has not reached the X destination
            if (super && data.position.X > p3.X)
            {
                //Dramatically increase fire rate
                data.fireRate = 0.10f;

                //Set a quick speed in the X axis
                data.speed.X = -100.0f;
            }

            //If super mode has been engaged and the X destination has been reached
            if (super && data.position.X < p3.X)
            {
                //Reset the fire rate
                data.fireRate = 0.50f;

                //Turn off the X speed
                data.speed.X = 0.0f;

                //Start moving towards the Default position for Base state
                data.speed.Y = -100.0f;

                //Start animation in reverse direction. If statement ensures it happens only once
                if (animationAdder == 1)
                {
                    //Starts decrementing the animation index
                    data.currentTexture -= 1;
                    animationCounter = 0;
                    animationAdder = -1;
                }

                //Checks to see if the texture in the animation should be changed
                if (animationCounter % 5 == 0)
                {
                    if (data.currentTexture > 0)
                    {
                        data.currentTexture += animationAdder;
                    }
                }
            }

            //If super mode has been engaged and the Boss has reached the default position
            if (super && data.position.X < p4.X && data.position.Y < p4.Y)
            {
                //Reset variables
                data.speed.Y = 0.0f;

                Yoff = false;
                Xoff = false;
                super = false;

                //Reset animation variables
                animationAdder = 1;
                animationCounter = 0;

                //Change the state back to Base state.
                currentState = BossState.Base;
            }

            //Increment Animation counter
            animationCounter++;

            return data;
        }

        /// <summary>
        /// This state will be engaged when the Boss is about to die. It will increase the movement speed and fire rate of the boss
        /// create an overall increase to the difficulty of finishing off the boss.
        /// </summary>
        override public BossData criticalState(BossData data, GameTimer timer)
        {
            //Updates new weapon
            laser.update(timer, new Vector2(0, 810), (data.position + new Vector2(data.dimensions.X / 2, data.dimensions.Y)));

            //Checks to see if the texture in the animation should be changed
            if (animationCounter % 3 == 0)
            {
                if (data.currentTexture < 5)
                {
                    data.currentTexture++;
                }
            }

            //Increment Animation counter
            animationCounter++;

            p1 = new Vector2((SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width * 0.9f) - data.dimensions.X,
                (SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height * 0.1f));
            p2 = new Vector2((SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width * 0.9f) - data.dimensions.X,
                (SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height * 0.5f) - 100 - data.dimensions.Y);
            p3 = new Vector2((SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width * 0.1f),
                (SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height * 0.5f) - 100 - data.dimensions.Y);
            p4 = new Vector2((SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width * 0.1f),
                (SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height * 0.1f));

            //Basic Square movement pattern between 100,100 and 200,200
            if (data.position.X < p1.X && data.position.Y < p1.Y)
            {
                data.speed.Y = 0.0f;
                data.speed.X += 50 * (float)timer.UpdateInterval.TotalSeconds;
            }
            else if (data.position.X > p2.X && data.position.Y < p2.Y)
            {
                data.speed.X = 0.0f;
                data.speed.Y += 50 * (float)timer.UpdateInterval.TotalSeconds;
            }
            else if (data.position.X > p3.X && data.position.Y > p3.Y)
            {
                data.speed.Y = 0.0f;
                data.speed.X -= 50 * (float)timer.UpdateInterval.TotalSeconds;
            }
            else if (data.position.X < p4.X && data.position.Y > p4.Y)
            {
                data.speed.X = 0.0f;
                data.speed.Y -= 50 * (float)timer.UpdateInterval.TotalSeconds;
            }

            return data;
        }
    }
}
