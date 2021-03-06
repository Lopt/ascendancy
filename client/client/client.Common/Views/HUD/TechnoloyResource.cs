﻿namespace Client.Common.Views.HUD
{
    using System;
    using CocosSharp;

    /// <summary>
    /// Technology resource display. Shows the node for the resource "technology".
    /// </summary>
    public class TechnologyResource : HUDNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.HUD.TechnologyResource"/> class.
        /// </summary>
        /// <param name="fileName">PNG file name.</param>
        /// <param name="color">Fill color.</param>
        public TechnologyResource(string fileName, CCColor3B color)
            : base()
        {
            m_background = new Button(
                fileName,
                fileName,
                OnTouched);

            AddChild(m_background);
            m_background.Sprite.Opacity = 100;
            // picture source
            // <div>Icons made by <a href="http://www.freepik.com" title="Freepik">Freepik</a> 
            // from <a href="http://www.flaticon.com" title="Flaticon">www.flaticon.com</a>    
            // is licensed by <a href="http://creativecommons.org/licenses/by/3.0/" 
            // title="Creative Commons BY 3.0">CC BY 3.0</a></div>
            m_progress = new CCProgressTimer(fileName);
            m_progress.Color = color;
            m_progress.Type = CCProgressTimerType.Bar;
            m_progress.BarChangeRate = new CCPoint(0.0f, 1.0f);
            m_progress.Midpoint = new CCPoint(0.0f, 0.0f);
            m_progress.PositionX = m_background.AnchorPoint.X;
            m_progress.PositionY = m_background.AnchorPoint.Y;

            m_background.AddChild(m_progress); 

            Schedule(ShowRessource);
        }

        /// <summary>
        /// Raises the touched event. Shows an Dialog with detailed information (NOT IMPLEMENTED).
        /// </summary>
        public void OnTouched()
        {
            var technology = GameAppDelegate.Account.Technology;
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
            var technology = GameAppDelegate.Account.Technology;
            m_progress.Percentage = (float)technology.GetValuePercent(GameAppDelegate.ServerTime) * 100;
        }

        /// <summary>
        /// Gets the size of the standard sprite.
        /// </summary>
        /// <value>The size.</value>
        public CCSize Size
        {
            get
            {
                return m_background.Size;
            }
        }

        /// <summary>
        /// Gets the sprite.
        /// </summary>
        /// <value>The sprite.</value>
        public CCSprite Sprite
        {
            get
            {
                return m_background.Sprite;
            }                
        }

        /// <summary>
        /// The background sprite/button.
        /// </summary>
        private Button m_background;

        /// <summary>
        /// The fill of the resource.
        /// </summary>
        private CCProgressTimer m_progress;
    }
}