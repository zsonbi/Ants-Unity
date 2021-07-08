using UnityEngine;

namespace AntSimulation
{
    /// <summary>
    /// Controls the whole colony
    /// </summary>
    internal class Colony : MonoBehaviour
    {
        public short colonyID;

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

        public float FoodReserve { get; private set; }

        private static GameObject workerAntObj; //Ant prefab
        private static GameObject gypsyAntObj;
        private static GameObject wingedAntObj;

        //-----------------------------------------------------
        //Runs when the script is loaded
        private void Awake()
        {
            //Loads in the ant prefab
            workerAntObj = Resources.Load<GameObject>("Prefabs/WorkerAnt");
            gypsyAntObj = Resources.Load<GameObject>("Prefabs/GypsyAnt");
            wingedAntObj = Resources.Load<GameObject>("Prefabs/WingedAnt");
            FoodReserve = SimulationOptions.DefaultColonyFoodReserveSize;
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
            Ant ant;
            float randFloat = Random.Range(0.0f, 1.0f);

            if (randFloat <= SimulationOptions.GypsyRate)
            {
                ant = Instantiate(gypsyAntObj, this.transform.position, new Quaternion(), this.transform).GetComponent<Ant>();
            }
            else if (randFloat <= SimulationOptions.WingedRate + SimulationOptions.GypsyRate)
            {
                ant = Instantiate(wingedAntObj, this.transform.position, new Quaternion(), this.transform).GetComponent<Ant>();
            }
            else
            {
                ant = Instantiate(workerAntObj, this.transform.position, new Quaternion(), this.transform).GetComponent<Ant>();
            }
            ant.SetColony(this.colonyID);
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

        //--------------------------------------------------------------
        internal float TakeFood(float amount)
        {
            if (FoodReserve <= 0)
                return -0.1f;
            FoodReserve -= amount;
            return 1f;
        }
    }
}