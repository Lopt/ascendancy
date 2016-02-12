namespace Client.Common.Views.HUD
{
    using System;
    using CocosSharp;

    /// <summary>
    /// HUD layer. Contains everything which lays in front of the screen and never move
    /// (btw it moves with the camera). Ressources, Dialogs, etc.
    /// </summary>
    public class HUDLayer : CCLayerColor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.HUD.HUDLayer"/> class.
        /// </summary>
        /// <param name="gameScene">Game scene.</param>
        public HUDLayer(GameScene gameScene)
            : base()
        {
            m_gameScene = gameScene;

            m_gps = new Button(
                "radars2-standard",
                "radars2-touched",
                new Action(BackToGPS));            
            m_gps.AnchorPoint = CCPoint.AnchorLowerLeft;
            AddChild(m_gps);

            m_debug = new Button(
                "debug-standard",
                "debug-touched",
                new Action(StartDebug));
            m_debug.AnchorPoint = CCPoint.AnchorLowerLeft;
            AddChild(m_debug);

            m_energyRessource = new EnergyResource();
            m_energyRessource.AnchorPoint = CCPoint.AnchorUpperLeft;
            AddChild(m_energyRessource);

            m_scrapResource = new ScrapResource("Scrap", CCColor3B.Orange);
            m_scrapResource.AnchorPoint = CCPoint.AnchorUpperLeft;
            AddChild(m_scrapResource);

            m_plutoniumResource = new PlutoniumResource("Plutonium", CCColor3B.Green);
            m_plutoniumResource.AnchorPoint = CCPoint.AnchorUpperLeft;
            AddChild(m_plutoniumResource);

            m_populationResource = new PopulationResource("Population", CCColor3B.White);
            m_populationResource.AnchorPoint = CCPoint.AnchorUpperLeft;
            AddChild(m_populationResource);

            m_techologyResource = new TechnologyResource("Tech", CCColor3B.Blue);
            m_techologyResource.AnchorPoint = CCPoint.AnchorUpperLeft;
            AddChild(m_techologyResource);

            m_question = new Button(
                "question-standard",
                "question-touched",
                new Action(DeveloperFunction));
            m_question.AnchorPoint = CCPoint.AnchorLowerLeft;
            AddChild(m_question);
        }

        /// <summary>
        /// Relocates center of the Map to GPS coordinates.
        /// </summary>
        public void BackToGPS()
        {
            m_gameScene.WorldLayerHex.ViewMode = WorldLayerHex.ViewModes.CurrentGPSPosition;
        }

        /// <summary>
        /// Starts the debugging.
        /// </summary>
        public void StartDebug()
        {
            m_gameScene.DebugLayer.Toggle();
        }

        /// <summary>
        /// Do somthing for tests.
        /// </summary>
        public void DeveloperFunction()
        {
            GameAppDelegate.Account.Scrap.MaximumValue = 1000;
            GameAppDelegate.Account.Plutonium.MaximumValue = 1000;
            GameAppDelegate.Account.Population.MaximumValue = 1000;
            GameAppDelegate.Account.Technology.MaximumValue = 1000;
            GameAppDelegate.Account.Energy.MaximumValue = 1000;

            GameAppDelegate.Account.Scrap.Set(1000, 0);
            GameAppDelegate.Account.Plutonium.Set(1000, 0);
            GameAppDelegate.Account.Population.Value = 1000;
            GameAppDelegate.Account.Technology.Set(1000, 0);
            GameAppDelegate.Account.Energy.Value = 1000;

            var account = new Core.Models.Account(GameAppDelegate.Account.ID + 1);
            new Core.Models.AccountManager().AddAccount(account);

            var pos = Models.Geolocation.Instance.CurrentGamePosition;
            var posI = new Core.Models.PositionI(pos.RegionPosition, pos.CellPosition);

            var actionCreate = Helper.ActionHelper.CreateEntity(
                new Core.Models.PositionI(pos),
                Core.Models.World.Instance.DefinitionManager.GetDefinition(Core.Models.Definitions.EntityType.Headquarter),
                account);
            
            m_gameScene.WorldLayerHex.DoAction(actionCreate);

            account.Scrap.MaximumValue = 10000;
            account.Plutonium.MaximumValue = 10000;
            account.Population.MaximumValue = 10000;
            account.Technology.MaximumValue = 10000;
            account.Energy.MaximumValue = 10000;
  
            account.Scrap.Set(10000, 0);
            account.Plutonium.Set(10000, 0);
            account.Population.Value = 10000;
            account.Technology.Set(10000, 0);
            account.Energy.Value = 10000;

            var newPos = new Core.Models.Position(pos.RegionPosition, new Core.Models.CellPosition((int)pos.X + 2, (int)pos.Y +2));  

            var actioncreate2 = Helper.ActionHelper.CreateEntity(new Core.Models.PositionI(newPos),
                Core.Models.World.Instance.DefinitionManager.GetDefinition(Core.Models.Definitions.EntityType.Barracks),
                account);

            m_gameScene.WorldLayerHex.DoAction(actioncreate2);

            var actioncr = Helper.ActionHelper.CreateEntity(
                               new Core.Models.PositionI(newPos),
                               new Core.Models.Definitions.Definition((int)Core.Models.Definitions.EntityType.Archer),
                               account);
            
            m_gameScene.WorldLayerHex.DoAction(actioncr);
        }

        /// <summary>
        /// Add the logo and loaded sprite to scene.
        /// </summary>
        protected override void AddedToScene()
        {
            base.AddedToScene();

            m_debug.PositionX = VisibleBoundsWorldspace.MinX;
            m_debug.PositionY = VisibleBoundsWorldspace.MinY;

            m_gps.PositionX = VisibleBoundsWorldspace.MinX + m_debug.Size.Width;
            m_gps.PositionY = VisibleBoundsWorldspace.MinY;

            m_energyRessource.PositionX = VisibleBoundsWorldspace.MinX;
            m_energyRessource.PositionY = VisibleBoundsWorldspace.MaxY;

            m_scrapResource.PositionX = m_energyRessource.PositionX + m_energyRessource.Size.Width;
            m_scrapResource.PositionY = VisibleBoundsWorldspace.MaxY;

            m_plutoniumResource.PositionX = m_scrapResource.PositionX + m_scrapResource.Size.Width;
            m_plutoniumResource.PositionY = VisibleBoundsWorldspace.MaxY;

            m_populationResource.PositionX = m_plutoniumResource.PositionX + m_plutoniumResource.Size.Width;
            m_populationResource.PositionY = VisibleBoundsWorldspace.MaxY;

            m_techologyResource.PositionX = m_populationResource.PositionX + m_populationResource.Size.Width;
            m_techologyResource.PositionY = VisibleBoundsWorldspace.MaxY;

            m_question.PositionX = VisibleBoundsWorldspace.MaxX;
            m_question.PositionX = VisibleBoundsWorldspace.MaxX - m_question.Size.Width;
            
            m_question.PositionY = VisibleBoundsWorldspace.MinY;
        }

        /// <summary>
        /// The back to gps coordinates position button.
        /// </summary>
        private Button m_gps;

        /// <summary>
        /// The open debug layer button.
        /// </summary>
        private Button m_debug;

        /// <summary>
        /// Call developer function.
        /// </summary>
        private Button m_question;

        /// <summary>
        /// The energy ressource hud element.
        /// </summary>
        private EnergyResource m_energyRessource;

        /// <summary>
        /// The scrap resource hud element.
        /// </summary>
        private ScrapResource m_scrapResource;

        /// <summary>
        /// The plutonium resource hud element.
        /// </summary>
        private PlutoniumResource m_plutoniumResource;

        /// <summary>
        /// The population hud resource.
        /// </summary>
        private PopulationResource m_populationResource;

        /// <summary>
        /// The techology hud resource.
        /// </summary>
        private TechnologyResource m_techologyResource;

        /// <summary>
        /// The game scene.
        /// </summary>
        private GameScene m_gameScene;    
    }
}

