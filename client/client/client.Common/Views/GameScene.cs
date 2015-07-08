using System;
using CocosSharp;
using System.Collections.Generic;
using client.Common.Models;

namespace client.Common.Views
{
    public class GameScene : CCScene
    {
        public GameScene (CCWindow mainWindow)
            : base (mainWindow)
        {
            m_region = new WorldLayer (Geolocation.Instance.CurrentRegionPosition);
            this.AddChild (m_region);

        }


        #region Properties

        WorldLayer m_region;

        #endregion
    }
}

