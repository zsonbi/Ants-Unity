using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodHandler : MonoBehaviour
{
    private float time = 0f;

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
}