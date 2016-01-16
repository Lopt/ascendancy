namespace Client.Common.Views.HUD
{
    using System;
    using CocosSharp;

    /// <summary>
    /// Scrap resource display. Shows the for the resource "scrap".
    /// </summary>
    public class ScrapResource : HUDNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.HUD.ScrapResource"/> class.
        /// </summary>
        public ScrapResource()
            : base()
        {
            m_background = new Button(
                Constants.HUD.Scrap.DISPLAY,
                Constants.HUD.Scrap.DISPLAY,
                OnTouched);

            AddChild(m_background);

            var scrap = GameAppDelegate.Account.Scrap;
            Schedule(ShowRessource);
        }

        /// <summary>
        /// Raises the touched event. Shows an Dialog with detailed information (NOT IMPLEMENTED).
        /// </summary>
        public void OnTouched()
        {
            var scrap = GameAppDelegate.Account.Scrap;
        }

        /// <summary>
        /// Called when added to the scene.
        /// </summary>
        protected override void AddedToScene()
        {
            base.AddedToScene();
            m_background.Position = Position;
        }

        /// <summary>
        /// Refreshes the current state of the resource in the HUD
        /// </summary>
        /// <param name="time">time since the last call.</param>
        private void ShowRessource(float time)
        {
            var energy = GameAppDelegate.Account.Scrap;

            // Wege die Farbe zu ändern
            //m_background.Sprite.Color = CCColor3B.Black;  
            //m_background.Sprite.UpdateDisplayedColor(CCColor3B.Red);

            var test = m_background.Sprite.TextureRectInPixels;
            CCProgressTimer bla = new CCProgressTimer("Scrap");
           
            //bla.BarChangeRate = CCPoint(1, 0);
        }

        /// <summary>
        /// The background sprite/button.
        /// </summary>
        private Button m_background;

    }
}