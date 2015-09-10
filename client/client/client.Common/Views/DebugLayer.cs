namespace Client.Common.Views
{
    using System;
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
        /// Initializes a new instance of the <see cref="Client.Common.Views.LogoLayer"/> class.
        /// </summary>
        /// <param name="startScene">Start scene.</param>
        public DebugLayer()
            : base()
        {
            m_label = new CCLabel ("", "verdana", 12) {
                Position = VisibleBoundsWorldspace.LowerLeft,
                Color = CCColor3B.Black,
                HorizontalAlignment = CCTextAlignment.Left,
                VerticalAlignment = CCVerticalTextAlignment.Top,
                AnchorPoint = CCPoint.AnchorLowerLeft,
                Dimensions = VisibleBoundsWorldspace.Size//new CCSize(200, 200)
            };

            Position = VisibleBoundsWorldspace.LowerLeft;
            AnchorPoint = CCPoint.AnchorLowerLeft;
            Color = CCColor3B.White;
            Opacity = 255;
            Visible = false;
            AddChild(m_label);
        }

        #region overide

        /// <summary>
        /// Shows/Hides the DebugLayer.
        /// </summary>
        public void Toggle()
        {
            Visible = !Visible;

            if (Visible)
            {
                Helper.Logging.Info("Logging Layer opened");
                string output = "";

                foreach (var text in Helper.Logging.GetLog())
                {
                    output += text + "\n";
                }

                m_label.Text = output;
            }
        }

        #endregion

        private CCLabel m_label;



    }
}