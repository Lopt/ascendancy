using System;
using CocosSharp;
using client.Common.view;
using client.Common.Helper;
using client.Common.Controllers;
using @base.control;

namespace client.Common.Views
{
	public class LogoLayer : CCLayerColor
	{
		public LogoLayer ()
			: base ()
		{

			m_RegContr = Controller.Instance.RegionManagerController as RegionController;

			m_Logo = new CCSprite ("Logo");
			m_Loading = new CCSprite ("monkey");


			this.AddChild (m_Logo);
			this.AddChild (m_Loading); 

			this.Color = CCColor3B.White;
			this.Opacity = 255;

			this.Schedule (Loading);

			var touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesEnded = (touches, ccevent) => {
				if (GameAppDelegate.m_Loading >= GameAppDelegate.Loading.Done)
					Window.DefaultDirector.ReplaceScene (new GameScene (Window));
			};

			this.AddEventListener (touchListener);
				
		}

		#region overide

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			m_Logo.PositionX = this.VisibleBoundsWorldspace.MidX;
			m_Logo.PositionY = this.VisibleBoundsWorldspace.MidY;
			m_Logo.AnchorPoint = CCPoint.AnchorMiddle;
			m_Logo.Scale = Modify.GetScaleFactor (m_Logo.ContentSize, new CCSize (VisibleBoundsWorldspace.MaxX, VisibleBoundsWorldspace.MaxY));

			m_Loading.PositionX = this.VisibleBoundsWorldspace.MidX;
			m_Loading.PositionY = this.VisibleBoundsWorldspace.MinY;
			m_Loading.AnchorPoint = CCPoint.AnchorMiddleBottom;

		}

		#endregion

		#region Scheduling

		void Loading (float FrameTimesInSecond)
		{
			if (GameAppDelegate.m_Loading >= GameAppDelegate.Loading.Done) {
				m_Loading.Visible = false;

			}

		}

		#endregion

		#region Properties

		RegionController m_RegContr;
		CCSprite m_Logo;
		CCSprite m_Loading;

		#endregion
	}
}

