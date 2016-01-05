namespace Core.Models.Resources
{
    using System;

    /// <summary>
    /// All available Resources.
    /// </summary>
    public enum Resources
    {
        Scrap = 0,
        Energy = 1,
        Technology = 2,
        Plutonium = 3,
        Population = 4
    }

    /// <summary>
    /// Scrap resource of the player.
    /// </summary>
    public class Scrap : IncrementalResource
    {        
    }

    /// <summary>
    /// Technology resource of the player.
    /// </summary>
    public class Technology : IncrementalResource
    {        
    }

    /// <summary>
    /// Plutonium resource of the player.
    /// </summary>
    public class Plutonium : IncrementalResource
    {        
    }

    /// <summary>
    /// Energy resource of the player.
    /// </summary>
    public class Energy : StateResource
    {        
    }

    /// <summary>
    /// Population resource of the player.
    /// </summary>
    public class Population : StateResource
    {        
    }
}