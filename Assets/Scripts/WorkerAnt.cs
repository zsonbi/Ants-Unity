using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntSimulation
{
    internal class WorkerAnt : Ant
    {
        public WorkerAnt()
        {
            base.Speed = SimulationOptions.Speed;
            base.maxHealth = 100f;
            base.Attack = 2f;
            base.maxHunger = SimulationOptions.MaxHunger * SimulationOptions.WorkerAntHungerMultiplier;
        }
    }
}