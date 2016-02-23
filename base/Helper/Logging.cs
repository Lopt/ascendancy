namespace Client.Common.Helper
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Logging class. To log any relevant (or irrelevant) information about the current state of the code.
    /// </summary>
    public class Logging
    {
        /// <summary>
        /// Mode. Relevance in this order:
        /// Debug
        /// Info
        /// Warning
        /// Error
        /// Critical
        /// </summary>
        public enum Mode
        {
            Debug,
            Info,
            Warning,
            Error,
            Critical
        }

        /// <summary>
        /// sets the minimum mode which should be logged.
        /// </summary>
        /// <param name="mode">Logging Mode.</param>
        public static void BasicConfig(Mode mode)
        {
            m_mode = mode;
        }

        /// <summary>
        /// Debug Log. Detailed information, typically of interest only when diagnosing problems.
        /// </summary>
        /// <param name="text">Logging Text.</param>
        public static void Debug(string text)
        {
            Log(text, Mode.Debug);
        }

        /// <summary>
        /// Confirmation that things are working as expected.
        /// </summary>
        /// <param name="text">Logging Text.</param>
        public static void Info(string text)
        {   
            Log(text, Mode.Info);
        }

        /// <summary>
        /// An indication that something unexpected happened, or indicative of some problem in the near future (e.g. ‘disk space low’).
        /// The software is still working as expected.
        /// </summary>
        /// <param name="text">Logging Text.</param>
        public static void Warning(string text)
        {   
            Log(text, Mode.Warning);
        }

        /// <summary>
        /// Due to a more serious problem, the software has not been able to perform some function.
        /// </summary>
        /// <param name="text">Logging Text.</param>
        public static void Error(string text)
        {   
            Log(text, Mode.Error);
        }

        /// <summary>
        /// A serious error, indicating that the program itself may be unable to continue running.
        /// </summary>
        /// <param name="text">Logging Text.</param>
        public static void Critical(string text)
        {   
            Log(text, Mode.Critical);
        }

        /// <summary>
        /// Gets the log list (copy).
        /// </summary>
        /// <returns>a copy of the log list.</returns>
        public static List<string> GetLog()
        {
            lock (m_lock)
            {
                return new List<string>(m_log);
            }
        }

        /// <summary>
        /// Logging with the specified mode.
        /// </summary>
        /// <param name="text">Log Text.</param>
        /// <param name="mode">Log Mode.</param>
        private static void Log(string text, Mode mode)
        {
            if (mode >= m_mode)
            {
                lock (m_lock)
                {
                    m_log.Add(mode.ToString() + " | " + text);
                }
            }
        }

        /// <summary>
        /// The lock.
        /// </summary>
        private static object m_lock = new object();

        /// <summary>
        /// The log.
        /// </summary>
        private static List<string> m_log = new List<string>();

        /// <summary>
        /// The mode.
        /// </summary>
        private static Mode m_mode = Mode.Debug;
    }
}