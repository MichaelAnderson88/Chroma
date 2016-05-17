using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chroma.GameClasses.Utilities.Particle_System
{
    /// <summary>
    /// This object will be a small object pool of particles that will be used for graphical effects in the game. If this pool is active,
    /// the particles associated with it will be drawn at the location specified by what needs it for a very short period of time after
    /// which the object will be set to inactive and will wait until it is needed again.
    /// </summary>
    class ParticlePool
    {

        //List that will store the particles.
        List<Particle> particles;

        //Flag that will determine if this particular instance is in use.
        public bool isActive = false;

        //Random number generator
        Random r;
        
        //Counter to know when the pool should be deactivated
        int counter;

        /// <summary>
        /// Default constructor for the ParticlePool object. Will create and store the particles for use later.
        /// </summary>
        public ParticlePool(Texture2D text)
        {
            //Initializes the Random object
            r = new Random(3515154);

            //Initializes the list
            particles = new List<Particle>();

            //Fills the particle list
            for (int i = 0; i < 20; i++)
            {
                //Temporary Particle object with default values, position values will be updated when the pool is activated.
                Particle temp = new Particle(text);

                //Adds to the list.
                particles.Add(temp);
            }

            //Randomizes the particles
            getRandomization();
        }

        /// <summary>
        /// Sets the origin of the particle burst.
        /// </summary>
        public void setOrigin(Point origin)
        {
            foreach (Particle p in particles)
            {
                p.position = new Vector2(origin.X, origin.Y);
            }
        }

        /// <summary>
        /// Update method, will move particles based on values set before hand.
        /// </summary>
        public void update(GameTimer timer)
        {
            counter = 0;

            //Only update the particles if the pool is still active
            foreach (Particle p in particles)
            {
                //Check to see if the particle is still active
                if (p.active)
                {
                    //Update the particle
                    p.update(timer);

                    //Increment the temp counter
                    counter++;
                }
            }

            //If no particles were updated, the pool has run to completion so deactive the pool 
            if (counter == 0)
            {
                isActive = false;
            }
            
            if(!isActive)
            {
                //Reset the particles so the pool is ready for the next use
                reset();
            }
        }

        /// <summary>
        /// Draws the particles in this pool
        /// </summary>
        public void draw(SpriteBatch sb)
        {
            foreach (Particle p in particles)
            {
                //Checks to see if the particle should still be drawn
                if (p.active)
                {
                    p.draw(sb);
                }
            }
        }

        /// <summary>
        /// This is a helper function that will change and randomize a Vector that a particle will use to translate.
        /// It will also create a random lifetime for the particle.
        /// </summary>
        /// <returns></returns>
        private void getRandomization()
        {
            //Iterates through particle list and sets randomized information
            foreach (Particle p in particles)
            {
                p.lifeTime = 0.5 + (2 * r.NextDouble());

                p.speed = new Vector2(r.Next(-20, 20), r.Next(-20, 20));
            }
        }

        /// <summary>
        /// Starts the object pool. Sets the active flag so that this pool is updated. Also sets up the particles associated with
        /// this pool.
        /// </summary>
        public void startPool()
        {
            getRandomization();
            isActive = true;
        }

        /// <summary>
        /// Resets the particle pool
        /// </summary>
        private void reset()
        {
            //Resets the particle pool
            foreach (Particle p in particles)
            {
                p.active = true;
            }
        }
    }
}
