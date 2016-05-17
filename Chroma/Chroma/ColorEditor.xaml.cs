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
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Chroma
{
    public partial class Colors : PhoneApplicationPage
    {
        //Local DataPackage object that will be pushed to the Global version of this object when navigating away from this page.
        DataPackage data;

        // Create the popup object.
        Popup p = new Popup();

        public Colors()
        {
            InitializeComponent();
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

        /// <summary>
        /// Button Click listener that will indentify which colors were selected, store that information in the global data variable
        /// then proceed to the game.
        /// </summary>
        private void Nav_Game(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }


        private void showPopup_Click(object sender, RoutedEventArgs e)
        {
            // Create some content to show in the popup. Typically you would 
            // create a user control.
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(System.Windows.Media.Colors.Black);
            border.BorderThickness = new Thickness(5.0);

            StackPanel panel1 = new StackPanel();
            panel1.Background = new SolidColorBrush(System.Windows.Media.Colors.LightGray);

            //Red Button
            Button button1 = new Button();
            button1.Content = "Close";
            button1.Margin = new Thickness(5.0);
            button1.Name = "Red";
            button1.Background = new SolidColorBrush(System.Windows.Media.Colors.Red);

            //Green Button
            Button button2 = new Button();
            button2.Content = "Close";
            button2.Margin = new Thickness(5.0);
            button2.Name = "Green";
            button2.Background = new SolidColorBrush(System.Windows.Media.Colors.Green);

            //Blue Button
            Button button3 = new Button();
            button3.Content = "Close";
            button3.Margin = new Thickness(5.0);
            button3.Name = "Blue";
            button3.Background = new SolidColorBrush(System.Windows.Media.Colors.Blue);

            //Switch to determine which Button Handler to use
            //First cast the button to grab information from the sender
            Button b = (Button)sender;

            //Send button name through switch which will set the pop up's button's click handler
            switch (b.Name)
            {
                case "S1":
                    button1.Click += new RoutedEventHandler(Shield1_Click);
                    button2.Click += new RoutedEventHandler(Shield1_Click);
                    button3.Click += new RoutedEventHandler(Shield1_Click);
                    break;
                case "S2":
                    button1.Click += new RoutedEventHandler(Shield2_Click);
                    button2.Click += new RoutedEventHandler(Shield2_Click);
                    button3.Click += new RoutedEventHandler(Shield2_Click);
                    break;
                case "W1":
                    button1.Click += new RoutedEventHandler(Attack1_Click);
                    button2.Click += new RoutedEventHandler(Attack1_Click);
                    button3.Click += new RoutedEventHandler(Attack1_Click);
                    break;
                case "W2":
                    button1.Click += new RoutedEventHandler(Attack2_Click);
                    button2.Click += new RoutedEventHandler(Attack2_Click);
                    button3.Click += new RoutedEventHandler(Attack2_Click);
                    break;
                default:
                    break;
            }

            TextBlock textblock1 = new TextBlock();
            textblock1.Text = "Choose a Color";
            textblock1.Margin = new Thickness(5.0);
            panel1.Children.Add(textblock1);
            panel1.Children.Add(button1);
            panel1.Children.Add(button2);
            panel1.Children.Add(button3);
            border.Child = panel1;

            // Set the Child property of Popup to the border 
            // which contains a stackpanel, textblock and button.
            p.Child = border;

            // Set where the popup will show up on the screen.
            p.VerticalOffset = 150;
            p.HorizontalOffset = 150;

            // Open the popup.
            p.IsOpen = true;
        }

        /// <summary>
        /// Shield 1 Button click handler. Sets the background of the original button to show the user
        /// what color has been selected and sets the value in the DataPacket.
        /// </summary>
        void Shield1_Click(object sender, RoutedEventArgs e)
        {
            // Close the popup.
            p.IsOpen = false;

            //sets temp button to access color value
            Button temp = (Button)sender;

            //Sets the color of the original button to match that of the chosen button
            S1.Background = temp.Background;

            //Switch to determine what color to load into the DataObject
            switch(temp.Name)
            {
                case "Red":
                    {
                        //Sets the information in the DataPacket
                        data.shield1 = Microsoft.Xna.Framework.Color.Red;
                        break;
                    }
                case "Green":
                    {
                        //Sets the information in the DataPacket
                        data.shield1 = Microsoft.Xna.Framework.Color.Green;
                        break;
                    }
                case "Blue":
                    {
                        //Sets the information in the DataPacket
                        data.shield1 = Microsoft.Xna.Framework.Color.Blue;
                        break;
                    }
            } 
        }

        /// <summary>
        /// Shield 2 Button click handler. Sets the background of the original button to show the user
        /// what color has been selected and sets the value in the DataPacket.
        /// </summary>
        void Shield2_Click(object sender, RoutedEventArgs e)
        {
            // Close the popup.
            p.IsOpen = false;

            //sets temp button to access color value
            Button temp = (Button)sender;

            //Sets the color of the original button to match that of the chosen button
            S2.Background = temp.Background;

            //Switch to determine what color to load into the DataObject
            switch (temp.Name)
            {
                case "Red":
                    {
                        //Sets the information in the DataPacket
                        data.shield2 = Microsoft.Xna.Framework.Color.Red;
                        break;
                    }
                case "Green":
                    {
                        //Sets the information in the DataPacket
                        data.shield2 = Microsoft.Xna.Framework.Color.Green;
                        break;
                    }
                case "Blue":
                    {
                        //Sets the information in the DataPacket
                        data.shield2 = Microsoft.Xna.Framework.Color.Blue;
                        break;
                    }
            }
        }

        /// <summary>
        /// Attack 1 Button click handler. Sets the background of the original button to show the user
        /// what color has been selected and sets the value in the DataPacket.
        /// </summary>
        void Attack1_Click(object sender, RoutedEventArgs e)
        {
            // Close the popup.
            p.IsOpen = false;

            //sets temp button to access color value
            Button temp = (Button)sender;

            //Sets the color of the original button to match that of the chosen button
            W1.Background = temp.Background;

            //Switch to determine what color to load into the DataObject
            switch (temp.Name)
            {
                case "Red":
                    {
                        //Sets the information in the DataPacket
                        data.projectile1 = Microsoft.Xna.Framework.Color.Red;
                        break;
                    }
                case "Green":
                    {
                        //Sets the information in the DataPacket
                        data.projectile1 = Microsoft.Xna.Framework.Color.Green;
                        break;
                    }
                case "Blue":
                    {
                        //Sets the information in the DataPacket
                        data.projectile1 = Microsoft.Xna.Framework.Color.Blue;
                        break;
                    }
            }
        }

        /// <summary>
        /// Attack 2 Button click handler. Sets the background of the original button to show the user
        /// what color has been selected and sets the value in the DataPacket.
        /// </summary>
        void Attack2_Click(object sender, RoutedEventArgs e)
        {
            // Close the popup.
            p.IsOpen = false;

            //sets temp button to access color value
            Button temp = (Button)sender;

            //Sets the color of the original button to match that of the chosen button
            W2.Background = temp.Background;

            //Switch to determine what color to load into the DataObject
            switch (temp.Name)
            {
                case "Red":
                    {
                        //Sets the information in the DataPacket
                        data.projectile2 = Microsoft.Xna.Framework.Color.Red;
                        break;
                    }
                case "Green":
                    {
                        //Sets the information in the DataPacket
                        data.projectile2 = Microsoft.Xna.Framework.Color.Green;
                        break;
                    }
                case "Blue":
                    {
                        //Sets the information in the DataPacket
                        data.projectile2 = Microsoft.Xna.Framework.Color.Blue;
                        break;
                    }
            }
        }
    }
}