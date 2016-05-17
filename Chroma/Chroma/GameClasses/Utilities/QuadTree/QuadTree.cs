using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Chroma.GameClasses.Controllers;

namespace Chroma.GameClasses.Utilities.QuadTree
{
    class QTNode
    {
        //Physical Variables for the QuadTree Node
        Vector2 position;
        Vector2 size;
        Rectangle node;

        //Parent and Children Nodes
        QTNode parent;
        List<QTNode> children;

        //CollisionObject List
        public List<CollisionObject> objectsWithin;

        //Depth of the Node
        int depth;

        //Flag that is set if the collision is proper
        private bool properCollision = false;

        /// <summary>
        /// Constructor for the Quad Tree Node. If the root node is being created, the parent is null.
        /// </summary>
        public QTNode(Vector2 position, Vector2 size, QTNode parent, int depth, int maxDepth)
        {
            this.position = position;
            this.parent = parent;
            this.depth = depth;
            this.size = size;
            children = new List<QTNode>();
            node = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            objectsWithin = new List<CollisionObject>();

            //Checks to see if this node is at the maximum depth of the quad tree. If it is not then create children nodes.
            if (depth < maxDepth)
            {
                setChildren(maxDepth);
            }
        }

        /// <summary>
        /// Builds the list of child nodes that this node contains. This function is only called if the max depth has
        /// not been reached.
        /// </summary>
        /// <param name="maxDepth">Is the maximum amount of tiers to the tree</param>
        public void setChildren(int maxDepth)
        {
            QTNode topLeft = new QTNode(position, new Vector2((size.X / 2), (size.Y / 2)), this, (depth + 1), maxDepth);
            QTNode topRight = new QTNode(new Vector2(size.X / 2, position.Y), new Vector2((size.X / 2), (size.Y / 2)), this, (depth + 1), maxDepth);
            QTNode bottomLeft = new QTNode(new Vector2(position.X, (size.Y / 2)), new Vector2((size.X / 2), (size.Y / 2)), this, (depth + 1), maxDepth);
            QTNode bottomRight = new QTNode(new Vector2((size.X / 2), (size.Y / 2)), new Vector2((size.X / 2), (size.Y / 2)), this, (depth + 1), maxDepth);
            children.Add(topLeft);
            children.Add(topRight);
            children.Add(bottomLeft);
            children.Add(bottomRight);
        }

        /// <summary>
        /// Update method that updates the CollisionObject list, calls all the child node's updates, 
        /// checks CollisionObjects in that list for collisions, then dumbs the list back to the parent.
        /// </summary>
        public void update()
        {
            //Updates the CollisionObject list to ensure that only CollisionObjects within itself remain
            updateList();

            //Checks if leaf node
            if (children.Count != 0)
            {
                //Recursively call all children's update
                foreach (QTNode q in children)
                {
                    q.update();
                }
            }

            //Iterates through the remaining CollisionObjects and checks collisions with other CollisionObjects in the list
            foreach (CollisionObject c in objectsWithin)
            {
                checkCollisions(c);
            }
        }

        /// <summary>
        /// Updates the internal list of objects
        /// </summary>
        public void updateList()
        {
            List<CollisionObject> tempObject = new List<CollisionObject>();
            //Checks if leaf node
            if (children.Count != 0 && objectsWithin.Count != 0)
            {
                //First: grabs a CollisionObject
                foreach (CollisionObject c in objectsWithin)
                {
                    List<QTNode> tempList = new List<QTNode>();
                    
                    //Second: checks it against all children
                    foreach (QTNode q in children)
                    {
                        //Third: checks to see if that child intersects that CollisionObject
                        if (q.node.Intersects(c.hitBox))
                        {
                            //If so adds to list
                            tempList.Add(q);
                        }

                    }

                    //Fourth: if only one child intersected the CollisionObject
                    if (tempList.Count != 0 && tempList.Count < 2)
                    {
                        //Add that CollisionObject to that child's list
                        tempList[0].objectsWithin.Add(c);
                    }
                    else
                    {
                        //If more than one child intersected the CollisionObject, add to temp list to keep in this node's list
                        tempObject.Add(c);
                    }
                }

                objectsWithin = tempObject;
            }
        }

