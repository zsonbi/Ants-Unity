using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntSimulation : MonoBehaviour
{
    public int startingPop = 10;
    public List<Vector2> colonyPositions = new List<Vector2>();
    private GameObject colonyObj;

    // Start is called before the first frame update
    private void Start()
    {
        colonyObj = Resources.Load<GameObject>("Prefabs/Colony");
        foreach (var item in colonyPositions)
        {
            AddColony(item, startingPop);
        }
    }

    private void AddColony(Vector2 position, int popSize)
    {
        Colony colony = Instantiate(colonyObj, position, new Quaternion(), this.transform).GetComponent<Colony>();
        colony.AddAnts(popSize);
    }
}