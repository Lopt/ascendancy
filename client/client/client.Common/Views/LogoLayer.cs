namespace Client.Common.Views
{
    using System;
    using Client.Common.Controllers;
    using Client.Common.Helper;
    using CocosSharp;
    using Core.Controllers.Actions;

    /// <summary>
    /// Logo layer.
    /// </summary>
    public class LogoLayer : CCLayerColor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.LogoLayer"/> class.
        /// </summary>
        /// <param name="startScene">Start scene.</param>
        public LogoLayer(StartScene startScene)
            : base()
        {
            m_startScene = startScene;

            m_logo = new CCSprite("logo");

            m_loadedSprite = new CCSprite("Ladebalken");
            m_loadedSprite.Visible = false;

            AddChild(m_logo);
            AddChild(m_loadedSprite); 

            Color = CCColor3B.White;
            Opacity = 255;

            Schedule(LoadingProgress);
        }

        #region overide

        /// <summary>
        /// Add the logo and loaded sprite to scene.
        /// </summary>
        protected override void AddedToScene()
        {
            base.AddedToScene();

            m_logo.PositionX = this.VisibleBoundsWorldspace.MidX;
            m_logo.PositionY = this.VisibleBoundsWorldspace.MidY;
            m_logo.AnchorPoint = CCPoint.AnchorMiddle;

            m_loadedSprite.PositionX = this.VisibleBoundsWorldspace.MidX;
            m_loadedSprite.PositionY = this.VisibleBoundsWorldspace.MinY;
            m_loadedSprite.AnchorPoint = CCPoint.AnchorMiddleBottom;
        }

        #endregion

        #region Scheduling

        /// <summary>
        /// Show the loaded sprite if the terrain type is loaded.
        /// </summary>
        /// <param name="frameTimesInSecond">Frame times in second.</param>
        private void LoadingProgress(float frameTimesInSecond)
        {
            if (m_startScene.Phase >= StartScene.Phases.TerrainsLoaded)
            {
                m_loadedSprite.Visible = true;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The m_logo.
        /// </summary>
        private CCSprite m_logo;

        /// <summary>
        /// The m_loaded sprite.
        /// </summary>
        private CCSprite m_loadedSprite;

        /// <summary>
        /// The m_start scene.
        /// </summary>
        private StartScene m_startScene;

        #endregion
    }
}