using System;

namespace client.Common.Views.Action
{
    public class Action : Core.Views.ViewEntity
    {
        public Action(Core.Models.ModelEntity model)
            : base(model)
        {
        }

        /// <summary>
        /// Gets called before ActionControl.Do() gets executed. Should get and store data which will be needed in Schedule.
        /// </summary>
        virtual public void BeforeDo()
        {
        }

        /// <summary>
        /// Schedules the action. Should do anything do animate the action (e.g. draw the entity, animate his moving or start/end animating a fight)
        /// Returns true if the action has ended, otherwise false.
        /// </summary>
        /// <param name="frameTimesInSecond">frames times in seconds.</param>
        virtual public bool Schedule(float frameTimesInSecond)
        {
            return true;
        }
    }
}

