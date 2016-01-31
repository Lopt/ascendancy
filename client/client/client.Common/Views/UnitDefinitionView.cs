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
            m_animations = new Dictionary<UnitAnimation, CCAnimate>();
            var filename = model.ID.ToString();

            try
            {
                m_spritesheet = new CCSpriteSheet("unit_" + filename + ".plist", "unit_" + filename + ".png");

                var idle = new CCAnimation(m_spritesheet.Frames.FindAll((x) => x.TextureFilename.Contains("idle")), 0.2f);
                var fight = new CCAnimation(m_spritesheet.Frames.FindAll((x) => x.TextureFilename.Contains("idle")), 0.2f);

                m_animations[UnitAnimation.Idle] = new CCAnimate(idle);
                m_animations[UnitAnimation.Fight] = new CCAnimate(fight);

            }
            catch (Exception err)
            {
                Logging.Error(err.Message);
                Logging.Error("View.UnitDefinitionView: Couldn't load UnitDefinitionView. "+ filename + ".plist or " + filename + ".png are incorrect.");
            }
        }

        public CCSprite GetSpriteCopy()
        {
            return new CCSprite(m_spritesheet.Frames[0]);
        }

        public CCAnimate GetAnimate(UnitAnimation type)
        {
            CCAnimate animate = null;
            m_animations.TryGetValue(type, out animate);
            return animate;
        }

        private CCSpriteSheet m_spritesheet;
        private Dictionary<UnitAnimation, CCAnimate> m_animations;
    }
}

