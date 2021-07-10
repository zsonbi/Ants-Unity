namespace AntSimulation
{
    /// <summary>
    /// The good working class of society white skinned etc. (well ants are black now but who cares)
    /// </summary>
    internal class WorkerAnt : Ant
    {
        //Constructor
        public WorkerAnt()
        {
            base.Speed = SimulationOptions.Speed;
            base.maxHealth = 100f;
            base.Attack = 2f;
            base.maxHunger = SimulationOptions.MaxHunger * SimulationOptions.WorkerAntHungerMultiplier;
        }
    }
}