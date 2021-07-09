namespace AntSimulation
{
    /// <summary>
    /// A fucking winged ant be carful it can even attack your eye
    /// it just moves faster and sees farther away then the normal ant
    /// also is a bit better at combat
    /// </summary>
    internal class WingedAnt : Ant
    {
        //Constructor -,-
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