using System;
using System.Threading;
using System.Collections.Generic;

namespace Server.Models
{

    /// <summary>
    /// An Class which provides an queue with an position.
    /// It is used to avoid threading collisions.
    /// </summary>
    public class AveragePositionQueue
    {
        public AveragePositionQueue()
        {
            m_average = new Core.Models.Position(0, 0);
            m_lock = new Mutex();

            m_queue = new Queue<Core.Models.Action>();
        }

        public Core.Models.Action Dequeue()
        {
            try
            {
                m_lock.WaitOne();

                var action = m_queue.Dequeue();
                action.ActionTime = DateTime.Now;
                var actionC = (Core.Controllers.Actions.Action) action.Control;
                var actionPosition = new Core.Models.Position(actionC.GetRegionPosition());

                var newAverageX = m_average.X * (m_queue.Count + 1) - actionPosition.X; 
                var newAverageY = m_average.Y * (m_queue.Count + 1) - actionPosition.Y;    

                if (!IsEmpty())
                {
                    m_average = new Core.Models.Position(newAverageX /  m_queue.Count, newAverageY / m_queue.Count);
                }
                return action;
            }
            finally
            {
                m_lock.ReleaseMutex();
            }

        }


        public void Enqueue(Core.Models.Action action)
        {
            try
            {   
                var actionC = (Core.Controllers.Actions.Action)action.Control;
                var actionPosition = new Core.Models.Position(actionC.GetRegionPosition());

                m_lock.WaitOne();
                var newAverageX = m_average.X * m_queue.Count + actionPosition.X; 
                var newAverageY = m_average.Y * m_queue.Count + actionPosition.Y;                            
                m_queue.Enqueue(action);  
                m_average = new Core.Models.Position(newAverageX / m_queue.Count, newAverageY / m_queue.Count);
            }
            finally
            {
                m_lock.ReleaseMutex();
            }
        }

        public double Distance(Core.Models.Position position)
        {
            return m_average.Distance(position);
        }

        public bool IsEmpty()
        {
            return m_queue.Count == 0;
        }

        private Core.Models.Position m_average;
        private Mutex m_lock;
        private Queue<Core.Models.Action> m_queue;
    }
}

