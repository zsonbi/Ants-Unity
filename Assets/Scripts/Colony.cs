using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colony : MonoBehaviour
{
    public short AntCount = 0;
    public bool SpawnAntWhenFoodIsBroughtHome = false;

    public float XPos { get => this.transform.position.x; }
    public float YPos { get => this.transform.position.y; }

    private static Sprite brownAntSprite;
    private static GameObject antObj;

    private void Awake()
    {
        antObj = Resources.Load<GameObject>("Prefabs/Ant");
    }

    public void AddAnts(int numberOfAnts)
    {
        for (int i = 0; i < numberOfAnts; i++)
        {
            AddAnt();
        }
    }

    private void AddAnt()
    {
        Instantiate(antObj, this.transform.position, new Quaternion(), this.transform);

        this.AntCount++;
    }

    public void BroughtHomeFood()
    {
        if (SpawnAntWhenFoodIsBroughtHome)
            AddAnt();
    }
}