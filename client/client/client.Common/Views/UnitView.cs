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
            var defView = (UnitDefinitionView)model.Definition.View;
            if (defView != null)
            {
                var diplomacy = model.GetDiplomacy(GameAppDelegate.Account);
                var sprite = defView.GetSpriteCopy(diplomacy);
                //                Node.Position = Helper.PositionHelper.GamePositionIToWorldPoint(model.Position);
                sprite.Scale = 1.35f;
                sprite.AnchorPoint = new CCPoint(0.0f, 0.8f);
                Node = sprite;
                Node.Position = Helper.PositionHelper.CellToTile(model.Position.CellPosition);
                DrawRegion = model.Position.RegionPosition;
                m_healthbar = new Effects.Healthbar();
                Node.AddChild(m_healthbar);

                RefreshHealth();
                Animate(UnitAnimation.Idle);
            }
        }

        public float Animate(UnitAnimation type)
        {
            var model = (Core.Models.Entity)Model;
            var defView = (UnitDefinitionView)model.Definition.View;
            var animate = defView.GetAnimate(model.GetDiplomacy(GameAppDelegate.Account), type);

            Node.RunActionAsync(animate);
            return animate.Duration;
        }

        public void RefreshHealth()
        {
            var model = (Core.Models.Entity)Model;
            m_healthbar.UpdateHealthbar(model.HealthPercent);
        }

        public void AnimateForever(UnitAnimation type)
        {
            var model = (Core.Models.Entity)Model;
            var defView = (UnitDefinitionView)model.Definition.View;
            var animate = defView.GetAnimate(model.GetDiplomacy(GameAppDelegate.Account), type);

            Node.RunActionAsync(new CCRepeatForever(animate));
        }

        public void Die()
        {
            Node.RemoveChild(this.m_healthbar);
            Animate(UnitAnimation.Die);
            Node.ScheduleOnce(RemoveUnit, Constants.ViewConstants.UnitView.DEATH_LYING_AROUD_TIME);
        }

        private void RemoveUnit(float time)
        {
            Node.RemoveFromParent();
        }

        public void StopAnimation()
        {
            Node.StopAllActions();
        }

        public Core.Models.RegionPosition DrawRegion
        {
            get;
            set;
        }

        public CCPoint DrawPoint
        {
            get
            {
                return Node.Position;
            }

            set
            {
                Node.Position = value;
            }
        }

        public CCNode Node
        {
            get;
            private set;
        }

        private Effects.Healthbar m_healthbar;
    }
}

