using System;
using CocosSharp;
using client.Common.Helper;
using client.Common.Controllers;
using @base.control;

namespace client.Common.Views
{
    public class LogoLayer : CCLayerColor
    {
        public LogoLayer(StartScene startScene)
            : base()
        {
            m_startScene = startScene;

            m_logo = new CCSprite("logo");

            m_loadedSprite = new CCSprite("Ladebalken");
            m_loadedSprite.Visible = false;



            this.AddChild(m_logo);
            this.AddChild(m_loadedSprite); 

            this.Color = CCColor3B.White;
            this.Opacity = 255;

            this.Schedule(LoadingProgress);
        }

        #region overide

        protected override void AddedToScene()
        {
            base.AddedToScene();

            m_logo.PositionX = this.VisibleBoundsWorldspace.MidX;
            m_logo.PositionY = this.VisibleBoundsWorldspace.MidY;
            m_logo.AnchorPoint = CCPoint.AnchorMiddle;
            //m_logo.Scale = Modify.GetScaleFactor(m_logo.ContentSize, new CCSize(VisibleBoundsWorldspace.MaxX, VisibleBoundsWorldspace.MaxY));

            m_loadedSprite.PositionX = this.VisibleBoundsWorldspace.MidX;
            m_loadedSprite.PositionY = this.VisibleBoundsWorldspace.MinY;
            m_loadedSprite.AnchorPoint = CCPoint.AnchorMiddleBottom;

        }

        #endregion

        #region Scheduling

        void LoadingProgress(float frameTimesInSecond)
        {
            if (m_startScene.Phase >= StartScene.Phases.TerrainTypeLoaded)
            {
                m_loadedSprite.Visible = true;
            }

        }


        #endregion

        #region Properties

        CCSprite m_logo;
        CCSprite m_loadedSprite;
        StartScene m_startScene;

        #endregion
    }
}

