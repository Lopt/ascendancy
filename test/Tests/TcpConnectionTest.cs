namespace test.Tests
{
    using System;
    using System.Net.Sockets;
    using NUnit.Framework;
    using Newtonsoft.Json;
    using TCPServer;
    using Client.Common;

    [TestFixture()]
    public class TcpConnectionTest
    {
        public TcpConnectionTest()
        {
        }
        [Test()]
        public void Connection()
        {
            var client = new TcpClient();
            client.Connect(
                Client.Common.Constants.ClientConstants.TCP_SERVER,
                Client.Common.Constants.ClientConstants.TCP_PORT);
            Assert.IsTrue(client.Connected);
        }
            
        [Test()]
        public async void SendAndRecive()
        {
            var testPackageOut = new Core.Connection.Packet();
            testPackageOut.Content = JsonConvert.SerializeObject("json");
            //"{\"Status\":0,\"Actions\":[[],[],[],[],[],[],[],[],[],[],[],[],[{\"Parameters\":{\"CreatePosition\":{\"X\":5316345,\"Y\":3354734},\"CreateBuilding\":276},\"Type\":2}],[],[],[],[],[],[],[],[],[],[],[],[]],\"Entities\":[]}"
            //what does Default ??
            testPackageOut.MethodType = Core.Connection.MethodType.Default;

            var client = new TcpClient();
            client.Connect(
                Client.Common.Constants.ClientConstants.TCP_SERVER,
                Client.Common.Constants.ClientConstants.TCP_PORT);

            var stream = client.GetStream();

            await testPackageOut.SendAsync(stream);

            var testPackageIn = await Core.Connection.Packet.ReceiveAsync(stream);

            Assert.AreEqual(testPackageOut.GetHashCode(), testPackageIn.GetHashCode());
        }
    }
}

