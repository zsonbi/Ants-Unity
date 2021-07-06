using UnityEngine;

namespace AntSimulation
{
    /// <summary>
    /// Controls the whole colony
    /// </summary>
    internal class Colony : MonoBehaviour
    {
        /// <summary>
        /// The number of ants the colony has
        /// </summary>
        public short AntCount = 0;

        /// <summary>
        /// The x coordinate of the colony
        /// </summary>
        public float XPos { get => this.transform.position.x; }

        /// <summary>
        /// The y coordinate of the colony
        /// </summary>
        public float YPos { get => this.transform.position.y; }

        private static GameObject workerAntObj; //Ant prefab
        private static GameObject gypsyAntObj; //Ant prefab

        //-----------------------------------------------------
        //Runs when the script is loaded
        private void Awake()
        {
            //Loads in the ant prefab
            workerAntObj = Resources.Load<GameObject>("Prefabs/Ant");
            gypsyAntObj = Resources.Load<GameObject>("Prefabs/GypsyAnt");
        }

        //------------------------------------------------------
        /// <summary>
        /// Adds a number of ants to the colony
        /// </summary>
        /// <param name="numberOfAnts">the number it should spawn</param>
        public void AddAnts(int numberOfAnts)
        {
            for (int i = 0; i < numberOfAnts; i++)
            {
                AddAnt();
            }
        }

        //-------------------------------------------------------------
        //Adds a single ant to the colony
        private void AddAnt()
        {
            if (Random.Range(0.0f, 1.0f) <= SimulationOptions.GypsyRate)
            {
                Instantiate(gypsyAntObj, this.transform.position, new Quaternion(), this.transform);
            }
            else
            {
                Instantiate(workerAntObj, this.transform.position, new Quaternion(), this.transform);
            }
            this.AntCount++;
        }

        //-------------------------------------------------------------
        /// <summary>
        ///Should be only called when an ant brings home food
        /// </summary>
        internal void BroughtHomeFood()
        {
            if (SimulationOptions.SpawnAntWhenFoodIsBroughtHome)
                AddAnt();
        }
    }
}