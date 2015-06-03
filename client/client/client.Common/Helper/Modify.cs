using System;
using CocosSharp;
using @base.model;

namespace client.Common.Helper
{
    public class Modify
    {
        public Modify ()
        {
        }

        public static float GetScaleFactor (CCSize content, CCSize space)
        {
            var contentSquare = content.Height * content.Width;
            var spaceSquare = space.Height * space.Width;
            var scaleFactor = spaceSquare / contentSquare;

            return scaleFactor;
        }
 
    }
}

