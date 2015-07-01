using System;

namespace client.Common.Views.Action
{
    public class Action : @base.view.ViewEntity
    {
        public Action(@base.model.ModelEntity model)
            : base(model)
        {
        }

        virtual public void BeforeDo()
        {
        }

        /// <summary>
        /// schedules the action. returns true if ended, otherwise false.
        /// </summary>
        virtual public bool Schedule(float frameTimesInSecond)
        {
            return true;
        }
    }
}

