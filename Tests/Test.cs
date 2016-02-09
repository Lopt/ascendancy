﻿using NUnit.Framework;
using System;
using System.Net.Sockets;
using Core.Connection;
using TCPServer;
using Newtonsoft.Json; 
using Core.Models;

namespace Tests
{
    [TestFixture]
    public class CoreTest
    {
        [Test]
        public void LatLonTest()
        {
            var LaLo = new LatLon(50.97695325, 11.02396488);
            Assert.IsNotNull(LaLo);

            //Tests to get a Latitude and Longitude out of an GamePosition
            var pos = new Position(LaLo);
            var LaLo2 = new LatLon(pos);
            Assert.IsNotNull(LaLo2);
        }

        [Test]
        public void RegionPositionTest()
        {
            //var RegionPos = new RegionPosition();
        }

        [Test]
        public void PositionTest()
        {
        }

        [Test]
        public void PositionITest()
        {
            var Position = new PositionI(0, 0);
            Assert.IsNotNull(Position);

            //test constructor with region position and cellposition
            //Position = new PositionI();
            //Assert.IsNotNull(Position);

            //test constructor with position
        }
    }

    [TestFixture]
    public class ConnectionTest
    {
        [Test]
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

            ///False login

            testLoginRequest = new Core.Connections.LoginRequest(
                new Core.Models.Position(8108, 15),
                "Maria",
                "Musterfrauxxx");
            testJson = JsonConvert.SerializeObject(testLoginRequest);

            testPackage = new Packet();

            testPackage.Content = testJson;
            testPackage.MethodType = MethodType.Login;

            testStream = this.getStream();

            testPackage.Send(testStream);

            data = JsonConvert.DeserializeObject<Core.Connections.LoginResponse>(Packet.Receive(testStream).Content);

            Assert.AreNotEqual(Core.Connections.LoginResponse.ReponseStatus.OK, data.Status);

        }

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

            var testStream = this.getStream();

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

            testStream = this.getStream();

            testPackage.Send(testStream);

            testPackageIn = Packet.Receive(testStream);

            var data = JsonConvert.DeserializeObject<Core.Connections.Response>(testPackageIn.Content);

            Assert.AreEqual(Core.Connections.Response.ReponseStatus.OK, data.Status);
        }
            
        [Test]
        public void DoAction()
        {
            var user = this.getLogin();

            var testAccount = new Core.Models.Account(user.AccountId);

            var testDefinition = new Core.Models.Definitions.Definition(276);

            var testPositionI = new Core.Models.PositionI(this.testPosition());

            var dictParam = new System.Collections.Generic.Dictionary<string, object>();
            dictParam[Core.Controllers.Actions.CreateUnit.CREATE_POSITION] = testPositionI; 
            dictParam[Core.Controllers.Actions.CreateUnit.CREATION_TYPE] = (long)testDefinition.SubType;

            var testAction = new Core.Models.Action(testAccount, Core.Models.Action.ActionType.CreateBuilding, dictParam);

            var testActions = new Core.Models.Action[] { testAction, };

            var testActionRequest = new Core.Connections.DoActionsRequest(
                                    user.SessionID,
                                    this.testPosition(),
                                    testActions);

            var testJson = JsonConvert.SerializeObject(testActionRequest);

            var testPackage = new Packet();

            testPackage.Content = testJson;
            testPackage.MethodType = MethodType.DoActions;

            var testStream = this.getStream();

            testPackage.Send(testStream);

            var data = JsonConvert.DeserializeObject<Core.Connections.Response>(Packet.Receive(testStream).Content);

            Assert.AreEqual(Core.Connections.Response.ReponseStatus.OK, data.Status);
        }

        /// <summary>
        /// Gets a login instance.
        /// </summary>
        /// <returns>The login.</returns>
        Core.Connections.LoginResponse getLogin()
        {
            var testStream = this.getStream();

            var testLoginRequest = new Core.Connections.LoginRequest(
                                    this.testPosition(),
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
        System.Net.Sockets.NetworkStream getStream()
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
        /// <returns>The Position.</returns>
        Core.Models.Position testPosition()
        {
            return new Core.Models.Position(new Core.Models.LatLon(50.97695325, 11.02396488));
        }
    }

    [TestFixture]
    public class LogicTest
    {
        public static readonly Core.Models.PositionI[] SurroundTilesEven =
            {
                new Core.Models.PositionI(0, -1),
                new Core.Models.PositionI(1, 0),
                new Core.Models.PositionI(1, 1),
                new Core.Models.PositionI(0, 1),
                new Core.Models.PositionI(-1, 1),
                new Core.Models.PositionI(-1, 0)
            };

        /// <summary>
        /// The surround tiles on odd x positions.
        /// From North to NorthEast in clockwise
        /// </summary>
        public static readonly Core.Models.PositionI[] SurroundTilesOdd =
            {
                new Core.Models.PositionI(0, -1),
                new Core.Models.PositionI(1, -1),
                new Core.Models.PositionI(1, 0),
                new Core.Models.PositionI(0, 1),
                new Core.Models.PositionI(-1, 0),
                new Core.Models.PositionI(-1, -1)
            };
    
        [Test]
        public void GetSurroundedFields()
        {
            // Test for Even x Coordinate
            var StartPosition = new Core.Models.PositionI(0, 0);
            var Positions = Core.Models.LogicRules.GetSurroundedFields(StartPosition);


            var expectedPos = new Core.Models.PositionI[]
            {
                StartPosition + SurroundTilesOdd[0],
                StartPosition + SurroundTilesOdd[1],
                StartPosition + SurroundTilesOdd[2],
                StartPosition + SurroundTilesOdd[3],
                StartPosition + SurroundTilesOdd[4],
                StartPosition + SurroundTilesOdd[5]
            };

            Assert.IsNotEmpty(Positions);
            Assert.AreEqual(6, Positions.Length);
            Assert.AreEqual(Positions, expectedPos);

            // Test For uneven x Coordinate
            StartPosition = new Core.Models.PositionI(1, 0);
            Positions = Core.Models.LogicRules.GetSurroundedFields(StartPosition);


            expectedPos = new Core.Models.PositionI[]
                {
                    StartPosition + SurroundTilesEven[0],
                    StartPosition + SurroundTilesEven[1],
                    StartPosition + SurroundTilesEven[2],
                    StartPosition + SurroundTilesEven[3],
                    StartPosition + SurroundTilesEven[4],
                    StartPosition + SurroundTilesEven[5]
                };
            
            Assert.AreEqual(Positions, expectedPos);
        }

        [Test]
        public void GetSurroundedPositions()
        {
            var StartPosition = new Core.Models.PositionI(0, 0);
            int Range = 1;
            var Positions = Core.Models.LogicRules.GetSurroundedPositions(StartPosition, Range);

            Assert.AreEqual(Positions.Count, 7);

            Range = 2;
            Positions = Core.Models.LogicRules.GetSurroundedPositions(StartPosition, Range);

            Assert.AreEqual(Positions.Count, 19);

            Range = 3;
            Positions = Core.Models.LogicRules.GetSurroundedPositions(StartPosition, Range);

            Assert.AreEqual(Positions.Count, 37);

            Range = 4;
            Positions = Core.Models.LogicRules.GetSurroundedPositions(StartPosition, Range);

            Assert.AreEqual(Positions.Count, 61);

            Range = 5;
            Positions = Core.Models.LogicRules.GetSurroundedPositions(StartPosition, Range);

            Assert.AreEqual(Positions.Count, 91);
        }

        [Test]
        public void Storage()
        {
            
        }
    }



}