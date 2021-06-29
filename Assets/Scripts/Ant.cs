using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    private float time = 0f;
    private float angle = 0f;
    private float colXCoord;
    private float colYCoord;

    private static int MaxDistance = 300;

    private static readonly float pickUpDist = 3f;
    private static float speed = 0.5f;
    private static float viewRange = 20f;
    private static float viewAngle = 1.5f;
    private static int foodLayerNumber;
    private static int foodTrailLayerNumber;
    private Stack<Vector2> breadCrumbs = new Stack<Vector2>();
    private static Transform AntDroppedCellsParent;
    private Queue<AntDroppedCell> trail;
    private Vector2 moveVector = new Vector2();

    private GameObject AntDroppedCellObj;

    public static float DirChangeTimer = 0.2f;

    public float xPos { get => this.transform.position.x; }//Returns the x pos of the ant
    public float yPos { get => this.transform.position.y; }//Returns the y pos of the ant
    public bool hasFood { get; private set; }
    public bool goingBack { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        this.colXCoord = transform.position.x;
        this.colYCoord = transform.position.y;
        angle = Random.Range(-Mathf.PI, Mathf.PI);
        this.trail = new Queue<AntDroppedCell>();
        for (int i = 0; i < 8; i++)
        {
            trail.Enqueue(Instantiate(AntDroppedCellObj, this.transform.position, new Quaternion(), AntDroppedCellsParent).GetComponent<AntDroppedCell>());
        }
        this.transform.eulerAngles = Vector3.forward * Mathf.Rad2Deg * angle;
        InvokeRepeating("ChangeDir", 0, DirChangeTimer);
        InvokeRepeating("DropBreadCrumb", 0, 0.9f);
    }

    private void Awake()
    {
        foodLayerNumber = 1 << LayerMask.NameToLayer("foodLayer");
        foodTrailLayerNumber = 1 << LayerMask.NameToLayer("foodTrailLayer");

        AntDroppedCellsParent = GameObject.Find("AntDroppedCells").transform;

        AntDroppedCellObj = Resources.Load<GameObject>("Prefabs/AntDroppedCell");
    }

    private void DropBreadCrumb()
    {
        AntDroppedCell Trailcache = trail.Dequeue();
        Trailcache.ResetCell(System.Convert.ToByte(this.hasFood), this.transform.position);
        trail.Enqueue(Trailcache);
    }

    private float See()
    {
        if (goingBack)
        {
            if (CalcVectorLength(xPos, yPos, colXCoord, colYCoord) < 5f)
            {
                GotHome();
                return See();
            }
            if (breadCrumbs.Count > 0)
            {
                Vector2 breadCrumb = breadCrumbs.Pop();
                return CalcAngle(xPos, yPos, breadCrumb.x, breadCrumb.y);
            }
            else
            {
                return CalcAngle(xPos, yPos, colXCoord, colYCoord);
            }
        }

        float closestDist = float.MaxValue;
        float BestAngle = float.MinValue;
        Collider2D[] targets = Physics2D.OverlapCircleAll(this.transform.position, viewRange, foodLayerNumber);
        for (int i = 0; i < targets.Length; i++)
        {
            Vector2 closest = targets[i].transform.position;
            float enclosedAngle = CalcAngle(xPos, yPos, closest.x, closest.y);

            if (Mathf.Abs(angle - enclosedAngle) <= viewAngle / 2)
            {
                float tempDist = CalcVectorLength(closest.x, closest.y, xPos, yPos);
                if (tempDist < pickUpDist)
                {
                    Destroy(targets[i].gameObject);
                    PickedUpFood();
                    return See();
                }

                if (closestDist > tempDist)
                {
                    closestDist = tempDist;
                    BestAngle = enclosedAngle;
                }
            }
        }

        if (BestAngle != float.MinValue)
        {
            return BestAngle;
        }

        Collider2D[] foodTrails = Physics2D.OverlapCircleAll(this.transform.position, viewRange, foodTrailLayerNumber);

        for (int i = 0; i < foodTrails.Length; i++)
        {
            Vector2 closest = foodTrails[i].transform.position;
            float enclosedAngle = CalcAngle(xPos, yPos, closest.x, closest.y);
            float relativeAngle = angle - enclosedAngle;

            if (Mathf.Abs(relativeAngle) <= viewAngle / 2)
            {
                return enclosedAngle;
            }
        }

        /*   if (BestAngle != float.MinValue)
           {
               return BestAngle;
           }*/

        return this.angle += Random.Range(-0.4f, 0.4f);
    }

    private void ChangeDir()
    {
        if (breadCrumbs.Count == MaxDistance)
        {
            goingBack = true;
        }
        angle = See();

        if (angle >= Mathf.PI)
        {
            angle -= Mathf.PI * 2;
        }
        else if (angle <= -Mathf.PI)
        {
            angle += Mathf.PI * 2;
        }
        this.transform.eulerAngles = Vector3.forward * Mathf.Rad2Deg * angle;

        moveVector.x = speed * Mathf.Cos(angle);
        moveVector.y = speed * Mathf.Sin(angle);

        if (!goingBack)
            breadCrumbs.Push(this.transform.position);
    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;

        if (time >= 0.05f)
        {
            this.transform.position = new Vector3(this.transform.position.x + moveVector.x, this.transform.position.y + moveVector.y);

            time = 0;
        }
    }

    private void PickedUpFood()
    {
        this.hasFood = true;
        this.goingBack = true;
    }

    private void GotHome()
    {
        if (hasFood)
        {
            this.angle = (this.angle - Mathf.PI) % (Mathf.PI * 2);
            this.GetComponentInParent<Colony>().BroughtHomeFood();
        }

        this.hasFood = false;
        this.goingBack = false;
        this.breadCrumbs.Clear();
    }

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

    /// <summary>
    /// Calculates the length of the vector
    /// </summary>
    /// <param name="x1">x coord of the base of the vector</param>
    /// <param name="y1">y coord of the base of the vector</param>
    /// <param name="x2">x coord of the end of the vector</param>
    /// <param name="y2">y coord of the end of the vector</param>
    /// <returns>the angle</returns>
    private static float CalcVectorLength(float x1, float y1, float x2, float y2)
    {
        return Mathf.Sqrt(Mathf.Pow(x2 - x1, 2) + Mathf.Pow(y2 - y1, 2));
    }

    private void OnBecameInvisible()
    {
        angle += Mathf.PI;
    }
}