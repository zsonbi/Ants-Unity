using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodHandler : MonoBehaviour
{
    public bool borderFood = true;
    private float time = 0f;

    private void Awake()
    {
        if (borderFood)
            FillBordersWithFood();
    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;
        if (time >= 0.1f)
        {
            if (Input.GetMouseButton(0))
            {
                GameObject food = new GameObject("food", typeof(SpriteRenderer), typeof(Food));

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                food.transform.position = new Vector3(mousePos.x, mousePos.y);
                food.transform.SetParent(this.transform);
            }
        }
    }

    private void FillBordersWithFood()
    {
        for (int i = 0; i < Screen.width; i++)
        {
            for (int j = 0; j < Screen.height; j++)
            {
                if ((i < 4 || i >= Screen.width - 4) || (j < 4 || j >= Screen.height - 4))
                {
                    GameObject food = new GameObject("food", typeof(SpriteRenderer), typeof(Food));

                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(i, j));
                    food.transform.position = new Vector3(mousePos.x, mousePos.y);
                    food.transform.SetParent(this.transform);
                }
            }
        }
    }
}