using System;
using System.Collections.Generic;
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

namespace Chroma.GameClasses.LevelSystem
{
    public class Level
    {
        /// <summary>
        /// Struct that will store enemy data from xml file parse. This data will be used to
        /// create the enemies in the EnemyController class.
        /// </summary>
        public struct Enemydata
        {
            //Enemy Type
            public string name;

            //Time at which the Enemy should spawn in the game
            public int spawnTime;

            //Spawn location Value
            public int spawnLoc;
        }

        //List of stored Enemydata structs
        public List<Enemydata> enemies;

        //Contains the parsed XML data
        XDocument levelData;

        /// <summary>
        /// Constructor for the Level class. Will take in a XDocument object and parse the data within to populate a list
        /// of Enemydata structs that will be used in the EnemyController's spawning system.
        /// </summary>
        /// <param name="data">XDocument object containing parsed XML level data</param>
        public Level(XDocument data)
        {
            //Stores the XDocument parameter locally for future use.
            levelData = data;

            enemies = new List<Enemydata>();
            parseXDoc();
        }

        /// <summary>
        /// Parses the XDocument passed from the LevelLoader and stores extracted data in Enemydata structs to 
        /// be used in EnemyController.
        /// </summary>
        private void parseXDoc()
        {
            //List of XElements extracted from the XDocument
            IEnumerable<XElement> enemyParse = levelData.Descendants("Level").Descendants("Enemy");

            //Iterates through the parsed list of Enemy XElements and creates an Enemydata struct and 
            //adds it to the list.
            foreach (XElement currentEnemy in enemyParse)
            {
                //Temporary Enemydata struct
                Enemydata e = new Enemydata();

                //Parses the String data from the XElement objects obtained from the XDocument
                e.name = currentEnemy.Attribute("Name").Value;
                e.spawnTime = Convert.ToInt16(currentEnemy.Attribute("SpawnTime").Value);
                e.spawnLoc = Convert.ToInt16(currentEnemy.Attribute("SpawnLocation").Value);

                //Adds the temporary struct to the list
                enemies.Add(e);
            }
        }
    }
}
