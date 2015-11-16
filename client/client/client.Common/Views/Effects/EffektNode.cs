namespace Client.Common.Views.Effects
{
    using System;
    using CocosSharp;
   
    public class EffectNode : CCNode
    {
        public EffectNode()
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