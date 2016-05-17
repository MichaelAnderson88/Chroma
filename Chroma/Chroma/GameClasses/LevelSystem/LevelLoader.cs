using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Xml;

namespace Chroma.GameClasses.LevelSystem
{
    /// <summary>
    /// This class will be used at the start of the application and will parse all Level files and store 
    /// the information in XDocuments. This class will also create and manage the List of Level objects 
    /// that will be used to run the game.
    /// </summary>
    public class LevelLoader
    {

        /// <summary>
        /// Stores the Level objects that will be used to run the game. The levels will be stored in this list in proper
        /// order. The first level object in this list will be Level 1 for example.
        /// </summary>
        public List<Level> levels;

        /// <summary>
        /// Constructor for the LevelLoader class.
        /// </summary>
        public LevelLoader()
        {
            levels = new List<Level>();
        }

        /// <summary>
        /// This function will take in a String of filenames that will be parsed into XDocument objects for e
        /// </summary>
        /// <param name="filenames">List of the Strings of names of files to be parsed</param>
        public void loadLevels(List<String> filenames)
        {
            //Iterates through the Strings containing the filenames of the XML files and parses into a XDocument object.
            //The Level objects are then constructed with the parsed data.
            foreach (String file in filenames)
            {
                //Creates a temporary XDocument object and parses the XML file
                XDocument tempXDoc = XDocument.Load(file);

                //Creates a temporary Level object to be given the XDocument for that level.
                Level tempLevel = new Level(tempXDoc);

                //Adds the temporary level to the Level list.
                levels.Add(tempLevel);
            }
        }
    }
}
