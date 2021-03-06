namespace AntSimulation
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    internal class SliderAttribute : System.Attribute
    {
    }

    [System.AttributeUsage(System.AttributeTargets.Field)]
    internal class CheckBoxAttribute : System.Attribute
    {
    }

    /// <summary>
    /// Stores the settings values for the simulation
    /// </summary>
    internal static class SimulationOptions
    {
        //Constants

        internal static readonly float WingedAntHungerMultiplier = 1.5f;
        internal static readonly float WorkerAntHungerMultiplier = 2f;
        internal static readonly float GypsyAntHungerMultiplier = 1.6f;
        internal static readonly float DefaultColonyFoodReserveSize = 500f;
        internal static readonly float MaxHunger = 2000f;

        //Changeable parameters

        [Slider]
        internal static int MaxDistance = 300; //The number of breadCrumbs the ant should leave before tracing them back

        [Slider]
        internal static byte NumberOfBreadCrumbs = 8; //The number of breadCrumb object the ant should have

        [Slider]
        internal static float PickUpDist = 3f; //The range of the food pickup

        [Slider]
        internal static float AttackRange = 5f; //The attack range of the gypsies

        [Slider]
        internal static float Speed = 0.5f; //The speed of the ant (The length of the vector)

        [Slider]
        internal static float ViewDistance = 20f; //The distance of the ant detection

        [Slider]
        internal static float ViewAngle = 1.5f; //The angle which the ant can see (in radian)

        [Slider]
        internal static float DirChangeTimer = 0.2f; //The seconds there should be between the direction changes

        [Slider]
        internal static int StartingPopulation = 100; //The starting number of ants

        [Slider]
        internal static float GypsyRate = 0.3f; //The rate which the gypsies are spawned

        [Slider]
        internal static float WingedRate = 0.1f; //The rate which the winged ants are spawned

        [Slider]
        internal static sbyte NumberOfTeams = 2; //The number of teams the ants will have

        [CheckBox]
        internal static bool SpawnAntWhenFoodIsBroughtHome = true; //Should the colonies spawn new ants when got the food

        [CheckBox]
        internal static bool DropBreadCrumbs = true; //Should the ants drop food
    }
}