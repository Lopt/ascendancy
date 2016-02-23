namespace Client.Common.Views.HUD
{
    using System;
    using CocosSharp;

    /// <summary>
    /// Energy resource display. Shows the node for the resource "energy".
    /// </summary>
    public class EnergyResource : HUDNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.HUD.EnergyResource"/> class.
        /// </summary>
        public EnergyResource()
            : base()
        {
            m_background = new Button(
                Constants.HUD.Energy.DISPLAY,
                Constants.HUD.Energy.DISPLAY,
                OnTouched);
            m_pointer = new CCSprite(Common.Constants.HUD.Energy.POINTER);
            
            AddChild(m_background);
            AddChild(m_pointer);

            var energy = GameAppDelegate.Account.Energy;
            Schedule(ShowRessource);

            energy.Value = 0.0;
            m_lastValue = 0.0;
        }

        /// <summary>
        /// Raises the touched event. Shows an Dialog with detailed information (NOT IMPLEMENTED).
        /// </summary>
        public void OnTouched()
        {
            var energy = GameAppDelegate.Account.Energy;
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
        /// Called when added to the scene.
        /// </summary>
        protected override void AddedToScene()
        {
            base.AddedToScene();
            m_background.Position = Position;
            m_pointer.Position += Constants.HUD.Energy.DISPLAY_CENTER * m_background.Size;
            m_pointer.AnchorPoint += Constants.HUD.Energy.POINTER_CENTER;
        }

        /// <summary>
        /// Refreshes the current state of the resource in the HUD
        /// </summary>
        /// <param name="time">time since the last call.</param>
        private void ShowRessource(float time)
        {
            var smallest_step = Constants.HUD.Energy.POINTER_SPEED * time;
            var rnd = new Random();
            var energy = GameAppDelegate.Account.Energy;

            var rot = (float)(energy.ValuePercent * Constants.HUD.Energy.MAX_POINTER) - Constants.HUD.Energy.DIFF_POINTER;
            var newValue = rot + m_noise;
            var diff = m_lastValue - newValue;
            if (diff > 0)
            {
                m_lastValue -= Math.Min(diff, smallest_step);
            }
            else if (diff < 0)
            {
                m_lastValue -= Math.Max(diff, -smallest_step);
            }
            else
            {
                m_noise = 1;
                for (var index = 0; index < Constants.HUD.Energy.NOISE_FACTOR; ++index)
                {
                    m_noise *= rnd.Next(
                        Constants.HUD.Energy.MIN_NOISE_FACTOR,
                        Constants.HUD.Energy.MAX_NOISE_FACTOR);
                }
                m_noise /= Constants.HUD.Energy.NOISE_DIV;
            }
            m_pointer.Rotation = (float)m_lastValue;
        }

        /// <summary>
        /// The background sprite/button.
        /// </summary>
        private Button m_background;

        /// <summary>
        /// The pointer sprite.
        /// </summary>
        private CCNode m_pointer;

        /// <summary>
        /// The last value of the resource.
        /// </summary>
        private double m_lastValue;

        /// <summary>
        /// It's an anlog device, so it has noise. Implemented so the pointer will "shake" near the value position.
        /// </summary>
        private double m_noise;
    }
}