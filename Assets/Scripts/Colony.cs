using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colony : MonoBehaviour
{
    public short AntCount { get; private set; }

    public float XPos { get => this.transform.position.x; }
    public float YPos { get => this.transform.position.y; }

    private static Sprite brownAntSprite;

    private void Awake()
    {
        brownAntSprite = Resources.Load<Sprite>("brownAnt");
    }

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < 500; i++)
        {
            AddAnt();
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void AddAnt()
    {
        GameObject ant = new GameObject("ant", typeof(SpriteRenderer), typeof(Ant));
        ant.GetComponent<SpriteRenderer>().sprite = brownAntSprite;
        ant.transform.position = new Vector3(XPos, YPos);
        ant.AddComponent<BoxCollider2D>();
        ant.transform.SetParent(this.transform);
        this.AntCount++;
    }
}