using UnityEngine;
using UnityEngine.UI;

namespace AntSimulation
{
    /// <summary>
    /// Controls the whole colony
    /// </summary>
    internal class Colony : MonoBehaviour
    {
        private float foodAmount; //The amount of food it has stored
        private short populatioSize = 0; //The size of the population

        /// <summary>
        /// The ID of the colony
        /// </summary>
        public sbyte colonyID;

        /// <summary>
        /// The number of ants the colony has
        /// </summary>
        internal short AntCount { get => populatioSize; set { populatioSize = value; populationSizeText.text = populatioSize.ToString(); } }

        /// <summary>
        /// The amount of food it has left and updates the label of the colony
        /// </summary>
        private float FoodReserve { get => foodAmount; set { foodAmount = value; foodAmountText.text = Mathf.Round(foodAmount).ToString(); } }

        //Cache to the labels

        public Text populationSizeText;
        public Text foodAmountText;

        /// <summary>
        /// The x coordinate of the colony
        /// </summary>
        public float XPos { get => this.transform.position.x; }

        /// <summary>
        /// The y coordinate of the colony
        /// </summary>
        public float YPos { get => this.transform.position.y; }

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
            ant.gameObject.layer = 9 + colonyID;
            this.AntCount++;
        }

        //-------------------------------------------------------------
        /// <summary>
        ///Should be only called when an ant brings home food
        /// </summary>
        internal void BroughtHomeFood()
        {
            this.FoodReserve += 4f;
            if (SimulationOptions.SpawnAntWhenFoodIsBroughtHome && this.FoodReserve > populatioSize * 4)
            {
                AddAnt();
                FoodReserve -= 9f;
            }
        }

        //--------------------------------------------------------------
        /// <summary>
        /// Take food from the colony
        /// </summary>
        /// <param name="amount">the amount the ant took</param>
        /// <returns>the fraction of the maxHunger it can fill itself up</returns>
        internal float TakeFood(float amount)
        {
            if (FoodReserve <= 0)
                return -0.1f;
            FoodReserve -= amount;
            return 1f;
        }
    }
}