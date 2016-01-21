namespace Client.Common.Constants.HUD
{
    using System;
    using CocosSharp;

    /// <summary>
    /// All scrap HUD constants.
    /// </summary>
    public class Resource
    {

        public static void SetDISPLAY(String filename)
        {
            m_DISPLAY = filename;    
        }

        public static String GetDISPLAY()
        {
            return m_DISPLAY;
        }

        //public static string DISPLAY = "Resource";
        public static readonly CCPoint DISPLAY_CENTER = new CCPoint(0.50f, -0.66f);

        private static string m_DISPLAY;
    }
}

