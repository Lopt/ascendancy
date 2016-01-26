using NUnit.Framework;
using System;
using System.Net.Sockets;
using Core.Connection;
using TCPServer;
using Newtonsoft.Json; 

namespace Tests
{
    [TestFixture()]
    public class ConnectionTest
    {
        [Test()]
        public void Connection()
        {
            var client = new TcpClient();
            client.Connect(
                Client.Common.Constants.ClientConstants.TCP_SERVER,
                Client.Common.Constants.ClientConstants.TCP_PORT);
            Assert.True(client.Connected);
        }

        [Test]
        public void SendAndRecive()
        {
            var testPackageOut = new Core.Connection.Packet();
            testPackageOut.Content = JsonConvert.SerializeObject("{\"Status\":0,\"Actions\":[[],[],[],[],[],[],[],[],[],[],[],[],[{\"Parameters\":{\"CreatePosition\":{\"X\":5316345,\"Y\":3354734},\"CreateBuilding\":276},\"Type\":2}],[],[],[],[],[],[],[],[],[],[],[],[]],\"Entities\":[]}");
            //"{\"Status\":0,\"Actions\":[[],[],[],[],[],[],[],[],[],[],[],[],[{\"Parameters\":{\"CreatePosition\":{\"X\":5316345,\"Y\":3354734},\"CreateBuilding\":276},\"Type\":2}],[],[],[],[],[],[],[],[],[],[],[],[]],\"Entities\":[]}"
            //what does Default ??
            testPackageOut.MethodType = Core.Connection.MethodType.DoActions;

            var client = new TcpClient();
            client.Connect(
                Client.Common.Constants.ClientConstants.TCP_SERVER,
                Client.Common.Constants.ClientConstants.TCP_PORT);

            var stream = client.GetStream();

            testPackageOut.Send(stream);

            var testPackageIn = Core.Connection.Packet.Receive(stream);

            Assert.AreNotEqual(testPackageOut.GetHashCode(), testPackageIn.GetHashCode());
        }

        [Test]
        public void http()
        {
            
        }
    }
}