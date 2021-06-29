using UnityEngine;

public class BreadCrumb : MonoBehaviour
{
    protected short timeToLive = 30;
    protected Color color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
    private float time;

    private static Sprite circle;

    public float xPos { get => this.transform.position.x; }
    public float yPos { get => this.transform.position.y; }

    private void Start()
    {
        circle = Resources.Load<Sprite>("Circle");
        this.gameObject.GetComponent<SpriteRenderer>().color = color;
        this.GetComponent<SpriteRenderer>().sprite = circle;
        this.gameObject.isStatic = true;
    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;

        if (time >= 0.3f)
        {
            timeToLive--;
            color.a = (float)timeToLive / 30.0f;
            this.GetComponent<SpriteRenderer>().color = color;
            if (timeToLive <= 0)
            {
                Destroy(this.gameObject);
            }

            time = 0;
        }
    }
}