namespace Client.Common
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// TCP connection. Provides an TCP Connection to write/read.
    /// </summary>
    public class TcpConnection
    {
        /// <summary>
        /// Send the specified methodType and JSON to the TCP server.
        /// </summary>
        /// <param name="methodType">Method type.</param>
        /// <param name="json">JSON serialized Request.</param>
        /// <returns>JSON serialized Response</returns>
        public virtual async Task<string> SendAsync(Core.Connection.MethodType methodType, string json)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The connector which can be used to access send.
        /// </summary>
        public static TcpConnection Connector;
    }
}