using System.Collections.Generic;
using UnityEngine;

namespace AntSimulation
{
    /// <summary>
    /// This object handles the ant's sensor and movement
    /// </summary>
    internal class Ant : MonoBehaviour
    {
        //Changeable parameters

        private static readonly int MaxDistance = 300; //The number of breadCrumbs the ant should leave before tracing them back
        private static readonly byte numberOfBreadCrumbs = 8; //The number of breadCrumb object the ant should have
        private static readonly float pickUpDist = 3f; //The range of the food pickup
        private static readonly float speed = 0.5f; //The speed of the ant (The length of the vector)
        private static readonly float viewDistance = 20f; //The distance of the ant detection
        private static readonly float viewAngle = 1.5f; //The angle which the ant can see (in radian)
        public static float DirChangeTimer = 0.2f; //The seconds there should be between the direction changes

        //Cache

        private static int foodLayerMask; //The mask for the foodLayer
        private static int foodTrailLayerMask; //The mask for the foodTrailLayer
        private static Transform DroppedCellParent; //Pointer to the DroppedCells collection (So we can organize them logically)
        private static GameObject DroppedCellPrefab; //Prefab for the droppedCellObject

        private float lookingDirection = 0f; //The direction which the ant is looking (in radian)
        private float colXCoord; //The x coordinate of the colony
        private float colYCoord; //The y coordinate of the colony
        private Stack<Vector2> breadCrumbs = new Stack<Vector2>(); //The cells location which the ant drops when it doesn't carry any food
        private Queue<AntDroppedCell> trail = new Queue<AntDroppedCell>(); //The objects which are showed visually as the dropped cells
        private Vector2 moveVector = new Vector2(); //The vector which the ant should move when Move() is called
        private List<Vector2> previousFoodTrails = new List<Vector2>(); //The coordinates of the previous foodTrails so it can be passed to the next foodTrail (it's may size is equal to the numberOfBreadCrumbs)
        private Stack<Vector2> nextFoodTrail = new Stack<Vector2>(); //This stack is filled when the ant sees a foodTrail

        /// <summary>
        /// Returns the x pos of the ant
        /// </summary>
        public float XPos { get => this.transform.position.x; }

        /// <summary>
        /// Returns the y pos of the ant
        /// </summary>
        public float YPos { get => this.transform.position.y; }

        /// <summary>
        /// Gets if the ant has food
        /// </summary>
        public bool HasFood { get; private set; }

        /// <summary>
        /// Gets if the ant is going back
        /// </summary>
        public bool IsGoingBack { get; private set; }

        //---------------------------------------------------------
        // Start is called before the first frame update
        private void Start()
        {
            //Sets the colony's coords
            this.colXCoord = transform.position.x;
            this.colYCoord = transform.position.y;
            //Generate a random direction
            this.lookingDirection = Random.Range(-Mathf.PI, Mathf.PI);
            //Fills the trail queue
            for (int i = 0; i < numberOfBreadCrumbs; i++)
            {
                this.trail.Enqueue(Instantiate(DroppedCellPrefab, this.transform.position, new Quaternion(), DroppedCellParent).GetComponent<AntDroppedCell>());
            }
            //Sets the methods to periodic calls
            InvokeRepeating("ChangeDir", 0, DirChangeTimer);
            InvokeRepeating("DropBreadCrumb", 0, 0.9f);
            InvokeRepeating("Move", 0, 0.05f);
        }

        //-----------------------------------------------------
        //Runs when the script is loaded
        private void Awake()
        {
            //Fills the cache
            foodLayerMask = 1 << LayerMask.NameToLayer("foodLayer");
            foodTrailLayerMask = 1 << LayerMask.NameToLayer("foodTrailLayer");
            DroppedCellParent = GameObject.Find("AntDroppedCells").transform;
            DroppedCellPrefab = Resources.Load<GameObject>("Prefabs/AntDroppedCell");
        }

        //----------------------------------------------------------
        //Drops a breadcrumb
        private void DropBreadCrumb()
        {
            AntDroppedCell Trailcache = this.trail.Dequeue();
            if (this.HasFood)
            {
                Trailcache.ResetCell(System.Convert.ToByte(this.HasFood), this.transform.position, this.previousFoodTrails);
                if (this.previousFoodTrails.Count == numberOfBreadCrumbs)
                    this.previousFoodTrails.RemoveAt(0);

                this.previousFoodTrails.Add(this.transform.position);
            }
            else
                Trailcache.ResetCell(System.Convert.ToByte(this.HasFood), this.transform.position);
            this.trail.Enqueue(Trailcache);
        }

