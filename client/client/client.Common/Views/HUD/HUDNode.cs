namespace Client.Common.Views.HUD
{
    using System;
    using CocosSharp;

    public class HUDNode : CCNode
    {
        public HUDNode()
            : base()
        {
        }

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

