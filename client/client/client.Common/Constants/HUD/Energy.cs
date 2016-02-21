namespace Client.Common.Constants.HUD
{
    using System;
    using CocosSharp;

    /// <summary>
    /// All Energy HUD constants
    /// </summary>
    public class Energy
    {
        /// <summary>
        /// File for the Background of the Energy Resource
        /// </summary>
        public const string DISPLAY = "EnergyRessource1";

        /// <summary>
        /// File for the Foreground (the pointer) of the Energy Resource
        /// </summary>
        public const string POINTER = "EnergyRessource2";

        /// <summary>
        /// (in percent) Position where the pointer should be at the display
        /// </summary>
        public static readonly CCPoint DISPLAY_CENTER = new CCPoint(0.50f, -0.66f);

        /// <summary>
        /// (in percent) Position where the center of the pointer is
        /// </summary>
        public static readonly CCPoint POINTER_CENTER = new CCPoint(0.12f, -0.55f);

        /// <summary>
        /// how fast the pointer should move
        /// </summary>
        public static readonly float POINTER_SPEED = 7.0f;

        /// <summary>
        /// (degree) max rotation of pointer
        /// </summary>
        public static readonly float MAX_POINTER = 200.0f;

        /// <summary>
        /// The DIF f POINTE.
        /// </summary>
        public static readonly float DIFF_POINTER = -10.0f;

        /// <summary>
        /// exponential noise (higher -> more noise)
        /// </summary>
        public static readonly int NOISE_FACTOR  = 12;

        /// <summary>
        /// factor noise (higher -> lower noise)
        /// </summary>
        public static readonly int NOISE_DIV = 128;

        /// <summary>
        /// how much noise, minimum.
        /// </summary>
        public static readonly int MIN_NOISE_FACTOR = 1;

        /// <summary>
        /// how much noise, maximum.
        /// </summary>
        public static readonly int MAX_NOISE_FACTOR = 3;
    }
}
