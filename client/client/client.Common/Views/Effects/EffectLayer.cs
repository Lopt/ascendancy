namespace Client.Common.Views.Effects
{
    using System;
    using CocosSharp;

    /// <summary>
    /// Effect layer.
    /// </summary>
    public class EffectLayer : CCLayerColor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.Effects.EffectLayer"/> class.
        /// </summary>
        /// <param name="gameScene">Game scene.</param>
        public EffectLayer(GameScene gameScene)
            : base()
        {
            m_gameScene = gameScene;
            Position = m_gameScene.WorldLayerHex.Position;
            Schedule(Testupdate);
            // AddHealthbar();
            // m_healthbar = new Healthbar();
            // m_healthbar.AnchorPoint = CCPoint.AnchorMiddle;
            // AddChild(m_healthbar);
        }

        /// <summary>
        /// Testupdate the specified time.
        /// </summary>
        /// <param name="time">Schedule time.</param>
        public void Testupdate(float time)
        {
            // AnchorPoint = m_gameScene.WorldLayer.WorldTileMap.TileLayersContainer.AnchorPoint;
            // PositionX = m_gameScene.WorldLayer.WorldTileMap.TileLayersContainer.PositionX;
            // PositionY = m_gameScene.WorldLayer.WorldTileMap.TileLayersContainer.PositionY;
            // ContentSize = m_gameScene.WorldLayer.WorldTileMap.TileLayersContainer.ScaledContentSize;

            // m_healthbar.m_healthbar.AnchorPoint = m_gameScene.WorldLayer.WorldTileMap.TileLayersContainer.AnchorPoint;
            // m_healthbar.m_healthbar.Position = m_gameScene.WorldLayer.WorldTileMap.TileLayersContainer.Position + m_gameScene.WorldLayer.WorldTileMap.TileLayersContainer.ScaledContentSize.Center;

            // m_healthbar.m_healthbar.ContentSize = new CCSize(m_healthbar.m_healthbar.ContentSize.Width + 1, m_healthbar.m_healthbar.ContentSize.Height + 1);
        }

        /// <summary>
        /// Adds the healthbar.
        /// </summary>
        public void AddHealthbar(/*Core.Models.Entity entity, CCTileMapCoordinates coord*/)
        {
            m_healthbar = new Healthbar();
            m_healthbar.AnchorPoint = CCPoint.AnchorLowerLeft;
            AddChild(m_healthbar);
        }

        /// <summary>
        /// Addeds to scene.
        /// </summary>
        protected override void AddedToScene()
        {
            base.AddedToScene();

            m_healthbar.Position = Position;
        }

        /// <summary>
        /// The game scene.
        /// </summary>
        private GameScene m_gameScene;

        /// <summary>
        /// The healthbar.
        /// </summary>
        private Healthbar m_healthbar;
    }
}