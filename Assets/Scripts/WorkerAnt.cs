using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntSimulation
{
    internal class WorkerAnt : Ant
    {
        public WorkerAnt()
        {
            base.speed = SimulationOptions.Speed;
            base.health = 100f;
            base.attack = 2f;
        }
    }
}