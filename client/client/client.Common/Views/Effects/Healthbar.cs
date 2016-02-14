namespace Client.Common.Views.Effects
{
    using CocosSharp;
    using System;
    using Client.Common.Constants;

    public class Healthbar : CCNode
    {
        public Healthbar ()
            : base()
		{
            m_healthbar = new CCSprite("healthbartest");
            AddChild(m_healthbar);
		}

        protected override void AddedToScene()
        {
            m_healthbar.Position = new CCPoint(
                Constants.ViewConstants.Healthbar.POSITION_X,
                Constants.ViewConstants.Healthbar.POSITION_Y
            );
        }
                   
        public void UpdateHealthbar (float newValue)
        {
            var newWidth = newValue * Constants.ViewConstants.Healthbar.MAX_WIDTH;
            var newHeigth = newValue * Constants.ViewConstants.Healthbar.MAX_HEIGTH;

            m_healthbar.ScaleX = Math.Max(Constants.ViewConstants.Healthbar.MIN_WIDTH, newWidth);
            m_healthbar.ScaleY = Math.Max(Constants.ViewConstants.Healthbar.MIN_HEIGTH, newHeigth);                
        }

        private CCNode m_healthbar;
    }



}
