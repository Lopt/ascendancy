using System;
using CocosSharp;
using client.Common.view;
using client.Common.Helper;

namespace client.Common.Views
{
	public class LogoLayer : CCLayerColor
	{
		public LogoLayer ()
			: base ()
		{
			m_Logo = new CCSprite ("Logo");
			this.AddChild (m_Logo);

			this.Color = CCColor3B.White;
			this.Opacity = 255;
				
		}

		#region overide

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			m_Logo.PositionX = this.VisibleBoundsWorldspace.MaxX / 2;
			m_Logo.PositionY = this.VisibleBoundsWorldspace.MaxY / 2;
			m_Logo.AnchorPoint = CCPoint.AnchorMiddle;
			m_Logo.Scale = Modify.GetScaleFactor (m_Logo.ContentSize, new CCSize (VisibleBoundsWorldspace.MaxX, VisibleBoundsWorldspace.MaxY));
		}

		#endregion

		#region Properties

		CCSprite m_Logo;

		#endregion
	}
}

