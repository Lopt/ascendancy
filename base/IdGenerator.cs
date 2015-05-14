using System;
using System.Threading;

namespace @base
{
    public class IdGenerator
    {
        static int currentId;
        static public int GetId()
        {
            return Interlocked.Increment(ref currentId);
        }
    }
}

