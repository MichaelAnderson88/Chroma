using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Chroma.GameClasses.Controllers;
using Chroma.GameClasses.Utilities;

namespace Chroma
{
    public partial class GamePage : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;

        int startingLevel = 0;

        // Gameplay Controller
        LevelController game;

        //Sound Controller
        SoundManager sm;

        public GamePage()
        {
            InitializeComponent();

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;

            //Initialize SoundManager
            sm = new SoundManager(contentManager);

            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;

            //Pulls the Data from the App
            App app = Application.Current as App;
            startingLevel = app.data.level;

            // Creates the instance of the Game. Only one will ever be created. At this point
            // the LevelController will pre-load all level information to reduce load times between
            // levels.
            game = new LevelController(contentManager, app.data, sm);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            // Loads the level passed in from the frontend
            game.Load(startingLevel);

            // Start the timer
            timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Stop the timer
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            //Pauses music
            sm.stopTracks();

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            // Updates LevelController
            game.Update(timer);

            if (game.mainMenu)
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));

                //Clears the navigation stack
                while (NavigationService.BackStack.Any())
                {
                    NavigationService.RemoveBackEntry();
                }
            }
        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                // Calls LevelController draw
                game.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}