        /// <summary>
        /// Dumps the CollisionObjects stored in the list back to the parent node
        /// </summary>
        public void dumpList()
        {
            if (children.Count != 0)
            {
                foreach (QTNode q in children)
                {
                    q.dumpList();
                }
            }

            //First check to see if current node is the root node
            if (parent != null)
            {
                foreach (CollisionObject c in objectsWithin)
                {
                    //Sends boxes to parent
                    parent.objectsWithin.Add(c);
                }

                //Clear the CollisionObject list
                objectsWithin = new List<CollisionObject>();
                //tempObject = new List<CollisionObject>();
            }
        }

        /// <summary>
        /// Checks a CollisionObject from the internal CollisionObject list with all other CollisionObjects in that list.
        /// </summary>
        public void checkCollisions(CollisionObject toCheck)
        {
            List<CollisionObject> totalObjects = new List<CollisionObject>();

            //Collects CollisionObjects from the parent nodes to check for collisions
            //This eliminates the issue of CollisionObjects colliding but being stored in different
            //lists
            switch (depth)
            {
                case 2:
                    foreach (CollisionObject c in parent.objectsWithin)
                    {
                        totalObjects.Add(c);
                    }
                    break;
                case 3:
                    foreach (CollisionObject c in parent.objectsWithin)
                    {
                        totalObjects.Add(c);
                    }
                    foreach (CollisionObject c in parent.parent.objectsWithin)
                    {
                        totalObjects.Add(c);
                    }
                    break;
                case 4:
                    foreach (CollisionObject c in parent.objectsWithin)
                    {
                        totalObjects.Add(c);
                    }
                    foreach (CollisionObject c in parent.parent.objectsWithin)
                    {
                        totalObjects.Add(c);
                    }
                    foreach (CollisionObject c in parent.parent.parent.objectsWithin)
                    {
                        totalObjects.Add(c);
                    }
                    break;
                default:
                    break;
            }

            //Adds all the current CollisionObjects to the total List as well
            foreach (CollisionObject c in objectsWithin)
            {
                totalObjects.Add(c);
            }

            //Checks all CollisionObjects in the total CollisionObjects list for collisions.
            foreach (CollisionObject c in totalObjects)
            {
                //First make sure the CollisionObject is not the same as the one being checked
                if (!toCheck.Equals(c))
                {
                    if (toCheck.collidable && c.collidable)
                    {
                        //Check for a collision
                        if (toCheck.hitBox.Intersects(c.hitBox))
                        {
                            //Checks to see if the collision is supposed to happen.
                            if ((toCheck.cType == CollisionObjectType.EnemyProjectile && c.cType == CollisionObjectType.Player)
                                || (c.cType == CollisionObjectType.EnemyProjectile && toCheck.cType == CollisionObjectType.Player))
                            {
                                properCollision = true;
                            }
                            else if ((toCheck.cType.Equals(CollisionObjectType.PlayerProjectile) && (c.cType.Equals(CollisionObjectType.Enemy) || c.cType.Equals(CollisionObjectType.Boss)))
                                || (c.cType.Equals(CollisionObjectType.PlayerProjectile) && (toCheck.cType.Equals(CollisionObjectType.Enemy) || toCheck.cType.Equals(CollisionObjectType.Boss))))
                            {
                                properCollision = true;
                            }

                            //If the collision is deemed proper, do the proper collision code, else ignore the collision
                            if (properCollision)
                            {
                                //If there is a collision set the flags for both objects
                                toCheck.hasCollided = true;
                                c.hasCollided = true;

                                //If toCheck is not a projectile
                                if ((toCheck.cType != CollisionObjectType.EnemyProjectile) && (toCheck.cType != CollisionObjectType.PlayerProjectile))
                                {
                                    //Set the collision color for damage computation
                                    toCheck.collisionColor = c.color;
                                }
                                else //Else c is not a projectile and should be assigned collision color
                                {
                                    c.collisionColor = toCheck.color;
                                }

                                //reset flag
                                properCollision = false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Setter for the objectsWithin List. This only gets called for the Root node.
        /// </summary>
        public void setList(List<CollisionObject> objectList)
        {
            objectsWithin = objectList;
        }

        /// <summary>
        /// Getting for objectsWithin List. This only gets called by the Root node
        /// </summary>
        public List<CollisionObject> getList()
        {
            return objectsWithin;
        }
    }
}
