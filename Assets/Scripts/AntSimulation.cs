using System.Collections.Generic;
using UnityEngine;

namespace AntSimulation
{
    /// <summary>
    /// Simulation object
    /// </summary>
    public class AntSimulation : MonoBehaviour
    {
        public int startingPop = 10; //The starting number of ants
        public List<Vector2> colonyPositions = new List<Vector2>(); //The positions of the colonies

        private static GameObject colonyObj; //The colony prefab

        //------------------------------------------------------------------------------
        // Start is called before the first frame update
        private void Start()
        {
            //Loads the prefab
            colonyObj = Resources.Load<GameObject>("Prefabs/Colony");
            //Adds each colony
            foreach (var item in colonyPositions)
            {
                this.AddColony(item, startingPop);
            }
        }

        //-------------------------------------------------------------------------------
        //Adds a new colony to the simulation
        private void AddColony(Vector2 position, int popSize)
        {
            Colony colony = Instantiate(colonyObj, position, new Quaternion(), this.transform).GetComponent<Colony>();

            colony.AddAnts(popSize);
        }
    }
}