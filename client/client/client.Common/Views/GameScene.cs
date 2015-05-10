using System;
using CocosSharp;
using client.Common.Views;
using System.Collections.Generic;

namespace client.Common.view
{
	public class GameScene : CCScene
	{
		public GameScene (CCWindow _MainWindow)
			: base (_MainWindow)
		{
			m_region = new WorldLayer (Geolocation.GetInstance.CurrentRegionPosition);
			this.AddChild (m_region);

		}


		#region Properties

		WorldLayer m_region;

		#endregion
	}
}

