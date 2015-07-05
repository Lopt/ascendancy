using System;
using System.Threading;

namespace @base.model
{
    public class IdGenerator
    {
        static int m_currentId;
        static public int GetId()
        {
            return Interlocked.Increment(ref m_currentId);
        }


    }
}

