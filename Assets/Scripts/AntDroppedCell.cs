using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntDroppedCell : MonoBehaviour
{
    protected short timeToLive = maxTimeToLive;
    protected Color color;
    private float time;
    public static readonly byte maxTimeToLive = 15;
    private static int foodTrailLayer;
    private SpriteRenderer spriteRendererCache;
    private CircleCollider2D circleColliderCache;

    public float xPos { get => this.transform.position.x; }
    public float yPos { get => this.transform.position.y; }

    private void Awake()
    {
        foodTrailLayer = LayerMask.NameToLayer("foodTrailLayer");
        this.gameObject.SetActive(false);
        spriteRendererCache = this.GetComponent<SpriteRenderer>();
        circleColliderCache = this.GetComponent<CircleCollider2D>();
        circleColliderCache.enabled = false;
    }

    private void Start()
    {
        InvokeRepeating("Decay", 0, 0.5f);
    }

    private void Decay()
    {
        if (!this.gameObject.activeSelf)
            return;

        timeToLive--;
        this.color.a = timeToLive / (float)maxTimeToLive;
        spriteRendererCache.color = this.color;
        if (timeToLive <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    /// <summary>
    /// Resets the cell
    /// </summary>
    /// <param name="type">0-breadCrumb, 1-foodTrail</param>
    public void ResetCell(byte type, Vector2 newPos)
    {
        switch (type)
        {
            //BreadCrumb
            case 0:
                this.color = new Color(0f, 0f, 1f, 1f);
                this.gameObject.layer = 0;
                circleColliderCache.enabled = false;
                break;

            //FoodTrail
            case 1:
                this.color = new Color(1f, 0f, 0f, 1f);
                this.gameObject.layer = foodTrailLayer;
                circleColliderCache.enabled = true;
                break;

            default:
                break;
        }
        timeToLive = maxTimeToLive;
        this.transform.position = new Vector3(newPos.x, newPos.y, 15);
        this.gameObject.SetActive(true);
        spriteRendererCache.color = color;
    }
}