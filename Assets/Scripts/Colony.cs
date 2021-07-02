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
        /// Should the colony spawn a new ant when food is brought home
        /// </summary>
        public static bool SpawnAntWhenFoodIsBroughtHome = false;

        /// <summary>
        /// The x coordinate of the colony
        /// </summary>
        public float XPos { get => this.transform.position.x; }

        /// <summary>
        /// The y coordinate of the colony
        /// </summary>
        public float YPos { get => this.transform.position.y; }

        private static GameObject antObj; //Ant prefab

        //-----------------------------------------------------
        //Runs when the script is loaded
        private void Awake()
        {
            //Loads in the ant prefab
            antObj = Resources.Load<GameObject>("Prefabs/Ant");
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
            Instantiate(antObj, this.transform.position, new Quaternion(), this.transform);

            this.AntCount++;
        }

        //-------------------------------------------------------------
        /// <summary>
        ///Should be only called when an ant brings home food
        /// </summary>
        internal void BroughtHomeFood()
        {
            if (SpawnAntWhenFoodIsBroughtHome)
                AddAnt();
        }
    }
}