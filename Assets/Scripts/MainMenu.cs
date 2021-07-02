using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This object handles the main menu
/// </summary>
public class MainMenu : MonoBehaviour
{
    private Transform leftAnt; //Left ant in the main menu
    private Transform rightAnt; //Right ant in the main menu
    private float time = 0; //The time elapsed since last rotation

    //----------------------------------------------------------------------
    //Runs when the script is loaded
    private void Awake()
    {
        //Finds the ants transform so we can rotate them later
        leftAnt = this.gameObject.transform.Find("LeftAnt");
        rightAnt = this.gameObject.transform.Find("RightAnt");
    }

    //--------------------------------------------------------
    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;

        if (time >= 0.5f)
        {
            leftAnt.Rotate(180f, 0f, 0f);
            rightAnt.Rotate(180f, 0f, 0f);
            time = 0f;
        }
    }

    //----------------------------------------------------------------
    /// <summary>
    /// Loads in the simulation's scene
    /// </summary>
    public void StartSimulation()
    {
        SceneManager.LoadScene("Simulation", LoadSceneMode.Single);
    }

    //----------------------------------------------------------------
    /// <summary>
    /// Hides the main menu
    /// </summary>
    public void Options()
    {
        this.gameObject.SetActive(false);
    }

    //------------------------------------------------------------------
    /// <summary>
    /// Makes the main menu active
    /// </summary>
    public void Activate()
    {
        this.gameObject.SetActive(true);
    }

    //----------------------------------------------------------------------
    /// <summary>
    /// Exits the project
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }
}