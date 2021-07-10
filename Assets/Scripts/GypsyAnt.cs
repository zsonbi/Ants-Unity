using UnityEngine;

namespace AntSimulation
{
    /// <summary>
    /// If it asks for money then run
    /// It has a lot of attack, but doesn't work
    /// </summary>
    internal class GypsyAnt : Ant
    {
        private int otherAntsLayersMask = 0; //The other layers where could be enemies

        //Constructor -,-
        public GypsyAnt()
        {
            base.Speed = SimulationOptions.Speed * 0.7f;
            base.maxHealth = 200f;
            base.Attack = 20f;
            base.maxHunger = SimulationOptions.MaxHunger * SimulationOptions.GypsyAntHungerMultiplier;
        }

        //-----------------------------------------------------------------------
        //Overrides the see method so it seeks enemies instead of food
        protected override float See()
        {
            Collider2D[] nearbyAnts = Physics2D.OverlapCircleAll(this.transform.position, SimulationOptions.ViewDistance, otherAntsLayersMask);
            float closestEnemy = float.MaxValue;
            float bestAngle = this.lookingDirection;
            for (int i = 0; i < nearbyAnts.Length; i++)
            {
                Ant ant = nearbyAnts[i].GetComponent<Ant>();

                float distance = CalcVectorLength(nearbyAnts[i].transform.position.x, nearbyAnts[i].transform.position.y, this.XPos, this.YPos);

                //Attack the first ant which comes in attack range and moves towards it
                if (distance <= SimulationOptions.AttackRange)
                {
                    ant.TakeDamage(this.Attack);
                    this.TakeDamage(ant.Attack); //Those who live by the sword, die by the sword
                    return CalcAngle(this.XPos, this.YPos, nearbyAnts[i].transform.position.x, nearbyAnts[i].transform.position.y);
                }
                else if (closestEnemy > distance)
                {
                    closestEnemy = distance;
                    bestAngle = CalcAngle(this.XPos, this.YPos, nearbyAnts[i].transform.position.x, nearbyAnts[i].transform.position.y);
                }
            }
            if (closestEnemy != float.MaxValue)
                return bestAngle;
            else if (base.IsGoingBack)
                return GetDirectionToHome();
            else
                return base.lookingDirection += UnityEngine.Random.Range(-0.4f, 0.4f);
        }

        //-----------------------------------------------------------------
        /// <summary>
        /// Sets the ant's colony id to the number
        /// </summary>
        /// <param name="colID">the id</param>
        /// <exception cref="Exception">When trying to set it again</exception>
        public override void SetColony(sbyte colID)
        {
            base.SetColony(colID);

            //Please for the love of god don't touch this
            //Takes out the first 8 layers and this ant's layer from the mask
            this.otherAntsLayersMask = ~((1 << (9 + colID)) + 511);
        }
    }
}