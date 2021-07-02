using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This object handles the colonies setup in the options menu
/// </summary>
public class ColonySetup : MonoBehaviour
{
    private List<GameObject> colonies = new List<GameObject>(); //Stores the colonies so we can destroy them later
    private static Sprite circle; //Load in a circle which will be the colonies sprite

    //-----------------------------------------------------------
    //Runs when the script is loaded
    private void Awake()
    {
        circle = Resources.Load<Sprite>("circle");
        //Deactivate this object so it doesn't wreak in the menu scene
        this.gameObject.SetActive(false);
    }

    //------------------------------------------------------------
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnColony(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    //-----------------------------------------------------------------
    //Spawns a colony at the position
    private void SpawnColony(Vector2 position)
    {
        GameObject colony = new GameObject("colonyLocation", typeof(SpriteRenderer));
        colony.transform.position = new Vector3(position.x, position.y, 10);
        colony.GetComponent<SpriteRenderer>().sprite = circle;
        colony.GetComponent<SpriteRenderer>().color = new Color(0.21f, 0.06f, 0.07f, 1f);
        colony.transform.localScale = new Vector3(20.0f, 20.0f, 1.0f);
        colonies.Add(colony);
    }

    //----------------------------------------------------------------
    /// <summary>
    /// Activates the object ant loads in the stored colonies in the AntSimulation object
    /// </summary>
    public void Activate()
    {
        this.gameObject.SetActive(true);
        foreach (var item in AntSimulation.AntSimulation.colonyPositions)
        {
            SpawnColony(item);
        }
    }

    //-------------------------------------------------------------
    /// <summary>
    /// Saves the colonies and deactivate the object
    /// </summary>
    public void SaveAndClose()
    {
        for (int i = 0; i < colonies.Count - 1; i++)
        {
            AntSimulation.AntSimulation.colonyPositions.Add(colonies[i].transform.position);
        }
        CloseSetup();
    }

    //------------------------------------------------------------------
    /// <summary>
    /// Resets the colonies in the simulation and deletes it grom the screen
    /// </summary>
    public void ResetColonies()
    {
        AntSimulation.AntSimulation.colonyPositions.Clear();
        ClearWindow();
    }

    //--------------------------------------------------------------
    /// <summary>
    /// Clears the window of the colonies
    /// </summary>
    private void ClearWindow()
    {
        for (int i = 0; i < colonies.Count; i++)
        {
            Destroy(colonies[i]);
        }
        colonies.Clear();
    }

    //---------------------------------------------------------------
    /// <summary>
    /// Deactivates the Colonysetup sequence
    /// </summary>
    public void CloseSetup()
    {
        ClearWindow();
        this.gameObject.SetActive(false);
    }
}