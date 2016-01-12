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
            Position = m_gameScene.WorldLayerHex.Position;
            Schedule(testupdate);
            AddHealthbar();
            //m_healthbar = new Healthbar();
            //m_healthbar.AnchorPoint = CCPoint.AnchorMiddle;
            //AddChild(m_healthbar);
        }

        public void testupdate(float time)
        {
            //AnchorPoint = m_gameScene.WorldLayer.WorldTileMap.TileLayersContainer.AnchorPoint;
            //PositionX = m_gameScene.WorldLayer.WorldTileMap.TileLayersContainer.PositionX;
            //PositionY = m_gameScene.WorldLayer.WorldTileMap.TileLayersContainer.PositionY;
            //ContentSize = m_gameScene.WorldLayer.WorldTileMap.TileLayersContainer.ScaledContentSize;

            //m_healthbar.m_healthbar.AnchorPoint = m_gameScene.WorldLayer.WorldTileMap.TileLayersContainer.AnchorPoint;
            //m_healthbar.m_healthbar.Position = m_gameScene.WorldLayer.WorldTileMap.TileLayersContainer.Position + m_gameScene.WorldLayer.WorldTileMap.TileLayersContainer.ScaledContentSize.Center;


            //m_healthbar.m_healthbar.ContentSize = new CCSize(m_healthbar.m_healthbar.ContentSize.Width + 1, m_healthbar.m_healthbar.ContentSize.Height + 1);
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

            m_healthbar.Position = Position;
        }


        private GameScene m_gameScene;
        private Healthbar m_healthbar;
    }
}