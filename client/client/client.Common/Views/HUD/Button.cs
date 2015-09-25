namespace Client.Common.Views.HUD
{
    using System;
    using System.Threading;
    using System.Collections.Generic;
    using CocosSharp;

    public class Button : CCNode
    {
        public Button(CCSprite standard, CCSprite touched, Action callback)
            :base()
        {
            m_standard = standard;
            m_touched = touched;
            m_callback = callback;

            AddChild(m_standard);
            AddChild(m_touched);

            m_touched.Visible = false;

            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesBegan = OnTouchesBegan;
            touchListener.OnTouchesEnded = OnTouchesEnded;
            touchListener.OnTouchesCancelled = OnTouchesCancelled;
            AddEventListener(touchListener);
        }

        /// <summary>
        /// Add the button to the scene
        /// </summary>
        protected override void AddedToScene()
        {
            base.AddedToScene();

            m_touched.AnchorPoint = AnchorPoint;
            m_standard.AnchorPoint = AnchorPoint;
            m_standard.Position = Position;
            m_touched.Position = Position;
        }

        public CCSize Size
        {
            get
            {
                return m_standard.ContentSize;
            }
        }



        public void OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (m_standard.BoundingBoxTransformedToWorld.ContainsPoint(touches[0].Location))
            {
                m_touched.Visible = true;
                m_standard.Visible = false;
            }
        }

        public void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (m_touched.BoundingBoxTransformedToWorld.ContainsPoint(touches[0].Location) &&
                m_touched.Visible)
            {
                m_callback();
            }
            m_touched.Visible = false;
            m_standard.Visible = true;
        }

        public void OnTouchesCancelled(List<CCTouch> touches, CCEvent touchEvent)
        {
            m_touched.Visible = false;
            m_standard.Visible = true;
        }

        private CCSprite m_standard;
        private CCSprite m_touched;
        private Action m_callback;

    }
}

