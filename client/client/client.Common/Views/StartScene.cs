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

		}

		#region Properties

		LogoLayer m_LogoLayer;

		#endregion
	}
}

