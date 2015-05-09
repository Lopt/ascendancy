using System;
using CocosSharp;
using client.Common.Views;

namespace client.Common.view
{
	public class StartScene : CCScene
	{
		public StartScene (CCWindow _MainWindow)
			: base (_MainWindow)
		{
			m_LogoLayer = new LogoLayer ();
			this.AddChild (m_LogoLayer);

			var touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesEnded = (touches, ccevent) => {
				Window.DefaultDirector.ReplaceScene (new GameScene (Window));
			};

			this.AddEventListener (touchListener);
		}

		#region Properties

		LogoLayer m_LogoLayer;

		#endregion
	}
}

