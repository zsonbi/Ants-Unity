using UnityEngine;
using AntSimulation;
using UnityEngine.UI;

/// <summary>
/// This object handles the options for the simulation
/// </summary>
public class Options : MonoBehaviour
{
    //Runs when the script is loaded
    private void Awake()
    {
        this.gameObject.SetActive(false);
    }

    //------------------------------------------------------------------------------------
    /// <summary>
    /// Deactivates the object
    /// </summary>
    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }

    //-------------------------------------------------------------------------------------
    /// <summary>
    /// Activates the object
    /// </summary>
    public void Activate()
    {
        this.gameObject.SetActive(true);
    }

    //-------------------------------------------------------------------------------------
    /// <summary>
    /// Updates the starting population size in the simulation and updates the label
    /// </summary>
    /// <param name="numberOfAnts">the number of ants</param>
    public void StartingPopSizeSliderChanged(float numberOfAnts)
    {
        AntSimulation.AntSimulation.startingPop = System.Convert.ToInt32(numberOfAnts);
        (this.transform.Find("StartingPopNumberText").transform.Find("PopNumber").gameObject).GetComponent<Text>().text = numberOfAnts.ToString();
    }

    //---------------------------------------------------------------------------------------
    /// <summary>
    /// Updates the number of breadcrumbs each ant leave in the simulation and updates the label
    /// </summary>
    /// <param name="numberOfBreadCrumbs">the number of breadcrumbs</param>
    public void BreadCrumbsSliderChanged(float numberOfBreadCrumbs)
    {
        AntSimulation.Ant.numberOfBreadCrumbs = (byte)numberOfBreadCrumbs;
        (this.transform.Find("BreadCrumbsNumberText").transform.Find("BreadCrumbsNumber").gameObject).GetComponent<Text>().text = numberOfBreadCrumbs.ToString();
    }

    //---------------------------------------------------------------------------------------
    /// <summary>
    /// Updates the ants pick up distance and updates the label
    /// </summary>
    /// <param name="pickUpDist">the radius</param>
    public void PickUpDistanceSliderChanged(float pickUpDist)
    {
        AntSimulation.Ant.pickUpDist = pickUpDist;
        (this.transform.Find("PickUpDistanceText").transform.Find("PickUpDistanceNumber").gameObject).GetComponent<Text>().text = pickUpDist.ToString();
    }

    //---------------------------------------------------------------------------------------
    /// <summary>
    /// Updates the ants speed and updates the label
    /// </summary>
    /// <param name="speed">the speed</param>
    public void AntSpeedSliderChanged(float speed)
    {
        AntSimulation.Ant.speed = speed;
        (this.transform.Find("AntSpeedText").transform.Find("AntSpeedNumber").gameObject).GetComponent<Text>().text = speed.ToString();
    }

    //---------------------------------------------------------------------------------------
    /// <summary>
    /// Updates the ants view distance and updates the label
    /// </summary>
    /// <param name="viewDist">the view distance</param>
    public void AntViewDistanceSliderChanged(float viewDist)
    {
        AntSimulation.Ant.viewDistance = viewDist;
        (this.transform.Find("ViewDistanceText").transform.Find("ViewDistanceNumber").gameObject).GetComponent<Text>().text = viewDist.ToString();
    }

    //---------------------------------------------------------------------------------------
    /// <summary>
    /// Updates the ants view angle and updates the label
    /// </summary>
    /// <param name="viewAngle">the angle (in radian)</param>
    public void AntViewAngleSliderChanged(float viewAngle)
    {
        AntSimulation.Ant.viewAngle = viewAngle;
        (this.transform.Find("ViewAngleText").transform.Find("ViewAngleNumber").gameObject).GetComponent<Text>().text = viewAngle.ToString();
    }

    //---------------------------------------------------------------------------------------
    /// <summary>
    /// Updates the ants direction change timer parameter and updates the label
    /// </summary>
    /// <param name="dirChangeTime">the time between the direction changes for the ants</param>
    public void AntDirectionChangeSliderChanged(float dirChangeTime)
    {
        AntSimulation.Ant.DirChangeTimer = dirChangeTime;
        (this.transform.Find("AntDirChangePeriodTimeText").transform.Find("viewAngleNumber").gameObject).GetComponent<Text>().text = dirChangeTime.ToString();
    }

    //---------------------------------------------------------------------------------------
    /// <summary>
    /// Should the colonies spawn new ants when food is brought back
    /// </summary>
    /// <param name="isChecked">should colonies spawn new ants</param>
    public void ShouldSpawnAntWhenFoodIsBroughtHome(bool isChecked)
    {
        Colony.SpawnAntWhenFoodIsBroughtHome = isChecked;
    }
}