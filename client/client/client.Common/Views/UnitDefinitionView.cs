namespace Client.Common.Views
{
    using System;
    using System.Collections.Generic;
    using Client.Common.Helper;
    using CocosSharp;

    /// <summary>
    /// Unit animation kind.
    /// </summary>
    public enum UnitAnimation
    {
        None,
        Idle,
        Fight,
        Die,
        Walk
    }

    /// <summary>
    /// Unit definition view.
    /// </summary>
    public class UnitDefinitionView : Core.Views.ViewEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.UnitDefinitionView"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public UnitDefinitionView(Core.Models.Definitions.UnitDefinition model)
            : base(model)
        {
            m_animations = new Dictionary<Core.Models.Diplomatic, Dictionary<UnitAnimation, CCAnimate>>();
            m_spritesheets = new Dictionary<Core.Models.Diplomatic, CCSpriteSheet>();
            var filename = model.ID.ToString();

            foreach (Core.Models.Diplomatic situation in Enum.GetValues(typeof(Core.Models.Diplomatic)))
            {
                m_animations[situation] = new Dictionary<UnitAnimation, CCAnimate>();
                var plist = "unit_" + filename + "-" + situation.ToString() + ".plist";
                var png = "unit_" + filename + "-" + situation.ToString() + ".png";

                try
                {
                    m_spritesheets[situation] = new CCSpriteSheet(plist, png);
                    var defaultSprite = m_spritesheets[situation].Frames.Find((x) => x.TextureFilename.Contains("default"));

                    var idle = m_spritesheets[situation].Frames.FindAll((x) => x.TextureFilename.Contains("idle"));
                    var fight = m_spritesheets[situation].Frames.FindAll((x) => x.TextureFilename.Contains("attack"));
                    var walk = m_spritesheets[situation].Frames.FindAll((x) => x.TextureFilename.Contains("run"));
                    var die = m_spritesheets[situation].Frames.FindAll((x) => x.TextureFilename.Contains("die"));
                    idle.Add(defaultSprite);
                    fight.Add(defaultSprite);
                    walk.Add(defaultSprite);

                    m_animations[situation][UnitAnimation.Idle] = new CCAnimate(new CCAnimation(idle, 0.25f));
                    m_animations[situation][UnitAnimation.Fight] = new CCAnimate(new CCAnimation(fight, 0.25f));
                    m_animations[situation][UnitAnimation.Die] = new CCAnimate(new CCAnimation(die, 0.25f));
                }
                catch (Exception err)
                {
                    Logging.Error(err.Message);
                    Logging.Error("View.UnitDefinitionView: Couldn't load UnitDefinitionView. " + plist + " or " + png + " are incorrect.");
                }
            }
        }

        /// <summary>
        /// Gets the sprite copy.
        /// </summary>
        /// <returns>The sprite copy.</returns>
        /// <param name="diplomacy">The diplomacy.</param>
        public CCSprite GetSpriteCopy(Core.Models.Diplomatic diplomacy)
        {
            return new CCSprite(m_spritesheets[diplomacy].Frames.Find((x) => x.TextureFilename.Contains("default")));
        }

        /// <summary>
        /// Gets the animate.
        /// </summary>
        /// <returns>The animate.</returns>
        /// <param name="diplomacy">The diplomacy.</param>
        /// <param name="type">The type.</param>
        public CCAnimate GetAnimate(Core.Models.Diplomatic diplomacy, UnitAnimation type)
        {
            CCAnimate animate = null;
            m_animations[diplomacy].TryGetValue(type, out animate);
            return animate;
        }

        /// <summary>
        /// The spritesheets.
        /// </summary>
        private Dictionary<Core.Models.Diplomatic, CCSpriteSheet> m_spritesheets;

        /// <summary>
        /// The animations.
        /// </summary>
        private Dictionary<Core.Models.Diplomatic, Dictionary<UnitAnimation, CCAnimate>> m_animations;
    }
}
