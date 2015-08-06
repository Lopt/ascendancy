namespace Client.Common.Views.Actions
{
    using System;

    /// <summary>
    /// The View part of the Actions. So an action can be displayed.
    /// </summary>
    public class Action : Core.Views.ViewEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Views.Actions.Action"/> class.
        /// </summary>
        /// <param name="model">The action Model.</param>
        public Action(Core.Models.ModelEntity model)
            : base(model)
        {
        }

        /// <summary>
        /// Gets called before ActionControl.Do() gets executed. Should get and store data which will be needed in Schedule.
        /// </summary>
        public virtual void BeforeDo()
        {
        }

        /// <summary>
        /// Schedules the action. Should do anything do animate the action (e.g. draw the entity, animate his moving or start/end animating a fight)
        /// Returns true if the action has ended, otherwise false.
        /// </summary>
        /// <param name="frameTimesInSecond">frames times in seconds.</param>
        /// <returns>true if the schedule of the action is done</returns>
        public virtual bool Schedule(float frameTimesInSecond)
        {
            return true;
        }
    }
}