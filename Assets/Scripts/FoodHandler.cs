using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntSimulation
{
    public class FoodHandler : MonoBehaviour
    {
        public bool borderFood = true; //Should the border be filled with food

        private float time = 0f; //A time varriable so the user can't spawn endless amount of food

        private static GameObject foodPrefab; //Food prefab

        private static Transform foodHandlerTransForm;

        //-----------------------------------------------------
        //Runs when the script is loaded
        private void Awake()
        {
            //Load food prefab
            foodPrefab = Resources.Load<GameObject>("Prefabs/Food");
            if (borderFood)
                FillBordersWithFood();
            foodHandlerTransForm = this.transform;
        }

        //--------------------------------------------------------
        // Update is called once per frame
        private void Update()
        {
            time += Time.deltaTime;
            if (time >= 0.1f)
            {
                if (Input.GetMouseButton(0))
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Instantiate(foodPrefab, mousePos, new Quaternion(), this.transform);
                }
            }
        }

        internal static void AddFood(float xPos, float yPos)
        {
            Instantiate(foodPrefab, new Vector2(xPos, yPos), new Quaternion(), foodHandlerTransForm);
        }

        //---------------------------------------------------------------
        //Fills the border with food
        private void FillBordersWithFood()
        {
            for (int i = 0; i < Screen.width; i++)
            {
                for (int j = 0; j < Screen.height; j++)
                {
                    if ((i < 2 || i >= Screen.width - 2) || (j < 2 || j >= Screen.height - 2))
                    {
                        Vector2 convertedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Instantiate(foodPrefab, convertedPos, new Quaternion(), this.transform);
                    }
                }
            }
        }
    }
}