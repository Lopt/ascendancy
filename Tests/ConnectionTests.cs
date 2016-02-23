namespace Tests
{
    using System;
    using System.Net.Sockets;
    using Core.Connection;
    using Core.Models;
    using Newtonsoft.Json;
    using NUnit.Framework;

    /// <summary>
    /// Connection tests.
    /// </summary>
    [TestFixture]
    public class ConnectionTests
    {
        /// <summary>
        /// Tests if a Connection between client and server is established.
        /// </summary>
        [Test]
        public void Connection()
        {
            var client = new TcpClient();
            client.Connect(
                Client.Common.Constants.ClientConstants.TCP_SERVER,
                Client.Common.Constants.ClientConstants.TCP_PORT);
            Assert.True(client.Connected);
        }

        /// <summary>
        /// Tests if their is a Package exchange between server and client.
        /// </summary>
        [Test]
        public void SendAndRecive()
        {
            var testPackageOut = new Core.Connection.Packet();
            testPackageOut.Content = JsonConvert.SerializeObject("{\"Status\":0,\"Actions\":[[],[],[],[],[],[],[],[],[],[],[],[],[{\"Parameters\":{\"CreatePosition\":{\"X\":5316345,\"Y\":3354734},\"CreateBuilding\":276},\"Type\":2}],[],[],[],[],[],[],[],[],[],[],[],[]],\"Entities\":[]}");
            testPackageOut.MethodType = Core.Connection.MethodType.DoActions;

            var client = new TcpClient();
            client.Connect(
                Client.Common.Constants.ClientConstants.TCP_SERVER,
                Client.Common.Constants.ClientConstants.TCP_PORT);

            var stream = client.GetStream();

            testPackageOut.Send(stream);

            var testPackageIn = Core.Connection.Packet.Receive(stream);

            client.Close();

            Assert.True(!client.Connected);

            Assert.AreNotEqual(testPackageOut.GetHashCode(), testPackageIn.GetHashCode());
        }

        /// <summary>
        /// Tests the Login .
        /// </summary>
        [Test]
        public void Login()
        {
            var testLoginRequest = new Core.Connections.LoginRequest(
                                        new Core.Models.Position(8108, 15),
                                        "Maria",
                                        "Musterfrau");
            var testJson = JsonConvert.SerializeObject(testLoginRequest);

            var testPackage = new Packet();

            testPackage.Content = testJson;
            testPackage.MethodType = MethodType.Login;

            var client = new TcpClient();
            client.Connect(
                Client.Common.Constants.ClientConstants.TCP_SERVER,
                Client.Common.Constants.ClientConstants.TCP_PORT);

            Assert.True(client.Connected);

            var testStream = client.GetStream();

            testPackage.Send(testStream);

            var testPackageIn = Packet.Receive(testStream);

            client.Close();

            Assert.True(!client.Connected);

            var data = JsonConvert.DeserializeObject<Core.Connections.LoginResponse>(testPackageIn.Content);

            Assert.AreEqual(Core.Connections.LoginResponse.ReponseStatus.OK, data.Status);
            Assert.IsNotNull(data.Status);
            Assert.IsNotNull(data.AccountId);

            // Test False logins
            testLoginRequest = new Core.Connections.LoginRequest(
                new Core.Models.Position(8108, 15),
                "Maria",
                "Musterfrauxxx");
            testJson = JsonConvert.SerializeObject(testLoginRequest);

            testPackage = new Packet();

            testPackage.Content = testJson;
            testPackage.MethodType = MethodType.Login;

            testStream = this.GetStream();

            testPackage.Send(testStream);

            data = JsonConvert.DeserializeObject<Core.Connections.LoginResponse>(Packet.Receive(testStream).Content);

            Assert.AreNotEqual(Core.Connections.LoginResponse.ReponseStatus.OK, data.Status);
        }

        /// <summary>
        /// Tests if a Region is send form the server.
        /// </summary>
        [Test]
        public void LoadRegion()
        {
            var testLoginRequest = new Core.Connections.LoginRequest(
                                        new Core.Models.Position(100, 100),
                                        "Maria",
                                        "Musterfrau");
            var testJson = JsonConvert.SerializeObject(testLoginRequest);

            var testPackage = new Packet();

            testPackage.Content = testJson;
            testPackage.MethodType = MethodType.Login;

            var testStream = this.GetStream();

            testPackage.Send(testStream);

            var testPackageIn = Packet.Receive(testStream);

            var loginData = JsonConvert.DeserializeObject<Core.Connections.LoginResponse>(testPackageIn.Content);

            var testRegion = new Core.Models.RegionPosition[]
            {
                    new Core.Models.RegionPosition(100, 100),
            };

            var testRequest = new Core.Connections.LoadRegionsRequest(
                                  loginData.SessionID,
                                  new Core.Models.Position(100, 100),
                                  testRegion);

            testJson = JsonConvert.SerializeObject(testRequest);

            testPackage = new Packet();

            testPackage.Content = testJson;
            testPackage.MethodType = MethodType.LoadEntities;

            testStream = this.GetStream();

            testPackage.Send(testStream);

            testPackageIn = Packet.Receive(testStream);

            var data = JsonConvert.DeserializeObject<Core.Connections.Response>(testPackageIn.Content);

            Assert.AreEqual(Core.Connections.Response.ReponseStatus.OK, data.Status);
        }
            
        /// <summary>
        /// Tests if an action is send to the server.
        /// </summary>
        [Test]
        public void DoAction()
        {
            var user = this.GetLogin();

            var testAccount = new Core.Models.Account(user.AccountId);

            var testDefinition = new Core.Models.Definitions.Definition(276);

            var testPositionI = new Core.Models.PositionI(this.TestPosition());

            var dictParam = new System.Collections.Generic.Dictionary<string, object>();
            dictParam[Core.Controllers.Actions.CreateUnit.CREATE_POSITION] = testPositionI; 
            dictParam[Core.Controllers.Actions.CreateUnit.CREATION_TYPE] = (long)testDefinition.SubType;

            var testAction = new Core.Models.Action(testAccount, Core.Models.Action.ActionType.CreateBuilding, dictParam);

            var testActions = new Core.Models.Action[] { testAction, };

            var testActionRequest = new Core.Connections.DoActionsRequest(
                                    user.SessionID,
                                    this.TestPosition(),
                                    testActions);

            var testJson = JsonConvert.SerializeObject(testActionRequest);

            var testPackage = new Packet();

            testPackage.Content = testJson;
            testPackage.MethodType = MethodType.DoActions;

            var testStream = this.GetStream();

            testPackage.Send(testStream);

            var data = JsonConvert.DeserializeObject<Core.Connections.Response>(Packet.Receive(testStream).Content);

            Assert.AreEqual(Core.Connections.Response.ReponseStatus.OK, data.Status);
        }

        /// <summary>
        /// Gets a login instance.
        /// </summary>
        /// <returns>The login.</returns>
        private Core.Connections.LoginResponse GetLogin()
        {
            var testStream = this.GetStream();

            var testLoginRequest = new Core.Connections.LoginRequest(
                                    this.TestPosition(),
                                    "Maria",
                                    "Musterfrau");
            var testJson = JsonConvert.SerializeObject(testLoginRequest);

            var testPackage = new Packet();

            testPackage.Content = testJson;
            testPackage.MethodType = MethodType.Login;

            testPackage.Send(testStream);

            return JsonConvert.DeserializeObject<Core.Connections.LoginResponse>(Packet.Receive(testStream).Content);
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <returns>The stream.</returns>
        private System.Net.Sockets.NetworkStream GetStream()
        {
            var client = new TcpClient();

            client.Connect(
                Client.Common.Constants.ClientConstants.TCP_SERVER,
                Client.Common.Constants.ClientConstants.TCP_PORT);

            return client.GetStream();
        }

        /// <summary>
        /// Gets the TestPosition
        /// </summary>
        /// <returns> The Test Position.</returns>
        private Core.Models.Position TestPosition()
        {
            return new Core.Models.Position(new Core.Models.LatLon(50.97695325, 11.02396488));
        }
    }
}