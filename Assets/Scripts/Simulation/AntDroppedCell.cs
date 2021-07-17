using System.Collections.Generic;
using UnityEngine;

namespace AntSimulation
{
    /// <summary>
    /// This object is both the foodTrail and the breadCrumb depending on the type which is set in the ResetCell
    /// </summary>
    internal class AntDroppedCell : MonoBehaviour
    {
        //Constants
        public static readonly byte maxTimeToLive = 15; //The time this poor cell can live

        //Cache

        private SpriteRenderer spriteRendererCache;
        private CircleCollider2D circleColliderCache;

        //Private varriables

        private short timeToLive = maxTimeToLive; //The remaining time to live
        private Color color; //The color of the cell
        private static int foodTrailLayerNumber; //The foodTrailLayer's number

        /// <summary>
        /// Gets the previous foodTrails positions
        /// </summary>
        public List<Vector2> prevTrailPositions { get; private set; }

        /// <summary>
        /// Returns the x pos of the cell
        /// </summary>
        public float XPos { get => this.transform.position.x; }

        /// <summary>
        /// Returns the y pos of the cell
        /// </summary>
        public float YPos { get => this.transform.position.y; }

        //-----------------------------------------------------
        //Runs when the script is loaded
        private void Awake()
        {
            foodTrailLayerNumber = LayerMask.NameToLayer("foodTrailLayer");
            this.gameObject.SetActive(false);
            this.spriteRendererCache = this.GetComponent<SpriteRenderer>();
            this.circleColliderCache = this.GetComponent<CircleCollider2D>();
            this.circleColliderCache.enabled = false;
        }

        //-----------------------------------------------------------------
        // Start is called before the first frame update
        private void Start()
        {
            InvokeRepeating("Decay", 0, 0.5f);
        }

        //-------------------------------------------------------------
        //Decays the cell and fades it's color when timeToLive reaches 0 kills the cell
        private void Decay()
        {
            if (!this.gameObject.activeSelf)
                return;

            this.timeToLive--;
            this.color.a = timeToLive / (float)maxTimeToLive;
            this.spriteRendererCache.color = this.color;
            if (this.timeToLive <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Resets the cell
        /// </summary>
        /// <param name="type">0-breadCrumb, 1-foodTrail</param>
        public void ResetCell(byte type, Vector2 newPos, List<Vector2> prevTrailPositions = null)
        {
            switch (type)
            {
                //BreadCrumb
                case 0:
                    this.color = new Color(0f, 0f, 1f, 1f);
                    this.gameObject.layer = 0;
                    this.circleColliderCache.enabled = false;
                    this.timeToLive = (short)(maxTimeToLive);
                    //Should it show up
                    if (SimulationOptions.DropBreadCrumbs)
                        this.gameObject.SetActive(true);
                    break;

                //FoodTrail
                case 1:
                    this.color = new Color(1f, 0f, 0f, 1f);
                    this.gameObject.layer = foodTrailLayerNumber;
                    this.circleColliderCache.enabled = true;
                    this.prevTrailPositions = new List<Vector2>();
                    foreach (var item in prevTrailPositions)
                    {
                        this.prevTrailPositions.Add(item);
                    }
                    this.timeToLive = maxTimeToLive;
                    this.gameObject.SetActive(true);
                    break;

                default:
                    break;
            }

            this.transform.position = new Vector3(newPos.x, newPos.y, 15);

            this.spriteRendererCache.color = color;
        }
    }
}