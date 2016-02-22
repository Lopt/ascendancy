namespace Client.Common.Views
{
    using System;
    using System.Collections.Generic;
    using Client.Common.Controllers;
    using Client.Common.Helper;
    using CocosSharp;
    using Core.Controllers.Actions;

    /// <summary>
    /// Debug layer.
    /// </summary>
    public class DebugLayer : CCLayerColor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.DebugLayer"/> class.
        /// </summary>
        public DebugLayer()
            : base()
        {
            Position = VisibleBoundsWorldspace.LowerLeft;
            AnchorPoint = CCPoint.AnchorLowerLeft;

            m_label = new CCLabel(string.Empty, "verdana", 12)
            {
                Position = VisibleBoundsWorldspace.LowerLeft,
                Color = CCColor3B.Black,
                HorizontalAlignment = CCTextAlignment.Left,
                VerticalAlignment = CCVerticalTextAlignment.Top,
                AnchorPoint = CCPoint.AnchorLowerLeft,
                Dimensions = VisibleBoundsWorldspace.Size
            };
            Color = CCColor3B.White;
            Opacity = 255;
            AddChild(m_label);
            Visible = false;

            TouchHandler.Instance.ListenEnded(this, new Func<List<CCTouch>, CCEvent, bool>(OnTouchesEnded));
            TouchHandler.Instance.ListenBegan(this, new Func<List<CCTouch>, CCEvent, bool>(OnTouchesCatch));
            TouchHandler.Instance.ListenMoved(this, new Func<List<CCTouch>, CCEvent, bool>(OnTouchesCatch));
            TouchHandler.Instance.ListenCancelled(this, new Func<List<CCTouch>, CCEvent, bool>(OnTouchesCatch));
        }

        #region overide

        /// <summary>
        /// If the touch is catched return the visbility of this layer.
        /// </summary>
        /// <returns><c>true</c> if this layer is visible; otherwise, <c>false</c>.</returns>
        /// <param name="touches">The touches list.</param>
        /// <param name="touchEvent">The touch event.</param>
        public bool OnTouchesCatch(List<CCTouch> touches, CCEvent touchEvent)
        {
            return Visible;
        }

        /// <summary>
        /// If the touch is ended return the visbility of this layer and reverse it.
        /// </summary>
        /// <returns><c>true</c> if this layer is visible; otherwise, <c>false</c>.</returns>
        /// <param name="touches">The touches list.</param>
        /// <param name="touchEvent">The touch event.</param>
        public bool OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (Visible)
            {
                Visible = !Visible;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Shows/Hides the DebugLayer.
        /// </summary>
        public void Toggle()
        {
            Visible = !Visible;

            if (Visible)
            {
                Helper.Logging.Info("Logging Layer opened");
                string output = string.Empty;

                foreach (var text in Helper.Logging.GetLog())
                {
                    output += text + "\n";
                }

                m_label.Text = output;
            }
        }

        #endregion

        /// <summary>
        /// label where all debugging output stands
        /// </summary>
        private CCLabel m_label;
    }
}