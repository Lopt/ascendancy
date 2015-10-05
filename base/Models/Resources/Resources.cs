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

    public class Scrap : IncrementalResource
    {        
    }

    public class Technology : IncrementalResource
    {        
    }

    public class Plutonium : IncrementalResource
    {        
    }

    public class Energy : StateResource
    {        
    }

    public class Population : StateResource
    {        
    }
}