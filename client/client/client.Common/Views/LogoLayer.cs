using System;
using CocosSharp;
using client.Common.Helper;
using client.Common.Controllers;
using @base.control;

namespace client.Common.Views
{
    public class LogoLayer : CCLayerColor
    {
        public LogoLayer()
            : base()
        {

            m_RegionC = Controller.Instance.RegionManagerController as RegionManagerController;


            m_Logo = new CCSprite("logo");

            m_LoadedSprite = new CCSprite("Ladebalken");
            m_LoadedSprite.Visible = false;



            this.AddChild(m_Logo);
            this.AddChild(m_LoadedSprite); 

            this.Color = CCColor3B.White;
            this.Opacity = 255;

            this.Schedule(LoadingProgress);
        }

        #region overide

        protected override void AddedToScene()
        {
            base.AddedToScene();

            m_Logo.PositionX = this.VisibleBoundsWorldspace.MidX;
            m_Logo.PositionY = this.VisibleBoundsWorldspace.MidY;
            m_Logo.AnchorPoint = CCPoint.AnchorMiddle;
            m_Logo.Scale = Modify.GetScaleFactor(m_Logo.ContentSize, new CCSize(VisibleBoundsWorldspace.MaxX, VisibleBoundsWorldspace.MaxY));

            m_LoadedSprite.PositionX = this.VisibleBoundsWorldspace.MidX;
            m_LoadedSprite.PositionY = this.VisibleBoundsWorldspace.MinY;
            m_LoadedSprite.AnchorPoint = CCPoint.AnchorMiddleBottom;

        }

        #endregion

        #region Scheduling

        void LoadingProgress(float frameTimesInSecond)
        {
            if (GameAppDelegate.LoadingState >= GameAppDelegate.Loading.TerrainTypeLoaded)
            {
                m_LoadedSprite.Visible = true;
            }

            if (GameAppDelegate.LoadingState >= GameAppDelegate.Loading.Done)
            {
                Window.DefaultDirector.ReplaceScene(new GameScene(Window));
            }
        }

        #endregion

        #region Properties

        RegionManagerController m_RegionC;
        CCSprite m_Logo;
        CCSprite m_LoadedSprite;

        #endregion
    }
}

