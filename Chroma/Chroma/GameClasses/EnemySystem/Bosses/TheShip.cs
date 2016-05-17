using Chroma.GameClasses.Controllers;
using Chroma.GameClasses.Utilities;
using Chroma.GameClasses.Utilities.Shields;
using Chroma.GameClasses.Utilities.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chroma.GameClasses.EnemySystem.Bosses
{
    /// <summary>
    /// This will be the first boss in the game. A very basic boss that will have a base attack pattern
    /// with multiple pulse and laser weapons, a special attack of volleys of pulse shots, and a critical state of
    /// volleys of pulse shots with lasers.
    /// </summary>
    class TheShip : Boss
    {
        //Physical Weapon Variables
        Vector2 pulseweaponloc1;
        Vector2 pulseweaponloc2;

        //Weapons
        Weapon pulse1;
        Weapon pulse2;

        //Shields
        Shield base1;

        /// <summary>
        /// Constructor for TheShip Boss object. Sets up default physical values for the Boss as well as initializes
        /// the AI that this boss will run.
        /// </summary>
        public TheShip(Vector2 spawnloc)
        {

            //Sets up BossData
            data = new BossData();
            data.speed = new Vector2(0, 50);
            data.fireRate = 0.5f;
            data.dimensions = new Vector2(129, 114);
            data.position = new Vector2(spawnloc.X - (data.dimensions.X / 2), spawnloc.Y + 50);
            data.hitpoints = 5000;

            hitBox = new Rectangle((int)data.position.X, (int)data.position.Y, (int)data.dimensions.X, (int)data.dimensions.Y);

            //Sets the weapon's location to the far sides of the ship
            pulseweaponloc1 = new Vector2(5.0f, data.dimensions.Y);
            pulseweaponloc2 = new Vector2(data.dimensions.X - 5, data.dimensions.Y);

            //Initializes Weapons and adds to the weapon list
            pulse1 = new PulseCannon(data.fireRate);
            pulse2 = new PulseCannon(data.fireRate);
            data.weapons.Add(pulse1);
            data.weapons.Add(pulse2);

            //Initializes Shields and adds to the shield list
            base1 = new BasicShield(data.position, data.dimensions);
            base1.setColor(Color.Green);
            data.shields.Add(base1);

            color = data.shields[0].color;

            //Sets type
            type = EnemyType.BossTheShip;

            ai = new TheShipAI();
        }

        /// <summary>
        /// Updates the Boss physical values based on the AI's current state.
        /// </summary>
        override public void update(GameTimer timer)
        {
            data = ai.state(data, timer);

            //Updates the position of the Boss based on speed.
            data.position.Y += data.speed.Y * (float)timer.UpdateInterval.TotalSeconds;
            data.position.X += data.speed.X * (float)timer.UpdateInterval.TotalSeconds;

            //Updates the visual position of the Boss
            hitBox = new Rectangle((int)data.position.X, (int)data.position.Y, (int)data.dimensions.X, (int)data.dimensions.Y);

            //updates Rate of Fire for weapons
            data.weapons[0].setRate(data.fireRate);
            data.weapons[1].setRate(data.fireRate);

            //update the Weapons
            data.weapons[0].update(timer, new Vector2(0, 810), (data.position + pulseweaponloc1));
            data.weapons[1].update(timer, new Vector2(0, 810), (data.position + pulseweaponloc2));

            //updates the shield
            data.shields[0].update(timer, data.position);
        }

        override public void draw(SpriteBatch sb)
        {
            //Draws the shields first so it gets drawn over by the enemy itself
            foreach (Shield s in data.shields)
            {
                s.draw(sb);
            }

            //Draws the enemy
            sb.Draw(textures[data.currentTexture], hitBox, Color.White);

            //Draws the projectiles
            foreach (Weapon w in data.weapons)
            {
                w.drawProjectiles(sb);
            }
        }

        /// <summary>
        /// Sets the textures of the projectiles fired by the weapons
        /// </summary>
        override public void setProjectileTextures(Texture2D tex)
        {
            foreach (Weapon w in data.weapons)
            {
                w.setProjectileTexture(tex);
            }
        }

        /// <summary>
        /// Sets the texture of the shields used
        /// </summary>
        override public void setShieldTextures(Texture2D tex)
        {
            foreach (Shield s in data.shields)
            {
                s.setTexture(tex);
            }
        }
    }
}
