namespace Client.Common.Constants.ViewConstants
{
    using System;
    using CocosSharp;

    /// <summary>
    /// All constants which are relevant to the health bar
    /// </summary>
    public class Healthbar
    {
        /// <summary>
        /// minimum width of the health bar
        /// </summary>
        public static readonly float MIN_WIDTH = 0;

        /// <summary>
        /// maximum width of the health bar
        /// </summary>
        public static readonly float MAX_WIDTH = 40;

        /// <summary>
        /// maximum height of the health bar
        /// </summary>
        public static readonly float MIN_HEIGTH = 4;

        /// <summary>
        /// minimum height of the health bar
        /// </summary>
        public static readonly float MAX_HEIGTH = 4;

        /// <summary>
        /// position x of the healthbar, relativ to the tile
        /// </summary>
        public static readonly float POSITION_X = 37;

        /// <summary>
        /// position y of the healthbar, relativ to the tile
        /// </summary>
        public static readonly float POSITION_Y = 62;

        /// <summary>
        /// opacity of the lost 
        /// </summary>
        public static readonly byte OPACITY = 125;
    }

    /// <summary>
    /// all constants related to the unit itself
    /// </summary>
    public class UnitView
    {
        /// <summary>
        /// how long units should lay around after death in seconds
        /// </summary>
        public static readonly float DEATH_LYING_AROUD_TIME = 30;
    }
}