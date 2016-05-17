using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Chroma.GameClasses.Utilities.FrontendBackend;

namespace Chroma
{
    public partial class Levels : PhoneApplicationPage
    {
        //Local DataPackage object that will be pushed to the Global version of this object when navigating away from this page.
        DataPackage data;

        public Levels()
        {
            InitializeComponent();
            data = new DataPackage();
        }

        /// <summary>
        /// Overloaded navigated to function that will create access to the Datapackage object
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            App app = Application.Current as App;

            //If the data object has been created, store locally
            if (app.data != null)
            {
                data = app.data;
            }

            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Overloaded navigated from function that will push the DataPackage back to the global variable when the app navigates
        /// away from this page.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App app = Application.Current as App;

            //If the local data object has been created, store globally
            if (data != null)
            {
                app.data = data;
            }

            base.OnNavigatedFrom(e);
        }

        // Button Click listener that will indentify the button that was pushed and store that data into the DataPackage Object
        private void Nav_Game(object sender, RoutedEventArgs e)
        {
            Button tempButton = (Button)sender;

            //Pushes the sender through a switch to determine which level should be run.
            switch (tempButton.Name)
            {
                case "Level1":
                    {
                        data.setStartLevel(0);
                        break;
                    }
                case "Level2":
                    {
                        data.setStartLevel(1);
                        break;
                    }
                case "Level3":
                    {
                        data.setStartLevel(2);
                        break;
                    }
                default:
                    data.setStartLevel(0);
                    break;
            }

            NavigationService.Navigate(new Uri("/ColorEditor.xaml", UriKind.Relative));
        }
    }
}