using System;
using CocosSharp;
using Client.Common.Constants;

namespace Client.Common.Views
{
    public class UnitView : Core.Views.ViewEntity
    {
        
        public UnitView(Core.Models.Entity model)
            : base(model)
        {
            var defView = (UnitDefinitionView) model.Definition.View;

            if (defView != null)
            {
                var sprite = defView.GetSpriteCopy();
                //                Node.Position = Helper.PositionHelper.GamePositionIToWorldPoint(model.Position);
                sprite.Scale = 1.35f;
                sprite.AnchorPoint = new CCPoint(0.0f, 0.8f);
                Node = sprite;

                Animate(UnitAnimation.Idle);
            }
        }

        public void Animate(UnitAnimation type)
        {
            var model = (Core.Models.Entity)Model;
            var defView = (UnitDefinitionView)model.Definition.View;
            var animate = defView.GetAnimate(type);

            Node.RunAction(animate);
        }

        public void AnimateForever(UnitAnimation type)
        {
            var model = (Core.Models.Entity)Model;
            var defView = (UnitDefinitionView)model.Definition.View;
            var animate = defView.GetAnimate(type);

            Node.RunAction(new CCRepeatForever(animate));
        }

        public void StopAnimation()
        {
            Node.StopAllActions();
        }

        public CCNode Node
        {
            get;
            private set;
        }
    }
}