        //---------------------------------------------
        //Gets the direction which the ant should go
        private float See()
        {
            //If the ant is going back get the last breadCrumb's angle
            if (this.IsGoingBack)
            {
                if (CalcVectorLength(this.XPos, this.YPos, colXCoord, colYCoord) < 5f)
                {
                    this.GotHome();
                    return this.See();
                }
                if (this.breadCrumbs.Count > 0)
                {
                    Vector2 breadCrumb = this.breadCrumbs.Pop();
                    return CalcAngle(this.XPos, this.YPos, breadCrumb.x, breadCrumb.y);
                }
                else
                {
                    return CalcAngle(this.XPos, this.YPos, colXCoord, colYCoord);
                }
            }

            //Otherwise look for food and pick it up if is close enough
            float closestDist = float.MaxValue;
            float BestAngle = float.MinValue;
            //Get all of the food objects in a circle
            Collider2D[] foodsInCircle = Physics2D.OverlapCircleAll(this.transform.position, viewDistance, foodLayerMask);
            for (int i = 0; i < foodsInCircle.Length; i++)
            {
                Vector2 closest = foodsInCircle[i].transform.position;
                float enclosedAngle = CalcAngle(XPos, YPos, closest.x, closest.y);

                //If the ant can see that object
                if (Mathf.Abs(this.lookingDirection - enclosedAngle) <= viewAngle / 2)
                {
                    float tempDist = CalcVectorLength(closest.x, closest.y, this.XPos, this.YPos);
                    //Pick is up if it is close enough
                    if (tempDist < pickUpDist)
                    {
                        Destroy(foodsInCircle[i].gameObject);
                        PickedUpFood();
                        return this.See();
                    }
                    else if (closestDist > tempDist)
                    {
                        closestDist = tempDist;
                        BestAngle = enclosedAngle;
                    }
                }
            }
            //If there was any food go that way
            if (BestAngle != float.MinValue)
            {
                return BestAngle;
            }
            //If it has a foodTrail's stack use those positions for calculating the angle
            if (this.nextFoodTrail.Count > 0)
            {
                Vector2 nextPos = this.nextFoodTrail.Pop();
                return CalcAngle(this.XPos, this.YPos, nextPos.x, nextPos.y);
            }

            //Get all the foodTrails in a circle
            Collider2D[] foodTrails = Physics2D.OverlapCircleAll(this.transform.position, viewDistance, foodTrailLayerMask);
            for (int i = 0; i < foodTrails.Length; i++)
            {
                Vector2 closest = foodTrails[i].transform.position;
                float enclosedAngle = CalcAngle(this.XPos, this.YPos, closest.x, closest.y);
                float relativeAngle = this.lookingDirection - enclosedAngle;

                if (Mathf.Abs(relativeAngle) <= viewAngle / 2)
                {
                    List<Vector2> tempList = foodTrails[i].gameObject.GetComponent<AntDroppedCell>().prevTrailPositions;
                    //Copy the list's content to a stack
                    for (int listIndex = 0; listIndex < tempList.Count; listIndex++)
                    {
                        this.nextFoodTrail.Push(tempList[listIndex]);
                    }

                    return enclosedAngle;
                }
            }

            return this.lookingDirection += Random.Range(-0.4f, 0.4f);
        }

        //-------------------------------------------------
        //Change the ant's direction
        private void ChangeDir()
        {
            if (breadCrumbs.Count == MaxDistance)
                this.IsGoingBack = true;

            lookingDirection = See();

            //Keep the lookingDirection in [-Math.PI,Math.PI]
            if (this.lookingDirection >= Mathf.PI)
                this.lookingDirection -= Mathf.PI * 2;
            else if (this.lookingDirection <= -Mathf.PI)
                this.lookingDirection += Mathf.PI * 2;

            //Rotates the ant
            this.transform.eulerAngles = Vector3.forward * Mathf.Rad2Deg * this.lookingDirection;
            //Calculates the new moveVector
            this.moveVector.Set(speed * Mathf.Cos(this.lookingDirection), speed * Mathf.Sin(this.lookingDirection));
            //Drop a breadCrumb if the ant isn't going back
            if (!this.IsGoingBack)
                this.breadCrumbs.Push(this.transform.position);
        }

        //------------------------------------------------------
        //Moves the ant according to the moveVector
        private void Move()
        {
            this.transform.position = new Vector3(this.transform.position.x + moveVector.x, this.transform.position.y + moveVector.y);
        }

        //-----------------------------------------------------
        //Should only be called when the ant picked up food
        private void PickedUpFood()
        {
            this.HasFood = true;
            this.IsGoingBack = true;
        }

        //---------------------------------------------------------
        //Should be only called when the ant got home
        private void GotHome()
        {
            if (HasFood)
            {
                this.lookingDirection = (this.lookingDirection - Mathf.PI) % (Mathf.PI * 2);
                this.GetComponentInParent<Colony>().BroughtHomeFood();
                this.previousFoodTrails.Clear();
            }

            this.HasFood = false;
            this.IsGoingBack = false;
            this.breadCrumbs.Clear();
        }

        //-----------------------------------------------------------------------
        /// <summary>
        /// Calculates an angle of the vector
        /// </summary>
        /// <param name="x1">x coord of the base of the vector</param>
        /// <param name="y1">y coord of the base of the vector</param>
        /// <param name="x2">x coord of the end of the vector</param>
        /// <param name="y2">y coord of the end of the vector</param>
        /// <returns>the angle</returns>
        private static float CalcAngle(float x1, float y1, float x2, float y2)
        {
            return Mathf.Atan2(y2 - y1, x2 - x1);
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Calculates the length of the vector
        /// </summary>
        /// <param name="x1">x coord of the base of the vector</param>
        /// <param name="y1">y coord of the base of the vector</param>
        /// <param name="x2">x coord of the end of the vector</param>
        /// <param name="y2">y coord of the end of the vector</param>
        /// <returns>the length of the vector</returns>
        private static float CalcVectorLength(float x1, float y1, float x2, float y2)
        {
            return Mathf.Sqrt(Mathf.Pow(x2 - x1, 2) + Mathf.Pow(y2 - y1, 2));
        }

        //----------------------------------------------------------------------
        //When the ant goes out of the camera reverse the move direction
        private void OnBecameInvisible()
        {
            lookingDirection += Mathf.PI;
        }
    }
}