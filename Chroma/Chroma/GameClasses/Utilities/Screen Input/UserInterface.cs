using Chroma.GameClasses.PlayerSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Chroma.GameClasses.Utilities.Screen_Input
{
    /// <summary>
    /// This class will build and control the overlay that will act as the user interface for the game. It will be drawn on the lower portion
    /// of the screen and contain controls for changing the colours of the Player's shield and projectiles based on what was set in the menu
    /// system before the game was launched.
    /// </summary>
    public class UserInterface
    {
        //Textures for the inferface
        Texture2D overlay;
        Texture2D button;

        //Spritefont
        SpriteFont font;

        //Rectangles for the interface components
        public Rectangle overlayRect;
        Rectangle shield1, shield2, proj1, proj2;

        //Color components for shield and proj rectangles
        Color s1, s2, p1, p2;

        /// <summary>
        /// Constructor that will set up the rectangles that will not change and will pull Color information from the App.
        /// </summary>
        public UserInterface()
        {
            //Pulls the instance of the application to grab the color data
            App app = Application.Current as App;

            //Stores color data locally
            p1 = app.data.projectile1;
            p2 = app.data.projectile2;
            s1 = app.data.shield1;
            s2 = app.data.shield2;

            //Sets interface position
            arrangeInterface();
        }

        /// <summary>
        /// Loads the assets from the Content pipeline.
        /// </summary>
        public void load(Texture2D overlay, Texture2D button, SpriteFont font)
        {
            this.overlay = overlay;
            this.button = button;
            this.font = font;
        }

        /// <summary>
        /// Function that will position the components in this class.
        /// </summary>
        private void arrangeInterface()
        {
            //Main interface
            overlayRect = new Rectangle(0, (SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height - 200),
                SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width, 200);

            //Color changing rectangles
            shield1 = new Rectangle(20, SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height - 160, 50, 50);
            shield2 = new Rectangle(20, SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height - 90, 50, 50);

            proj1 = new Rectangle(SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width - (20 + 50),
                SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height - 160, 50, 50);
            proj2 = new Rectangle(SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width - (20 + 50),
                SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height - 90, 50, 50);
        }

        /// <summary>
        /// Draws components to screen.
        /// </summary>
        public void draw(SpriteBatch sb)
        {
            //Draw overlay
            sb.Draw(overlay, overlayRect, Color.Gray);

            //Draws labels
            sb.DrawString(font, "Shields", new Vector2(overlayRect.X + 5, overlayRect.Y - 10), Color.White);
            sb.DrawString(font, "Weapons", new Vector2(SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width - 100, overlayRect.Y - 10), Color.White);

            //Draw Color changing controls
            sb.Draw(button, shield1, s1);
            sb.Draw(button, shield2, s2);
            sb.Draw(button, proj1, p1);
            sb.Draw(button, proj2, p2);
        }

        /// <summary>
        /// Update function that will check for screen touches that intersect with the Color controls
        /// </summary>
        public void update(PlayerShip p, TouchCollection touches)
        {
            //If there was a touch
            if (touches.Count > 0 && touches[0].State == TouchLocationState.Pressed)
            {
                //Find the point of the touch
                Microsoft.Xna.Framework.Point point = new Microsoft.Xna.Framework.Point((int)touches[0].Position.X, (int)touches[0].Position.Y);

                //Check to see if that point was in any of the Color Controls and set the color if so
                //Shields
                if (shield1.Contains(point))
                {
                    foreach (Shield s in p.shields)
                    {
                        s.color = s1;
                    }

                    p.color = s1;
                }

                if (shield2.Contains(point))
                {
                    foreach (Shield s in p.shields)
                    {
                        s.color = s2;
                    }

                    p.color = s2;
                }

                //Projectiles
                if (proj1.Contains(point))
                {
                    foreach (Weapon w in p.weapons)
                    {
                        w.setProjectileColor(p1);
                    }
                }

                if (proj2.Contains(point))
                {
                    foreach (Weapon w in p.weapons)
                    {
                        w.setProjectileColor(p2);
                    }
                }
            }
        }
    }
}
