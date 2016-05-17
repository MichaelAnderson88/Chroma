using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Chroma.GameClasses.Utilities.FrontendBackend;
using System.Windows.Navigation;

namespace Chroma
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        // Simple button Click event handler to take us to the Level Selection Page
        private void Nav_Levels(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Levels.xaml", UriKind.Relative));
        }

        // Simple button Click event handler to take us to the Options Page
        private void Nav_Options(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Options.xaml", UriKind.Relative));
        }
    }
}