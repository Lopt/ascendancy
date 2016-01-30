using System;
using System.Collections.Generic;
using CocosSharp;

using Client.Common.Helper;

namespace Client.Common.Views
{
    public enum UnitAnimation
    {
        None,
        Idle,
        Fight
    }

    public class UnitDefinitionView : Core.Views.ViewEntity
    {
        public UnitDefinitionView(Core.Models.Definitions.UnitDefinition model)
            : base(model)
        {
            var filename = model.ID.ToString();
            try
            {
                var spritesheet = new CCSpriteSheet("unit_" + filename + ".plist", "unit_" + filename + ".png").Frames;
            }
            catch (Exception err)
            {
                Logging.Error(err.Message);
                Logging.Error("Couldn't load UnitDefinitionView. "+ filename + ".plist or " + filename + ".png are incorrect.");
            }
        }

        public CCSpriteSheet Spritesheet;
        public Dictionary<UnitAnimation, CCAnimate> Animations;
    }
}

