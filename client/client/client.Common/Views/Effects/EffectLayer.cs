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
            Position = m_gameScene.WorldLayer.Position;
            AnchorPoint = m_gameScene.WorldLayer.AnchorPoint;
            AddHealthbar();
            //m_healthbar = new Healthbar();
            //m_healthbar.AnchorPoint = CCPoint.AnchorMiddle;
            //AddChild(m_healthbar);
        }

        public void AddHealthbar(/*Core.Models.Entity entity, CCTileMapCoordinates coord*/)
        {
            m_healthbar = new Healthbar();
            m_healthbar.AnchorPoint = CCPoint.AnchorLowerLeft;
            AddChild(m_healthbar);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            m_healthbar.Position = m_gameScene.WorldLayer.UnitLayer.PositionWorldspace;
        }


        private GameScene m_gameScene;
        private Healthbar m_healthbar;
    }
}