using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Chroma.GameClasses.Utilities
{
    /// <summary>
    /// This class will manage sound effects and music in the game. Song objects will be used to play the Background music for the game while
    /// SoundEffect objects will be used for smaller sounds to be played at random parts during gameplay. Song objects run MP3 file formats 
    /// where SoundEffect run WAV files.
    /// </summary>
    public class SoundManager
    {
        
        //Sound effect objects. These are named according to the sound they should be making.
        SoundEffect cannon, pulse;

        //Song objects. Will be used as background music for the game. Named according to the track more than it's use.
        Song dranik, inktuta, riftseekers;

        //Master volume for sound effects. Used for muting
        float masterVolume = 1.0f;

        public SoundManager(ContentManager c)
        {
            loadSE(c);
            loadBGM(c);

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.3f;

            App app = Application.Current as App;
            if (app.data.mute)
            {
                MediaPlayer.Volume = 0.0f;
                masterVolume = 0.0f;
            }
        }

        /// <summary>
        /// Loads SoundEffect WAV files from the content pipeline
        /// </summary>
        private void loadSE(ContentManager c)
        {
            cannon = c.Load<SoundEffect>("Sounds/Blast1");
            pulse = c.Load<SoundEffect>("Sounds/Pulse");
        }

        /// <summary>
        /// Loads Song MP3 files from the content pipeline
        /// </summary>
        private void loadBGM(ContentManager c)
        {
            dranik = c.Load<Song>("Music/dranik");
            inktuta = c.Load<Song>("Music/inktuta");
            riftseekers = c.Load<Song>("Music/riftseekers");
        }

        /// <summary>
        /// Function that is called to play a repeated background music for the game.
        /// </summary>
        /// <param name="track">flag that will be used in a switch to determine which track to play</param>
        public void playBGM(int track)
        {
            //Switch statement that determines which track to play
            switch (track)
            {
                case 0:
                    MediaPlayer.Play(dranik);
                    break;
                case 1:
                    MediaPlayer.Play(inktuta);
                    break;
                case 2:
                    MediaPlayer.Play(riftseekers);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Function that will be called whenever a sound effect is to be played for the game. A flag is passed in to indicate 
        /// which sound effect should be played.
        /// </summary>
        /// <param name="track">flag that will be used in a switch to determine which track to play</param>
        public void playSound(int track)
        {
            //Switch statement that determines which track to play
            switch (track)
            {
                case 1:
                    cannon.Play(0.5f * masterVolume, 0.0f, 0.0f);
                    break;
                case 2:
                    pulse.Play(0.1f * masterVolume, 1.0f, 0.0f);
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
                case 10:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// This will stop the music that is currently being played
        /// </summary>
        public void stopTracks()
        {
            MediaPlayer.Pause();
        }
    }
}
