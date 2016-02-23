namespace Client.Common.Views.Effects
{
    using System;
    using CocosSharp;

    /// <summary>
    /// Effect node.
    /// </summary>
    public class EffectNode : CCNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.Effects.EffectNode"/> class.
        /// </summary>
        public EffectNode()
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