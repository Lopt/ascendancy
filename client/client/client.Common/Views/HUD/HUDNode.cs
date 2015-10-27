namespace Client.Common.Views.HUD
{
    using System;
    using CocosSharp;

    /// <summary>
    /// HUD node.
    /// </summary>
    public class HUDNode : CCNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.HUD.HUDNode"/> class.
        /// </summary>
        public HUDNode()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets the anchor point.
        /// </summary>
        /// <value>The anchor point.</value>
        public override CCPoint AnchorPoint
        {
            get
            {
                return base.AnchorPoint;
            }

            set
            {
                base.AnchorPoint = value;
                foreach (var child in Children)
                {
                    child.AnchorPoint = value;
                }
            }
        }
    }
}