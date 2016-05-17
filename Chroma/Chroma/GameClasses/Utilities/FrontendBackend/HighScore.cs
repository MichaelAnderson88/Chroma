using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Chroma.GameClasses.Utilities.FrontendBackend
{
    public class HighScore
    {
        //Local Score values
        public int[] scores = new int[3] { 0, 0, 0};

        //Contains the highscore data
        XDocument highScoreData;

        /// <summary>
        /// When the constructor runs, it will parse highscores from the file and store the scores locally
        /// </summary>
        public HighScore()
        {
            //Loads the file from isolated storage
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                //If the file exists, load it
                if (isoStore.FileExists("Highscores.xml"))
                {
                    using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream("Highscores.xml", FileMode.Open, isoStore))
                    {
                        highScoreData = XDocument.Load(isoStream);
                    }
                }
                else //else create it
                {
                    using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (Stream stream = storage.CreateFile("Highscores.xml"))
                        {
                            highScoreData = createXDoc();
                            createXDoc().Save(stream);
                        }
                    }
                }
            }

            //Parses the highscore from the level xml
            IEnumerable<XElement> score = highScoreData.Descendants("Level");

            int tempCount = 0;

            //Parses the Highscore Values out of the file
            foreach (XElement s in score)
            {
                scores[tempCount] = Convert.ToInt16(s.Attribute("Score").Value);
                tempCount++;
            }
        }

        /// <summary>
        /// Saves new highscores to the file
        /// </summary>
        public void saveNewHighscore(int highscore, int level)
        {
            //Saves the score to the list
            scores[level] = highscore;

            //Sets up file structure and writes to file
            var doc = createXDoc();

            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (Stream stream = storage.CreateFile("Highscores.xml"))
                {
                    doc.Save(stream);
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
                    new XElement("Level", new XAttribute("Number", 1), new XAttribute("Score", scores[0])),
                    new XElement("Level", new XAttribute("Number", 2), new XAttribute("Score", scores[1])),
                    new XElement("Level", new XAttribute("Number", 3), new XAttribute("Score", scores[2]))
                    ));
        }
    }
}
