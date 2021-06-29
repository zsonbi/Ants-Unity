using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private static Color foodColor = new Color(0f, 1f, 0f, 1f);
    private static Sprite circle;

    public float xPos { get => this.transform.position.x; }
    public float yPos { get => this.transform.position.y; }

    public Food()
    {
    }

    private void Awake()
    {
        circle = Resources.Load<Sprite>("Circle");
    }

    // Start is called before the first frame update
    private void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = circle;
        this.GetComponent<SpriteRenderer>().color = foodColor;
        this.gameObject.layer = LayerMask.NameToLayer("foodLayer");
        this.gameObject.AddComponent<BoxCollider2D>();
        this.gameObject.isStatic = true;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}