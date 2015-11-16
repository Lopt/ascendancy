namespace Client.Common.Views.Effects
{
    using CocosSharp;
    using Client.Common.Constants;

    public class Healthbar : HUD.HUDNode
    {
        public Healthbar (/*Core.Models.Entity model, CCTileMapCoordinates coord*/)
            : base()
		{
            //m_current = model.Health;
            //m_max = ((Core.Models.Definitions.UnitDefinition)model.Definition).Health; 
            //m_percent = 1;//(float)m_current / (float)m_max;
            m_healthbar = new CCSprite("healthbartest");

            //m_healthbar.ScaleX = m_percent * 72; //TILE_HIGHT;
            //m_healthbar.AnchorPoint = CCPoint.AnchorMiddle;//coord.Point;
            AddChild(m_healthbar);


		}

        public CCSize Size
        {
            get
            {
                return m_healthbar.ContentSize;
            }
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            m_healthbar.Position = Position;
        }
       
        public void UpdateHealthbar (Core.Models.Entity model, CCTileMapCoordinates coord)
        {
            m_percent = model.Health / ((Core.Models.Definitions.UnitDefinition)model.Definition).Health;
            m_healthbar.ScaleX = m_percent * 72; //TILE_HIGHT;
            m_healthbar.AnchorPoint = coord.Point;
            return;
        }

        private int m_max;
        private int m_current;
        private float m_percent;
        public CCNode m_healthbar;
    }



}
