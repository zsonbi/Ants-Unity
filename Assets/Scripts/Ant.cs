using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    private float time = 0f;
    private float angle = 0f;
    private byte counter = 0;
    private float colXCoord;
    private float colYCoord;

    private static readonly float pickUpDist = 3f;
    private static float speed = 1.0f;
    private static float viewRange = 10f;
    private static float viewAngle = 2f;
    private static int layerNum;
    private Stack<Vector2> breadCrumbs = new Stack<Vector2>();
    private static Transform breadCrumbsParent;

    public static byte DirChangeTimer = 20;

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
    }

    private void Awake()
    {
        layerNum = 1 << LayerMask.NameToLayer("foodLayer");

        breadCrumbsParent = GameObject.Find("BreadCrumbs").transform;
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
        Collider2D[] targets = Physics2D.OverlapCircleAll(this.transform.position, viewRange, layerNum);
        for (int i = 0; i < targets.Length; i++)
        {
            Vector2 closest = targets[i].ClosestPoint(this.transform.position);
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

        return this.angle += Random.Range(-0.4f, 0.4f);
    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;

        if (counter % DirChangeTimer == DirChangeTimer - 1)
        {
            angle = See();
            angle %= Mathf.PI * 2;
            this.transform.eulerAngles = Vector3.forward * Mathf.Rad2Deg * angle;
            if (!this.goingBack)
            {
                GameObject breadCrumb = new GameObject("breadCrumb", typeof(SpriteRenderer), typeof(BreadCrumb));
                breadCrumb.transform.position = new Vector3(xPos, yPos);
                breadCrumb.AddComponent<BoxCollider2D>();
                breadCrumbs.Push(new Vector2(xPos, yPos));
                breadCrumb.transform.SetParent(breadCrumbsParent);
            }
            else if (hasFood)
            {
                GameObject FoodTrail = new GameObject("FoodTrail", typeof(SpriteRenderer), typeof(FoodTrail));
                FoodTrail.transform.position = new Vector3(xPos, yPos);
            }

            counter = 0;
        }

        if (time >= 0.10f)
        {
            this.transform.position = new Vector3(this.transform.position.x + speed * Mathf.Cos(angle), this.transform.position.y + speed * Mathf.Sin(angle));

            time = 0;
            counter++;
        }
    }

    private void PickedUpFood()
    {
        this.hasFood = true;
        this.goingBack = true;
    }

    private void GotHome()
    {
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
}