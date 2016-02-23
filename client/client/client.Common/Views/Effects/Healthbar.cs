namespace Client.Common.Views.Effects
{
    using System;
    using Client.Common.Constants;
    using CocosSharp;

    /// <summary>
    /// The healthbar node.
    /// </summary>
    public class Healthbar : CCNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.Effects.Healthbar"/> class.
        /// </summary>
        public Healthbar()
            : base()
        {
            m_healthbar = new CCSprite("healthbar");
            m_healthbarEmpty = new CCSprite("healthbar");
            m_healthbarEmpty.Opacity = Constants.ViewConstants.Healthbar.OPACITY;
            AddChild(m_healthbar);
            AddChild(m_healthbarEmpty);
        }

        /// <summary>
        /// Updates the healthbar.
        /// </summary>
        /// <param name="newValue">New value.</param>
        public void UpdateHealthbar(float newValue)
        {
            var newWidth = newValue * Constants.ViewConstants.Healthbar.MAX_WIDTH;
            var newHeigth = newValue * Constants.ViewConstants.Healthbar.MAX_HEIGTH;

            m_healthbar.ScaleX = Math.Max(Constants.ViewConstants.Healthbar.MIN_WIDTH, newWidth);
            m_healthbar.ScaleY = Math.Max(Constants.ViewConstants.Healthbar.MIN_HEIGTH, newHeigth);                
        }

        /// <summary>
        /// Addeds to scene.
        /// </summary>
        protected override void AddedToScene()
        {
            m_healthbar.Position = new CCPoint(
                Constants.ViewConstants.Healthbar.POSITION_X,
                Constants.ViewConstants.Healthbar.POSITION_Y);
            m_healthbarEmpty.Position = m_healthbar.Position;
            m_healthbarEmpty.ScaleX = Constants.ViewConstants.Healthbar.MAX_WIDTH;
            m_healthbarEmpty.ScaleY = Constants.ViewConstants.Healthbar.MAX_HEIGTH;
        }

        /// <summary>
        /// The healthbar.
        /// </summary>
        private CCNode m_healthbar;

        /// <summary>
        /// The healthbar empty.
        /// </summary>
        private CCNode m_healthbarEmpty;
    }
}
