using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chroma.GameClasses.Utilities.Particle_System
{
    /// <summary>
    /// Small class that groups necessary variables for each particle making less calculations required in the ParticlePool class.
    /// </summary>
    class Particle
    {
        //1x1 pixel that will represent the particle
        public Rectangle box;

        //How long the particle will last 
        public double lifeTime;

        //Helps track how much time is left for the particle
        public double lifeTimer;

        //Direction the particle moves in
        public Vector2 speed;

        //Position of the particle
        public Vector2 position;

        //Stores the pixel texture for the particle
        public Texture2D texture;

        //Boolean to determine if the object should be drawn
        public bool active = true;

        /// <summary>
        /// Sets default variables
        /// </summary>
        public Particle(Texture2D texture)
        {
            lifeTime = 0.0f;
            lifeTimer = 0;
            speed = Vector2.Zero;
            position = Vector2.Zero;
            this.texture = texture;

            //Default position and dimensions
            box = new Rectangle(0, 0, 1, 1);
        }

        /// <summary>
        /// Updates the particle based on the random information passed in
        /// </summary>
        /// <param name="timer"></param>
        public void update(GameTimer timer)
        {
            position.X += speed.X * (float)timer.UpdateInterval.TotalSeconds;
            position.Y += speed.Y * (float)timer.UpdateInterval.TotalSeconds;

            box = new Rectangle((int)position.X, (int)position.Y, 1, 1);

            lifeTimer += timer.UpdateInterval.TotalSeconds;

            //If the timer has run out, stop updating
            if (lifeTimer > lifeTime)
            {
                active = false;
                lifeTimer = 0;
            }
        }

        /// <summary>
        /// Draw method for the particle
        /// </summary>
        /// <param name="sb"></param>
        public void draw(SpriteBatch sb)
        {
            sb.Draw(texture, box, Color.Orange);
        }
    }
}
