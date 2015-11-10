namespace Client.Common.Views.Effects
{
    using System;
    using CocosSharp;

    public class EffectLayer : CCLayerColor
    {
        public EffectLayer(GameScene gameScene)
            : base ()
        {
            m_gameScene = gameScene;
            m_testbar = new Healthbar();
            m_testbar.AnchorPoint = CCPoint.AnchorMiddle;


        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            m_testbar.Position = VisibleBoundsWorldspace.Center;
        }


        private GameScene m_gameScene;
        private Healthbar m_testbar;
    }
}