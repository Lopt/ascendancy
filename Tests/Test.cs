using NUnit.Framework;
using System;
using System.Net.Sockets;
using Core.Connection;
using TCPServer;
using Newtonsoft.Json; 
using Core.Models;

namespace Tests
{
    [TestFixture]
    public class PositionTests
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

            //Tests if the Latitude and Longitude are the same till the 4. digit after conversion
            var Lat = LaLo.Lat * 10000;
            var Lon = LaLo.Lon * 10000;

            var Lat2 = LaLo2.Lat * 10000;
            var Lon2 = LaLo2.Lon * 10000;

            Assert.AreEqual((int)Lat,(int)Lat2);
            Assert.AreEqual((int)Lon,(int)Lon2);

        }

        [Test]
        public void CellPositionTest()
        {
            //standart constructor
            var CellPos = new CellPosition(0, 0);
            Assert.IsNotNull(CellPos);

            //Constructor CellPosition out of Position
            var Position = new Position(0, 0);
            CellPos = new CellPosition(Position);
            Assert.IsNotNull(CellPos);

            //Constructor CellPosition out of PositionI
            var PositionI = new PositionI(0, 0);
            CellPos = new CellPosition(PositionI);
            Assert.IsNotNull(CellPos);

            //Tests the == operator
            var CellPos2 = new CellPosition(0, 0);
            Assert.True(CellPos == CellPos2);

            //Tests the Equas function
            Assert.True(CellPos.Equals(CellPos2));

            //Tests the != operator
            CellPos2 = new CellPosition(1, 1);
            Assert.True(CellPos != CellPos2);

            //Tests if its Posible to create a cell position bigger than 31 and smaller then 0
            CellPos = new CellPosition(32, 32);
            Assert.AreNotEqual(32, CellPos.CellX);
            Assert.AreNotEqual(32, CellPos.CellY);

            CellPos = new CellPosition(-1, -1);
            Assert.AreNotEqual(-1, CellPos.CellX);
            Assert.AreNotEqual(-1, CellPos.CellY);

        }

        [Test]
        public void RegionPositionTest()
        {
            //standart constructor
            var RegionPos = new RegionPosition(0, 0);
            Assert.IsNotNull(RegionPos);

            //Constructor RegionPosition out of a Position
            var Position = new Position(0, 0);
            RegionPos = new RegionPosition(Position);
            Assert.IsNotNull(RegionPos);

            //Constructor RegionPosition out of a PositionI
            var PositionI = new PositionI(0, 0);
            RegionPos = new RegionPosition(PositionI);
            Assert.IsNotNull(RegionPos);

            //Tests the + Operator
            var RegionPos2 = new RegionPosition(1, 1);
            RegionPos += RegionPos2;
            Assert.AreEqual(RegionPos2, RegionPos);

            //Tests the == Operator
            Assert.True(RegionPos == RegionPos2);

            //Tests the Equals funktion
            Assert.True(RegionPos.Equals(RegionPos2));


        }

        [Test]
        public void PositionTest()
        {
            //Standart Constructor
            var Position = new Position(0,0);
            Assert.IsNotNull(Position);

            //Constructor Position out of PositionI
            Position = new Position(new PositionI(0,0));
            Assert.IsNotNull(Position);

            //Constructor Position out of Latitude and Longitude
            Position = new Position(new LatLon(50.97695325, 11.02396488));
            Assert.IsNotNull(Position);

            //Constructor Position out of a ReagionPosition
            var RegionPos = new RegionPosition(0, 0);
            Position = new Position(RegionPos);
            Assert.IsNotNull(Position);

            //Constructor Position out of a RegionPosition and a CellPosition
            var CellPos = new CellPosition(0, 0);
            Position = new Position(RegionPos, CellPos);
            Assert.IsNotNull(Position);

            //Constructor Position out of a RegionPositionX, a RegionPositionY, a CellPositionX and a CellPositionY
            Position = new Position(RegionPos.RegionX, RegionPos.RegionY, CellPos.CellX, CellPos.CellY);
            Assert.IsNotNull(Position);

            //tests the + Operator
            Position = new Position(new LatLon(50.97695325, 11.02396488));
            var Position2 = new Position(0, 0);

            Position2 += Position;

            Assert.AreEqual(Position, Position2);


            //tests the - Operator
            Position -= Position2;

            Assert.AreNotEqual(Position2, Position);
            Assert.AreEqual(new Position(0, 0), Position);

            //tests the == Operator
            Assert.True(new Position(0, 0) == Position);

            //tests the Equals function
            Assert.True(Position.Equals(new Position(0, 0)));

            //test Distance with Position
            var dist = Position.Distance(Position2);
            Assert.IsNotNull(dist);


            //test Distance with PositionI
            var dist2 = Position2.Distance(new PositionI(0, 0));
            Assert.IsNotNull(dist2);
            Assert.AreEqual(dist, dist2);
        }

        [Test]
        public void PositionITest()
        {
            //standart Constructor
            var Position = new PositionI(0, 0);
            Assert.IsNotNull(Position);

            //Constructor Position out of a ReagionPosition
            var RegionPos = new RegionPosition(0, 0);
            var CellPos = new CellPosition(0, 0);
            Position = new PositionI(RegionPos, CellPos);
            Assert.IsNotNull(Position);

            Position = new PositionI(new Position(0, 0));
            Assert.IsNotNull(Position);

            //tests the + Operator
            Position = new PositionI(1, 1);
            var Position2 = new PositionI(0, 0);
            Position2 += Position;

            Assert.AreEqual(Position, Position2);

            //tests the - Operator
            Position -= Position2;

            Assert.AreNotEqual(Position2, Position);
            Assert.AreEqual(new PositionI(0, 0), Position);

            //tests the == Operator
            Assert.True(new PositionI(0, 0) == Position);

            //tests the Equals function
            Assert.True(Position.Equals(new PositionI(0, 0)));

            //test Distance with PositionI
            var dist = Position.Distance(Position2);
            Assert.IsNotNull(dist);

            //test Distance with PositionI
            var dist2 = Position2.Distance(new Position(0, 0));
            Assert.IsNotNull(dist2);
            Assert.AreEqual(dist, dist2);
        }
    }

    [TestFixture]
    public class CompressionHelperTests
    {
        [Test]
        public void CompressionDecompression()
        {
            //Test for a single Word
            string input = "Teststring";
            var inputbytes = System.Text.Encoding.UTF8.GetBytes(input);
            var Bytes = Core.Helper.CompressionHelper.Compress(inputbytes);
            var Output = Core.Helper.CompressionHelper.Decompress(Bytes);
            Assert.AreEqual(inputbytes, Output);
            Assert.AreNotEqual(Bytes, inputbytes);

            //Test for an long string
            input = "this is a long text who serves as a test for the compressionHelper class";
            inputbytes = System.Text.Encoding.UTF8.GetBytes(input);
            Bytes = Core.Helper.CompressionHelper.Compress(inputbytes);
            Output = Core.Helper.CompressionHelper.Decompress(Bytes);
            Assert.AreEqual(inputbytes, Output);
            Assert.AreNotEqual(Bytes, inputbytes);

            //Test vor an Very long String
            input = "this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something";
            inputbytes = System.Text.Encoding.UTF8.GetBytes(input);
            Bytes = Core.Helper.CompressionHelper.Compress(inputbytes);
            Output = Core.Helper.CompressionHelper.Decompress(Bytes);
            Assert.AreEqual(inputbytes, Output);
            Assert.AreNotEqual(Bytes, inputbytes);

            //Test for a long string with additional format comands
            input = "this is a long text who serves as a test for the compressionHelper class\t\tthis is a long text who serves as a test for the compressionHelper class";
            inputbytes = System.Text.Encoding.UTF8.GetBytes(input);
            Bytes = Core.Helper.CompressionHelper.Compress(inputbytes);
            Output = Core.Helper.CompressionHelper.Decompress(Bytes);
            Assert.AreEqual(inputbytes, Output);
            Assert.AreNotEqual(Bytes, inputbytes);

            //Test for an Empty string
            input = "";
            inputbytes = System.Text.Encoding.UTF8.GetBytes(input);
            Bytes = Core.Helper.CompressionHelper.Compress(inputbytes);
            Output = Core.Helper.CompressionHelper.Decompress(Bytes);
            Assert.AreEqual(inputbytes, Output);
            Assert.AreNotEqual(Bytes, inputbytes);

            //Test for own Byte data
            byte[] Bytedata =
                {
                    1,
                    2,
                    3,
                    15,
                    16,
                    189,
                    200,
                    255,
                    69,
                    89,
                    79,
                    69,
                    49,
                    59,
                    69,
                    39,
                    1,
                    159,
                    35,
                    57,
                    179,
                    123,
                    198,
                    169,
                };
            Bytes = Core.Helper.CompressionHelper.Compress(Bytedata);
            Output = Core.Helper.CompressionHelper.Decompress(Bytes);
            Assert.AreEqual(Bytedata, Output);
            Assert.AreNotEqual(Bytes, inputbytes);
        }
    }

    [TestFixture]
    public class LoadHelperTests
    {
        [Test]
        public void ReplacePath()
        {
            var regionPosition = new RegionPosition(new Position(new LatLon(50.97695325, 11.02396488)));
            var newPath = Core.Helper.LoadHelper.ReplacePath(Server.Models.ServerConstants.REGION_FILE, regionPosition);
            Assert.IsNotNull(newPath);
        }

        [Test]
        public void JsonToTerrain()
        {
            var regionPosition = new RegionPosition(new Position(new LatLon(50.97695325, 11.02396488)));
            var newPath = Core.Helper.LoadHelper.ReplacePath(Server.Models.ServerConstants.REGION_FILE, regionPosition);
            //var json = await Core.RequestAsync(newpath);

        }

        [Test]
        public void JsonToRegion()
        {
            
        }
    }

    [TestFixture]
    public class DefinitionsTests
    {
        [Test]
        public void TerrainDefinitions()
        {
            
            int[] Res = {0, 0, 0, 0 ,0};
            var Def = new Core.Models.Definitions.TerrainDefinition(Core.Models.Definitions.EntityType.Grassland, Res, true, true, 4, 5, 6);
            Assert.IsNotNull(Def);

            Assert.IsInstanceOf<Core.Models.Definitions.TerrainDefinition>(Def);

            Assert.AreEqual(Core.Models.Definitions.Category.Terrain, Def.Category);
            Assert.AreEqual(Core.Models.Definitions.EntityType.Grassland, Def.SubType);
            Assert.AreEqual(3, Def.ID);

            Assert.AreEqual(5, Def.Resources.Length);
            Assert.AreEqual(true, Def.Buildable);
            Assert.AreEqual(true, Def.Walkable);
            Assert.AreEqual(4, Def.TravelCost);
            Assert.AreEqual(5, Def.DefenseModifier);
            Assert.AreEqual(6, Def.AttackModifier);


            //Test if a Wrong entity could be a TerrainDefinition
            Def = new Core.Models.Definitions.TerrainDefinition(Core.Models.Definitions.EntityType.Archer, Res, true, true, 4, 5, 6);
            Assert.IsNotNull(Def);

            Assert.IsInstanceOf<Core.Models.Definitions.TerrainDefinition>(Def);

            Assert.AreNotEqual(Core.Models.Definitions.Category.Terrain, Def.Category);
            //Assert.AreEqual(Core.Models.Definitions.EntityType.Grassland, Def.SubType);
            //Assert.AreEqual(78, Def.ID);
        }

        [Test]
        public void UnitDefinitions()
        {
            string[] action = { };
            var Def = new Core.Models.Definitions.UnitDefinition(Core.Models.Definitions.EntityType.Archer, action, 1, 1, 100, 10, 2, 2, 100, 50, 0, 0);
            Assert.IsNotNull(Def);

            Assert.IsInstanceOf<Core.Models.Definitions.UnitDefinition>(Def);

            Assert.AreEqual(Core.Models.Definitions.Category.Unit, Def.Category);
            Assert.AreEqual(Core.Models.Definitions.EntityType.Archer, Def.SubType);
            Assert.AreEqual(78, Def.ID);

            Assert.AreEqual(1, Def.Attack);
            Assert.AreEqual(1, Def.Defense);
            Assert.AreEqual(100, Def.Health);
            Assert.AreEqual(10, Def.Moves);
            Assert.AreEqual(2, Def.AttackRange);
            Assert.AreEqual(2, Def.Population);
            Assert.AreEqual(100, Def.Scrapecost);
            Assert.AreEqual(50, Def.Energycost);
            Assert.AreEqual(0, Def.Plutoniumcost);
            Assert.AreEqual(0, Def.Techcost);



            //Test if a Terrain Entity could be a Unit Definition
            Def = new Core.Models.Definitions.UnitDefinition(Core.Models.Definitions.EntityType.Grassland, action, 1, 1, 100, 10, 2, 2, 100, 50, 0, 0);
            Assert.IsNotNull(Def);

            Assert.IsInstanceOf<Core.Models.Definitions.UnitDefinition>(Def);
            Assert.AreNotEqual(Core.Models.Definitions.Category.Unit, Def.Category);



        }
    }

    [TestFixture]
    public class MapRegionTests
    {
        [Test]
        public void MapRegion()
        {
            //Test with a RegionPosition as Input
            var regionPosition = new RegionPosition(new Position(new LatLon(50.97695325, 11.02396488)));
            var region = new Core.Models.Region(regionPosition);

            Assert.IsNotNull(region);
            Assert.IsInstanceOf<Core.Models.Region>(region);

            //Test with a Region as Input
            var region2 = new Region(region);

            Assert.IsNotNull(region2);
            Assert.IsInstanceOf<Core.Models.Region>(region2);
            //Assert.AreEqual(region, region2);

            //Test with a RegionPosition and an TerrainDefinition as Input
            int[] Res = {0, 0, 0, 0 ,0};
            var TerDef = new Core.Models.Definitions.TerrainDefinition(Core.Models.Definitions.EntityType.Grassland, Res, true, true, 4, 5, 6);
            //var region3 = new Region(regionPosition, TerDef);


                
        }
    }

    [TestFixture]
    public class ConnectionTests
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
    public class LogicTests
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