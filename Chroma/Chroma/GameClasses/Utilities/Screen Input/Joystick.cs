using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Chroma;

namespace Chroma.GameClasses.Utilities.Screen_Input
{
    /// <summary>
    /// Class that will represent the Joystick control which will be used to move the player around the screen.
    /// </summary>
    public class Joystick
    {
        //Interface variables
        ContainmentType t = ContainmentType.Disjoint;
        int leftId = -1;
        private BoundingSphere stickCollision;
        TouchLocation? touch = null;

        //Vector Variables for the multiple components of the Joystick
        Vector2 thumbPosition;
        Vector2 max;
        Vector2 min;
        private Vector2 thumbOriginalPosition;
        private Vector2 padArea;
        private Vector2 padCenter;

        Vector3 touch3DPoint = Vector3.Zero;
        /// <summary>
        /// The Value From the Joystick's Vector2. This is used to help update the player's position
        /// </summary>
        public Vector2 stick
        {
            get
            {
                Vector2 scaledVector = (thumbPosition - thumbOriginalPosition) / (padTexture.Width / 2);
                scaledVector.Y *= -1;

                if (scaledVector.Length() > 1f)
                {
                    scaledVector.Normalize();
                }

                return scaledVector;
            }
        }

        //Offsets for screen placement
        private float xOffset;
        private float yOffest;

        //Texture Variables
        private Texture2D padTexture;
        private Texture2D thumbTexture;

        //Stores the maximum distance the joystick can be moved
        float maxDistance;

        /// <summary>
        /// Constructor for the Joystick. Takes in textures, sets the max distance and sets the placement values for the Joystick's
        /// position on the interface
        /// </summary>
        public Joystick(Texture2D pad, Texture2D thumb)
        {
            padTexture = pad;
            thumbTexture = thumb;

            maxDistance = (padTexture.Width - thumbTexture.Width);

            //Placement according to screen rotation
            PlaceControls();
        }

        /// <summary>
        /// Function that determines the screen's orientation and size and sets the Joystick's position accordingly
        /// </summary>
        private void PlaceControls()
        {
            // left pad, it's always present and gives the direction for movement
            xOffset = (SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width / 2) - (padTexture.Width / 2);
            yOffest = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height - padTexture.Height - 20;

            padArea = new Vector2(xOffset, yOffest);

            padCenter = new Vector2(padArea.X + padTexture.Width / 2, padArea.Y + padTexture.Height / 2);

            stickCollision = new BoundingSphere(new Vector3(padCenter, 0), padTexture.Width / 2);

            thumbOriginalPosition = new Vector2(padArea.X + (padTexture.Width - thumbTexture.Width) / 2,
                padArea.Y + (padTexture.Height - thumbTexture.Height) / 2);

            min = new Vector2(padCenter.X - padTexture.Width / 2 - thumbTexture.Width / 2,
                padCenter.Y - padTexture.Height / 2 - thumbTexture.Height / 2);
            max = new Vector2(padCenter.X + padTexture.Width / 2 - thumbTexture.Width / 2,
                padCenter.Y + padTexture.Height / 2 - thumbTexture.Height / 2);

            thumbPosition = thumbOriginalPosition;
        }

        /// <summary>
        /// Draw function determine's the position of the pad of the Joystick and draws both it and the base
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch sb)
        {
            Vector2 currentLeftPosition = new Vector2(thumbOriginalPosition.X + stick.X * maxDistance,
                thumbOriginalPosition.Y + stick.Y * maxDistance * -1);

            //Draws the components that make up the Joystick
            sb.Draw(padTexture, padArea, Color.White * 0.75f);
            sb.Draw(thumbTexture, currentLeftPosition, Color.White);
        }

        /// <summary>
        /// Update function determines if touches are occuring and sets values accordingly.
        /// </summary>
        public void Update(TouchCollection touches)
        {
            touch = null;

            touches = TouchPanel.GetState();

            //foreach variable in touches
            for (int i = 0; i < touches.Count; i++)
            {
                TouchLocation t = touches[i];

                float x = t.Position.X;
                float y = t.Position.Y;

                if (t.Id == leftId)
                {
                    touch = t;
                    continue;
                }

                if (leftId == -1)
                {
                    if (IsTouchingLeftStick(ref x, ref y))
                    {
                        touch = t;
                        continue;
                    }
                }
            }

            if (touch.HasValue)
            {
                thumbPosition = new Vector2(
                    touch.Value.Position.X - thumbTexture.Width / 2,
                    touch.Value.Position.Y - thumbTexture.Height / 2);
                
                thumbPosition = Vector2.Clamp(thumbPosition, min, max);
                leftId = touch.Value.Id;
            }
            else
            {
                leftId = -1;
                thumbPosition += (thumbOriginalPosition - thumbPosition) * 0.9f;
            }
        }

        /// <summary>
        /// This function handles touches to the joystick
        /// </summary>
        private bool IsTouchingLeftStick(ref float x, ref float y)
        {
            Vector3 point = new Vector3(x, y, 0);
            stickCollision.Contains(ref point, out t);
            return (t == ContainmentType.Contains);
        }
    }
}
