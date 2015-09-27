namespace Client.Common.Views.HUD
{
    using System;
    using CocosSharp;

    /// <summary>
    /// HUD layer. Contains everything which lays in front of the screen and never move
    /// (btw it moves with the camera). Ressources, Dialogs, etc.
    /// </summary>
    public class HUDLayer : CCLayerColor
    {
        public HUDLayer(GameScene gameScene)
            : base()
        {
            m_gameScene = gameScene;

            m_gps = new Button(
                new CCSprite("radars2-standard"),
                new CCSprite("radars2-touched"),
                new Action(BackToGPS));
            AddChild(m_gps);

            m_debug = new Button(
                new CCSprite("debug-standard"),
                new CCSprite("debug-touched"),
                new Action(StartDebug));
            AddChild(m_debug);
        }   

        /// <summary>
        /// Add the logo and loaded sprite to scene.
        /// </summary>
        protected override void AddedToScene()
        {
            base.AddedToScene();

            m_debug.PositionX = VisibleBoundsWorldspace.MinX;
            m_debug.PositionY = VisibleBoundsWorldspace.MinY;
            m_debug.AnchorPoint = CCPoint.AnchorLowerLeft;

            m_gps.PositionX = VisibleBoundsWorldspace.MinX + m_debug.Size.Width;
            m_gps.PositionY = VisibleBoundsWorldspace.MinY;
            m_gps.AnchorPoint = CCPoint.AnchorLowerLeft;

            //m_touched.AnchorPoint = CCPoint.AnchorUpperLeft;
            //m_standard.AnchorPoint = CCPoint.AnchorUpperLeft;
        }

        public void BackToGPS()
        {
            m_gameScene.WorldLayer.DrawRegionsAsync();
        }

        public void StartDebug()
        {
            m_gameScene.DebugLayer.Toggle();
        }

        private Button m_gps;
        private Button m_debug;

        private GameScene m_gameScene;
    }
}

