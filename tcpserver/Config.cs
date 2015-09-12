namespace TCPServer
{
    using System;

    /// <summary>
    /// Config for server address and port
    /// </summary>
    public class Config
    {
        /// <summary>
        /// The Server Address (which he should listen, or 0.0.0.0 for everyone).
        /// </summary>
        public static readonly string SERVER = "0.0.0.0";

        /// <summary>
        /// The Server Port
        /// </summary>
        public static readonly int PORT = 13000;

        /// <summary>
        /// Maximum content size (in byte)
        /// </summary>
        public static readonly int MAX_CONTENT_SIZE = 256 * 16;
    }
}