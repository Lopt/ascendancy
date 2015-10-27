namespace Client.Common.Views.HUD
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using CocosSharp;

    /// <summary>
    /// Button element. Changes Sprite when touched
    /// </summary>
    public class Button : HUDNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.HUD.Button"/> class.
        /// </summary>
        /// <param name="standard">Standard Sprite.</param>
        /// <param name="touched">Sprite when Touched.</param>
        /// <param name="callback">Callback function.</param>
        public Button(string standard, string touched, Action callback)
            : base()
        {
            m_standard = new CCSprite(standard);
            m_touched = new CCSprite(touched);
            m_callback = callback;

            AddChild(m_standard);
            AddChild(m_touched);

            m_touched.Visible = false;

            TouchHandler.Instance.ListenBegan(this, new Func<List<CCTouch>, CCEvent, bool>(OnTouchesBegan));
            TouchHandler.Instance.ListenEnded(this, new Func<List<CCTouch>, CCEvent, bool>(OnTouchesEnded));
            TouchHandler.Instance.ListenCancelled(this, new Func<List<CCTouch>, CCEvent, bool>(OnTouchesCancelled));
        }

        /// <summary>
        /// Gets the size of the standard sprite.
        /// </summary>
        /// <value>The size.</value>
        public CCSize Size
        {
            get
            {
                return m_standard.ContentSize;
            }
        }

        /// <summary>
        /// Raises the touches began event.
        /// </summary>
        /// <param name="touches">Touch Positions.</param>
        /// <param name="touchEvent">Touch event.</param>
        /// <returns>true if the event was for this node</returns>
        public bool OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (m_standard.BoundingBoxTransformedToWorld.ContainsPoint(touches[0].Location))
            {
                m_touched.Visible = true;
                m_standard.Visible = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Raises the touches ended event.
        /// </summary>
        /// <param name="touches">Touch Positions.</param>
        /// <param name="touchEvent">Touch event.</param>
        /// <returns>true if the event was for this node</returns>
        public bool OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (m_touched.BoundingBoxTransformedToWorld.ContainsPoint(touches[0].Location) &&
                m_touched.Visible)
            {
                m_callback();
                m_touched.Visible = false;
                m_standard.Visible = true;
                return true;
            }
            m_touched.Visible = false;
            m_standard.Visible = true;
            return false;
        }

        /// <summary>
        /// Raises the touches cancelled event.
        /// </summary>
        /// <param name="touches">Touch Positions.</param>
        /// <param name="touchEvent">Touch event.</param>
        /// <returns>true if the event was for this node</returns>
        public bool OnTouchesCancelled(List<CCTouch> touches, CCEvent touchEvent)
        {
            m_touched.Visible = false;
            m_standard.Visible = true;
            return false;
        }

        /// <summary>
        /// Add the button to the scene
        /// </summary>
        protected override void AddedToScene()
        {
            base.AddedToScene();

            m_standard.Position = Position;
            m_touched.Position = Position;
        }

        /// <summary>
        /// The standard button sprite.
        /// </summary>
        private CCSprite m_standard;

        /// <summary>
        /// The touched button sprite.
        /// </summary>
        private CCSprite m_touched;

        /// <summary>
        /// The callback was happens when the button was pressed.
        /// </summary>
        private Action m_callback;
    }
}