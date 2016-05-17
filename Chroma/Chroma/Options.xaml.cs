using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Linq;

namespace Chroma
{
    public partial class Options : PhoneApplicationPage
    {
        public Options()
        {
            InitializeComponent();
        }

        private void ConfirmChanges(object sender, RoutedEventArgs e)
        {
            //If the mute checkbox was checked
            if (Convert.ToBoolean(mute.IsChecked))
            {
                //Set the flag to mute the game sound
                App app = Application.Current as App;
                app.data.setMute(true);
            }

            //If the delete data checkbox was checked
            if (Convert.ToBoolean(delete.IsChecked))
            {
                //Creates a new xml highscore file to delete the data
                using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (Stream stream = storage.CreateFile("Highscores.xml"))
                        {
                            createXDoc().Save(stream);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets up the basic format of the XML file and returns it as an XDocument for use this class
        /// </summary>
        private XDocument createXDoc()
        {
            return new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                    new XElement("Highscores",
                    new XElement("Level", new XAttribute("Number", 1), new XAttribute("Score", 0)),
                    new XElement("Level", new XAttribute("Number", 2), new XAttribute("Score", 0)),
                    new XElement("Level", new XAttribute("Number", 3), new XAttribute("Score", 0))
                    ));
        }
    }
}