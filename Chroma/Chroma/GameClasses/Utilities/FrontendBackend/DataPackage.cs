using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chroma.GameClasses.Utilities.FrontendBackend
{
    /// <summary>
    /// This class will store data that is collected from the front end portion of the application and send it
    /// with the URI Navigation function to the backend for processing.
    /// </summary>
    public class DataPackage
    {
        //Color objects for the Shield and Projectile colors
        public Color shield1;
        public Color shield2;
        public Color projectile1;
        public Color projectile2;

        //Starting level
        public int level = 0;

        //Sound Mute Flag
        public bool mute = false;

        /// <summary>
        /// Constructor for the object. Will initialize lists for use later.
        /// </summary>
        public DataPackage()
        {
            shield1 = Color.White;
            shield2 = Color.White;
            projectile1 = Color.White;
            projectile2 = Color.White;
        }

        /// <summary>
        /// Sets the level the game will start on
        /// </summary>
        public void setStartLevel(int level)
        {
            this.level = level;
        }

        /// <summary>
        /// Sets the flag for if sound should be muted or not.
        /// </summary>
        /// <param name="change"></param>
        public void setMute(bool change)
        {
            mute = change;
        }
    }
}
