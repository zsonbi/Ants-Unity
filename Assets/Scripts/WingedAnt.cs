namespace AntSimulation
{
    internal class WingedAnt : Ant
    {
        public WingedAnt()
        {
            base.speed = SimulationOptions.Speed * 1.5f;
            base.health = 300f;
            base.attack = 10f;
            base.viewDistance = SimulationOptions.ViewDistance * 2f;
        }
    }
}