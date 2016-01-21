namespace Client.Common.Constants.HUD
{
    using System;
    using CocosSharp;

    /// <summary>
    /// All Energy HUD constants
    /// </summary>
    public class Energy
    {
        public const string DISPLAY = "EnergyRessource1";
        public const string POINTER = "EnergyRessource2";

        public static readonly CCPoint DISPLAY_CENTER = new CCPoint(0.50f, -0.66f);
        public static readonly CCPoint POINTER_CENTER = new CCPoint(0.12f, -0.55f);

        public const float POINTER_SPEED = 7.0f;
        public const float MAX_POINTER = 200.0f;
        public const float DIFF_POINTER = -10.0f;

        public const int NOISE_FACTOR  = 12;
        public const int NOISE_DIV = 128;

        public const int MIN_NOISE_FACTOR = 1;
        public const int MAX_NOISE_FACTOR = 3;

    }
}

