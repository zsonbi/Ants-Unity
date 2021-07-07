using UnityEngine;

namespace AntSimulation
{
    internal class GypsyAnt : Ant
    {
        public GypsyAnt()
        {
            base.speed = SimulationOptions.Speed * 0.7f;
            base.health = 200f;
            base.attack = 20f;
        }

        protected override float See()
        {
            Collider2D[] nearbyAnts = Physics2D.OverlapCircleAll(this.transform.position, SimulationOptions.ViewDistance, antLayerMask);
            float closestEnemy = float.MaxValue;
            float bestAngle = this.lookingDirection;
            for (int i = 0; i < nearbyAnts.Length; i++)
            {
                Ant ant = nearbyAnts[i].GetComponent<Ant>();

                if (ant.ColonyID != this.ColonyID)
                {
                    float distance = CalcVectorLength(nearbyAnts[i].transform.position.x, nearbyAnts[i].transform.position.y, this.XPos, this.YPos);

                    if (distance <= SimulationOptions.AttackRange)
                    {
                        ant.UnderAttack(this.attack);
                        this.UnderAttack(ant.attack);
                        return CalcAngle(this.XPos, this.YPos, nearbyAnts[i].transform.position.x, nearbyAnts[i].transform.position.y);
                    }
                    else if (closestEnemy > distance)
                    {
                        closestEnemy = distance;
                        bestAngle = CalcAngle(this.XPos, this.YPos, nearbyAnts[i].transform.position.x, nearbyAnts[i].transform.position.y);
                    }
                }
            }
            if (closestEnemy != float.MaxValue)
                return bestAngle;
            else
                return base.lookingDirection += UnityEngine.Random.Range(-0.4f, 0.4f);
        }
    }
}