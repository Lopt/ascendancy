namespace Client.Common.Views
{
    using System;
    using Client.Common.Constants;
    using CocosSharp;

    /// <summary>
    /// The unit view entity.
    /// </summary>
    public class UnitView : Core.Views.ViewEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.UnitView"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public UnitView(Core.Models.Entity model)
            : base(model)
        {
            var defView = (UnitDefinitionView)model.Definition.View;
            if (defView != null)
            {
                var diplomacy = model.GetDiplomacy(GameAppDelegate.Account);
                var sprite = defView.GetSpriteCopy(diplomacy);
                // Node.Position = Helper.PositionHelper.GamePositionIToWorldPoint(model.Position);
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

        /// <summary>
        /// Animate the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The animation duration </returns>
        public float Animate(UnitAnimation type)
        {
            var model = (Core.Models.Entity)Model;
            var defView = (UnitDefinitionView)model.Definition.View;
            var animate = defView.GetAnimate(model.GetDiplomacy(GameAppDelegate.Account), type);

            Node.RunActionAsync(animate);
            return animate.Duration;
        }

        /// <summary>
        /// Refreshs the health.
        /// </summary>
        public void RefreshHealth()
        {
            var model = (Core.Models.Entity)Model;
            m_healthbar.UpdateHealthbar(model.HealthPercent);
        }

        /// <summary>
        /// Animation for the type.
        /// </summary>
        /// <param name="type">The type.</param>
        public void AnimateForever(UnitAnimation type)
        {
            var model = (Core.Models.Entity)Model;
            var defView = (UnitDefinitionView)model.Definition.View;
            var animate = defView.GetAnimate(model.GetDiplomacy(GameAppDelegate.Account), type);

            Node.RunActionAsync(new CCRepeatForever(animate));
        }

        /// <summary>
        /// the die animation.
        /// </summary>
        public void Die()
        {
            Node.RemoveChild(this.m_healthbar);
            Animate(UnitAnimation.Die);
            Node.ScheduleOnce(RemoveUnit, Constants.ViewConstants.UnitView.DEATH_LYING_AROUD_TIME);
        }

        /// <summary>
        /// Stops the animation.
        /// </summary>
        public void StopAnimation()
        {
            Node.StopAllActions();
        }

        /// <summary>
        /// Gets or sets the draw region.
        /// </summary>
        /// <value>The draw region.</value>
        public Core.Models.RegionPosition DrawRegion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the draw point.
        /// </summary>
        /// <value>The draw point.</value>
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

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <value>The node.</value>
        public CCNode Node
        {
            get;
            private set;
        }

        /// <summary>
        /// Removes the unit.
        /// </summary>
        /// <param name="time">The time.</param>
        private void RemoveUnit(float time)
        {
            Node.RemoveFromParent();
        }

        /// <summary>
        /// The healthbar.
        /// </summary>
        private Effects.Healthbar m_healthbar;
    }
}
