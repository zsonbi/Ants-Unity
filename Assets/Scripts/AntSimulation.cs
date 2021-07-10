using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AntSimulation
{
    /// <summary>
    /// Simulation object
    /// </summary>
    public class AntSimulation : MonoBehaviour
    {
        public static List<Vector2> colonyPositions = new List<Vector2>(new List<Vector2> { new Vector2(-100f, 0f), new Vector2(100f, 0f) }); //The positions of the colonies

        private static GameObject colonyObj; //The colony prefab

        private sbyte nextID = 0; //The next colonyID

        //------------------------------------------------------------------------------
        //Runs when the script is loaded
        private void Awake()
        {
            //Loads the prefab
            colonyObj = Resources.Load<GameObject>("Prefabs/Colony");
        }

        //---------------------------------------------------------
        // Start is called before the first frame update
        private void Start()
        {
            StartSimulation();
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Loads the main menu's scene
        /// </summary>
        public void BackToMainMenu()
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Starts the simulation (creates the colonies)
        /// </summary>
        public void StartSimulation()
        {
            //Adds each colony
            foreach (var item in colonyPositions)
            {
                this.AddColony(item, SimulationOptions.StartingPopulation);
            }
        }

        //-------------------------------------------------------------------------------
        //Adds a new colony to the simulation
        private void AddColony(Vector2 position, int popSize)
        {
            Colony colony = Instantiate(colonyObj, position, new Quaternion(), this.transform).GetComponent<Colony>();
            colony.colonyID = nextID++;
            nextID %= SimulationOptions.NumberOfTeams;
            colony.AddAnts(popSize);
        }
    }
}