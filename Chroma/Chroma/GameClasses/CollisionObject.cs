using Chroma.GameClasses.Controllers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chroma.GameClasses
{
    /// <summary>
    /// This is the super class for all collidable game objects in the game. PlayerShip, Enemy and Projectile objects will
    /// extend this class. This class is also made to allow these objects to be used in the Quad Tree Collision Detection System.
    /// </summary>
    public abstract class CollisionObject
    {
        //Physical Properties
        public Rectangle hitBox;

        //Collision flag
        public bool hasCollided = false;
        public bool collidable = true;

        //Object type used to further reduce collision checks within the quad tree
        public CollisionObjectType cType;

        //Color of the object
        public Color color;
        //Color of the object which collided with this object. Will be used for computing damage
        public Color collisionColor;
    }
}
