namespace AntSimulation
{
    internal class WingedAnt : Ant
    {
        public WingedAnt()
        {
            base.Speed = SimulationOptions.Speed * 1.5f;
            base.maxHealth = 200f;
            base.Attack = 10f;
            base.viewDistance = SimulationOptions.ViewDistance * 2f;
            base.maxHunger = SimulationOptions.MaxHunger * SimulationOptions.WingedAntHungerMultiplier;
        }
    }
}