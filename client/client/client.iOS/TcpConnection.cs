namespace Client.iOS
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    /// <summary>
    /// TCP connection. Provides an TCP Connection to write/read.
    /// </summary>
    public class TcpConnection : Common.TcpConnection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client.iOS.TcpConnection"/> class.
        /// </summary>
        public TcpConnection()
        {            
        }

        /// <summary>
        /// Send the specified methodType and JSON to the TCP server.
        /// </summary>
        /// <param name="methodType">Method type.</param>
        /// <param name="json">JSON serialized Request.</param>
        /// <returns>JSON serialized Response</returns>
        public override async Task<string> SendAsync(Core.Connection.MethodType methodType, string json)
        {
            var client = new TcpClient();
            /* // decomment for debugging purpose
            try
            {
                client.Connect(
                    Client.Common.Helper.ClientConstants.DEBUG_TCP_SERVER,
                    Client.Common.Helper.ClientConstants.TCP_PORT);
            }
            catch (SocketException exception)
            {
                */
            client.Connect(
                Common.Constants.ClientConstants.TCP_SERVER,
                Common.Constants.ClientConstants.TCP_PORT); 

            // }
            var stream = client.GetStream();

            var packetOut = new Core.Connection.Packet();
            packetOut.Content = json;
            packetOut.MethodType = methodType;
            await packetOut.SendAsync(stream);
            var packetIn = await Core.Connection.Packet.ReceiveAsync(stream);

            client.Close();
            return packetIn.Content;
        }
    }
}

