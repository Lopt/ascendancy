namespace Client.Droid
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// TCP connection. Provides an TCP Connection to write/read.
    /// </summary>
    public class TcpConnection : Common.TcpConnection
    {
        public TcpConnection()
        {            
        }   
            
        public override string Send(Core.Connection.MethodType methodType, string json)
        {
            var client = new TcpClient();
            client.Connect(
                Client.Common.Helper.ClientConstants.TCP_SERVER,
                Client.Common.Helper.ClientConstants.TCP_PORT);
            var stream = client.GetStream();

            var packetOut = new Core.Connection.Packet();
            packetOut.Content = json;
            packetOut.MethodType = methodType;
            packetOut.Send(stream);
            var packetIn = Core.Connection.Packet.Receive(stream);
           
            client.Close();
            return packetIn.Content;
        }
    }
}