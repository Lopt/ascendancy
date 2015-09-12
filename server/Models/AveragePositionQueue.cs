namespace Server.Models
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// An Class which provides an queue with an position.
    /// It is used to avoid threading collisions.
    /// </summary>
    public class AveragePositionQueue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Server.Models.AveragePositionQueue"/> class.
        /// </summary>
        public AveragePositionQueue()
        {
            m_average = new Core.Models.Position(0, 0);
            m_lock = new Mutex();

            m_queue = new Queue<Core.Models.Action>();
        }

        /// <summary>
        /// Removes an returns an action from this queue.
        /// </summary>
        /// <returns>Removed Action</returns>
        public Core.Models.Action Dequeue()
        {
            try
            {
                m_lock.WaitOne();

                var action = m_queue.Dequeue();
                action.ActionTime = DateTime.Now;
                var actionC = (Core.Controllers.Actions.Action)action.Control;
                var actionPosition = new Core.Models.Position(actionC.GetRegionPosition());

                var newAverageX = (m_average.X * (m_queue.Count + 1)) - actionPosition.X; 
                var newAverageY = (m_average.Y * (m_queue.Count + 1)) - actionPosition.Y;    

                if (!IsEmpty())
                {
                    m_average = new Core.Models.Position(newAverageX / m_queue.Count, newAverageY / m_queue.Count);
                }
                return action;
            }
            finally
            {
                m_lock.ReleaseMutex();
            }
        }

        /// <summary>
        /// Enqueue the specified action.
        /// </summary>
        /// <param name="action">Action which should be executed.</param>
        public void Enqueue(Core.Models.Action action)
        {
            try
            {   
                var actionC = (Core.Controllers.Actions.Action)action.Control;
                var actionPosition = new Core.Models.Position(actionC.GetRegionPosition());

                m_lock.WaitOne();
                var newAverageX = (m_average.X * m_queue.Count) + actionPosition.X; 
                var newAverageY = (m_average.Y * m_queue.Count) + actionPosition.Y;                            
                m_queue.Enqueue(action);  
                m_average = new Core.Models.Position(newAverageX / m_queue.Count, newAverageY / m_queue.Count);
            }
            finally
            {
                m_lock.ReleaseMutex();
            }
        }

        /// <summary>
        /// Distance of the queue to an specific position.
        /// </summary>
        /// <param name="position">Other Position.</param>
        /// <returns>Distance of queue to the specific position</returns>
        public double Distance(Core.Models.Position position)
        {
            return m_average.Distance(position);
        }

        /// <summary>
        /// Determines whether this instance is empty.
        /// </summary>
        /// <returns><c>true</c> if this instance is empty; otherwise, <c>false</c>.</returns>
        public bool IsEmpty()
        {
            return m_queue.Count == 0;
        }

        /// <summary>
        /// The average position of actions in the queue.
        /// </summary>
        private Core.Models.Position m_average;

        /// <summary>
        /// The lock.
        /// </summary>
        private Mutex m_lock;

        /// <summary>
        /// The queue.
        /// </summary>
        private Queue<Core.Models.Action> m_queue;
    }
}