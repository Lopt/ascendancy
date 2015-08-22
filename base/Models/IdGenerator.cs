namespace Core.Models
{
    using System;
    using System.Threading;

    /// <summary>
    /// Generates thread safe IDs.
    /// </summary>
    public class IdGenerator
    {
        /// <summary>
        /// The current identifier.
        /// </summary>
        private static int m_currentId;

        /// <summary>
        /// Gets the identifier and increments it.
        /// </summary>
        /// <returns>An identifier.</returns>
        public static int GetId()
        {
            return Interlocked.Increment(ref m_currentId);
        }
    }
}