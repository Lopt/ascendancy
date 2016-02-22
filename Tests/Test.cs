namespace Tests
{
    using System;
    using System.Net.Sockets;
    using Core.Connection;
    using Core.Models;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using TCPServer;

    /// <summary>
    /// Position tests.
    /// </summary>
    [TestFixture]
    public class PositionTests
    {
        /// <summary>
        /// Tests the Latitude and Longitude class.
        /// </summary>
        [Test]
        public void LatLonTest()
        {
            var latLon = new LatLon(50.97695325, 11.02396488);
            Assert.IsNotNull(latLon);

            // Tests to get a Latitude and Longitude out of an GamePosition
            var pos = new Position(latLon);
            var latLon2 = new LatLon(pos);
            Assert.IsNotNull(latLon2);

            // Tests if the Latitude and Longitude are the same till the 4. digit after conversion
            var lat = latLon.Lat * 10000;
            var lon = latLon.Lon * 10000;

            var lat2 = latLon2.Lat * 10000;
            var lon2 = latLon2.Lon * 10000;

            Assert.AreEqual((int)lat, (int)lat2);
            Assert.AreEqual((int)lon, (int)lon2);
        }

        /// <summary>
        /// Tests the CellPosition class.
        /// </summary>
        [Test]
        public void CellPositionTest()
        {
            // standart constructor
            var cellPos = new CellPosition(0, 0);
            Assert.IsNotNull(cellPos);

            // Constructor CellPosition out of Position
            var position = new Position(0, 0);
            cellPos = new CellPosition(position);
            Assert.IsNotNull(cellPos);

            // Constructor CellPosition out of PositionI
            var positionI = new PositionI(0, 0);
            cellPos = new CellPosition(positionI);
            Assert.IsNotNull(cellPos);

            // Tests the == operator
            var cellPos2 = new CellPosition(0, 0);
            Assert.True(cellPos == cellPos2);

            // Tests the Equas function
            Assert.True(cellPos.Equals(cellPos2));

            // Tests the != operator
            cellPos2 = new CellPosition(1, 1);
            Assert.True(cellPos != cellPos2);

            // Tests if its Posible to create a cell position bigger than 31 and smaller then 0
            cellPos = new CellPosition(32, 32);
            Assert.AreNotEqual(32, cellPos.CellX);
            Assert.AreNotEqual(32, cellPos.CellY);

            cellPos = new CellPosition(-1, -1);
            Assert.AreNotEqual(-1, cellPos.CellX);
            Assert.AreNotEqual(-1, cellPos.CellY);
        }

        /// <summary>
        /// Tests the RegionPosition class.
        /// </summary>
        [Test]
        public void RegionPositionTest()
        {
            // standart constructor
            var regionPos = new RegionPosition(0, 0);
            Assert.IsNotNull(regionPos);

            // Constructor RegionPosition out of a Position
            var position = new Position(0, 0);
            regionPos = new RegionPosition(Position);
            Assert.IsNotNull(regionPos);

            // Constructor RegionPosition out of a PositionI
            var positionI = new PositionI(0, 0);
            regionPos = new RegionPosition(positionI);
            Assert.IsNotNull(regionPos);

            // Tests the + Operator
            var regionPos2 = new RegionPosition(1, 1);
            regionPos += regionPos2;
            Assert.AreEqual(regionPos2, regionPos);

            // Tests the == Operator
            Assert.True(regionPos == regionPos2);

            // Tests the Equals funktion
            Assert.True(regionPos.Equals(regionPos2));
        }

        /// <summary>
        /// Tests the Position class.
        /// </summary>
        [Test]
        public void PositionTest()
        {
            // Standart Constructor
            var position = new Position(0, 0);
            Assert.IsNotNull(position);

            // Constructor Position out of PositionI
            position = new Position(new PositionI(0, 0));
            Assert.IsNotNull(position);

            // Constructor Position out of Latitude and Longitude
            position = new Position(new LatLon(50.97695325, 11.02396488));
            Assert.IsNotNull(position);

            // Constructor Position out of a ReagionPosition
            var regionPos = new RegionPosition(0, 0);
            position = new Position(regionPos);
            Assert.IsNotNull(position);

            // Constructor Position out of a RegionPosition and a CellPosition
            var cellPos = new CellPosition(0, 0);
            position = new Position(regionPos, cellPos);
            Assert.IsNotNull(position);

            // Constructor Position out of a RegionPositionX, a RegionPositionY, a CellPositionX and a CellPositionY
            position = new Position(regionPos.RegionX, regionPos.RegionY, cellPos.CellX, cellPos.CellY);
            Assert.IsNotNull(position);

            // tests the + Operator
            position = new Position(new LatLon(50.97695325, 11.02396488));
            var position2 = new Position(0, 0);

            position2 += position;

            Assert.AreEqual(position, position2);

            // tests the - Operator
            position -= position2;

            Assert.AreNotEqual(position2, position);
            Assert.AreEqual(new Position(0, 0), position);

            // tests the == Operator
            Assert.True(new Position(0, 0) == position);

            // tests the Equals function
            Assert.True(position.Equals(new Position(0, 0)));

            // test Distance with Position
            var dist = position.Distance(position2);
            Assert.IsNotNull(dist);

            // test Distance with PositionI
            var dist2 = position2.Distance(new PositionI(0, 0));
            Assert.IsNotNull(dist2);
            Assert.AreEqual(dist, dist2);
        }

        /// <summary>
        /// Tests the PositionI class.
        /// </summary>
        [Test]
        public void PositionITest()
        {
            // standart Constructor
            var position = new PositionI(0, 0);
            Assert.IsNotNull(position);

            // Constructor Position out of a ReagionPosition
            var regionPos = new RegionPosition(0, 0);
            var cellPos = new CellPosition(0, 0);
            position = new PositionI(regionPos, cellPos);
            Assert.IsNotNull(position);

            position = new PositionI(new Position(0, 0));
            Assert.IsNotNull(position);

            // tests the + Operator
            position = new PositionI(1, 1);
            var position2 = new PositionI(0, 0);
            position2 += position;

            Assert.AreEqual(position, position2);

            // tests the - Operator
            position -= position2;

            Assert.AreNotEqual(position2, position);
            Assert.AreEqual(new PositionI(0, 0), position);

            // tests the == Operator
            Assert.True(new PositionI(0, 0) == position);

            // tests the Equals function
            Assert.True(position.Equals(new PositionI(0, 0)));

            // test Distance with PositionI
            var dist = position.Distance(position2);
            Assert.IsNotNull(dist);

            // test Distance with PositionI
            var dist2 = position2.Distance(new Position(0, 0));
            Assert.IsNotNull(dist2);
            Assert.AreEqual(dist, dist2);
        }
    }

    /// <summary>
    /// Compression helper tests.
    /// </summary>
    [TestFixture]
    public class CompressionHelperTests
    {
        /// <summary>
        /// Tests the Compression and Decompression.
        /// </summary>
        [Test]
        public void CompressionDecompression()
        {
            // Test for a single Word
            string input = "Teststring";
            var inputbytes = System.Text.Encoding.UTF8.GetBytes(input);
            var bytes = Core.Helper.CompressionHelper.Compress(inputbytes);
            var output = Core.Helper.CompressionHelper.Decompress(bytes);
            Assert.AreEqual(inputbytes, output);
            Assert.AreNotEqual(bytes, inputbytes);

            // Test for an long string
            input = "this is a long text who serves as a test for the compressionHelper class";
            inputbytes = System.Text.Encoding.UTF8.GetBytes(input);
            bytes = Core.Helper.CompressionHelper.Compress(inputbytes);
            output = Core.Helper.CompressionHelper.Decompress(bytes);
            Assert.AreEqual(inputbytes, output);
            Assert.AreNotEqual(bytes, inputbytes);

            // Test vor an Very long String
            input = "this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something this is a long text who serves as a test for the compressionHelper class so i need to Write some cause its Something";
            inputbytes = System.Text.Encoding.UTF8.GetBytes(input);
            bytes = Core.Helper.CompressionHelper.Compress(inputbytes);
            output = Core.Helper.CompressionHelper.Decompress(bytes);
            Assert.AreEqual(inputbytes, output);
            Assert.AreNotEqual(bytes, inputbytes);

            // Test for a long string with additional format comands
            input = "this is a long text who serves as a test for the compressionHelper class\t\tthis is a long text who serves as a test for the compressionHelper class";
            inputbytes = System.Text.Encoding.UTF8.GetBytes(input);
            bytes = Core.Helper.CompressionHelper.Compress(inputbytes);
            output = Core.Helper.CompressionHelper.Decompress(bytes);
            Assert.AreEqual(inputbytes, output);
            Assert.AreNotEqual(bytes, inputbytes);

            // Test for an Empty string
            input = string.Empty;
            inputbytes = System.Text.Encoding.UTF8.GetBytes(input);
            bytes = Core.Helper.CompressionHelper.Compress(inputbytes);
            output = Core.Helper.CompressionHelper.Decompress(bytes);
            Assert.AreEqual(inputbytes, output);
            Assert.AreNotEqual(bytes, inputbytes);

            // Test for own Byte data
            byte[] bytedata =
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
            bytes = Core.Helper.CompressionHelper.Compress(bytedata);
            output = Core.Helper.CompressionHelper.Decompress(bytes);
            Assert.AreEqual(bytedata, output);
            Assert.AreNotEqual(bytes, inputbytes);
        }
    }

    /// <summary>
    /// Load helper tests.
    /// </summary>
    [TestFixture]
    public class LoadHelperTests
    {
        /// <summary>
        /// Tests the ReplacePath function.
        /// </summary>
        [Test]
        public void ReplacePath()
        {
            var regionPosition = new RegionPosition(new Position(new LatLon(50.97695325, 11.02396488)));
            var newPath = Core.Helper.LoadHelper.ReplacePath(Server.Models.ServerConstants.REGION_FILE, regionPosition);
            Assert.IsNotNull(newPath);
        }

        /// <summary>
        /// Tests the JsonToTerrain function.
        /// </summary>
        [Test]
        public void JsonToTerrain()
        {
            var regionPosition = new RegionPosition(new Position(new LatLon(50.97695325, 11.02396488)));
            var newPath = Core.Helper.LoadHelper.ReplacePath(Server.Models.ServerConstants.REGION_FILE, regionPosition);
            // var json = await Core.RequestAsync(newpath);
        }

        /// <summary>
        /// Tests the JsonToRegion function.
        /// </summary>
        [Test]
        public void JsonToRegion()
        {    
        }
    }

    /// <summary>
    /// Definitions tests.
    /// </summary>
    [TestFixture]
    public class DefinitionsTests
    {
        /// <summary>
        /// Tests the TerrainDefinitions class.
        /// </summary>
        [Test]
        public void TerrainDefinitions()
        {
            int[] res = {0, 0, 0, 0, 0};
            var def = new Core.Models.Definitions.TerrainDefinition(Core.Models.Definitions.EntityType.Grassland, res, true, true, 4, 5, 6);
            Assert.IsNotNull(def);

            Assert.IsInstanceOf<Core.Models.Definitions.TerrainDefinition>(def);

            Assert.AreEqual(Core.Models.Definitions.Category.Terrain, def.Category);
            Assert.AreEqual(Core.Models.Definitions.EntityType.Grassland, def.SubType);
            Assert.AreEqual(3, def.ID);

            Assert.AreEqual(5, def.Resources.Length);
            Assert.AreEqual(true, def.Buildable);
            Assert.AreEqual(true, def.Walkable);
            Assert.AreEqual(4, def.TravelCost);
            Assert.AreEqual(5, def.DefenseModifier);
            Assert.AreEqual(6, def.AttackModifier);

            // Test if a Wrong entity could be a TerrainDefinition
            def = new Core.Models.Definitions.TerrainDefinition(Core.Models.Definitions.EntityType.Archer, res, true, true, 4, 5, 6);
            Assert.IsNotNull(def);

            Assert.IsInstanceOf<Core.Models.Definitions.TerrainDefinition>(def);

            Assert.AreNotEqual(Core.Models.Definitions.Category.Terrain, def.Category);
            // Assert.AreEqual(Core.Models.Definitions.EntityType.Grassland, Def.SubType);
            // Assert.AreEqual(78, Def.ID);
        }

        /// <summary>
        /// Tests the UnitDefinitions class.
        /// </summary>
        [Test]
        public void UnitDefinitions()
        {
            string[] action = { };
            var def = new Core.Models.Definitions.UnitDefinition(Core.Models.Definitions.EntityType.Archer, action, 1, 1, 100, 10, 2, 2, 100, 50, 0, 0);
            Assert.IsNotNull(def);

            Assert.IsInstanceOf<Core.Models.Definitions.UnitDefinition>(def);

            Assert.AreEqual(Core.Models.Definitions.Category.Unit, def.Category);
            Assert.AreEqual(Core.Models.Definitions.EntityType.Archer, def.SubType);
            Assert.AreEqual(78, def.ID);

            Assert.AreEqual(1, def.Attack);
            Assert.AreEqual(1, def.Defense);
            Assert.AreEqual(100, def.Health);
            Assert.AreEqual(10, def.Moves);
            Assert.AreEqual(2, def.AttackRange);
            Assert.AreEqual(2, def.Population);
            Assert.AreEqual(100, def.Scrapecost);
            Assert.AreEqual(50, def.Energycost);
            Assert.AreEqual(0, def.Plutoniumcost);
            Assert.AreEqual(0, def.Techcost);

            // Test if a Terrain Entity could be a Unit Definition
            def = new Core.Models.Definitions.UnitDefinition(Core.Models.Definitions.EntityType.Grassland, action, 1, 1, 100, 10, 2, 2, 100, 50, 0, 0);
            Assert.IsNotNull(def);

            Assert.IsInstanceOf<Core.Models.Definitions.UnitDefinition>(def);
            Assert.AreNotEqual(Core.Models.Definitions.Category.Unit, def.Category);
      }
    }

    /// <summary>
    /// Map.Region tests.
    /// </summary>
    [TestFixture]
    public class MapRegionTests
    {
        /// <summary>
        /// Tests the Region class.
        /// </summary>
        [Test]
        public void MapRegion()
        {
            // Test with a RegionPosition as Input
            var regionPosition = new RegionPosition(new Position(new LatLon(50.97695325, 11.02396488)));
            var region = new Core.Models.Region(regionPosition);

            Assert.IsNotNull(region);
            Assert.IsInstanceOf<Core.Models.Region>(region);

            // Test with a Region as Input
            var region2 = new Region(region);

            Assert.IsNotNull(region2);
            Assert.IsInstanceOf<Core.Models.Region>(region2);
            Assert.AreEqual(region.RegionPosition, region2.RegionPosition);

            // Test with a RegionPosition and an TerrainDefinition as Input
            int[] res = {0, 0, 0, 0, 0};
            var TerDef = new Core.Models.Definitions.TerrainDefinition(Core.Models.Definitions.EntityType.Grassland, res, true, true, 4, 5, 6);
            // var region3 = new Region(regionPosition, TerDef);
      }
    }

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

    /// <summary>
    /// Logic tests.
    /// </summary>
    [TestFixture]
    public class LogicTests
    {
        /// <summary>
        /// The surround tiles on even x positions.
        /// From North to NorthEast in clockwise
        /// </summary>
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
    
        /// <summary>
        /// Gets the surrounded fields.
        /// </summary>
        [Test]
        public void GetSurroundedFields()
        {
            // Test for Even x Coordinate
            var startPosition = new Core.Models.PositionI(0, 0);
            var positions = Core.Models.LogicRules.GetSurroundedFields(startPosition);

            var expectedPos = new Core.Models.PositionI[]
            {
                startPosition + SurroundTilesOdd[0],
                startPosition + SurroundTilesOdd[1],
                startPosition + SurroundTilesOdd[2],
                startPosition + SurroundTilesOdd[3],
                startPosition + SurroundTilesOdd[4],
                startPosition + SurroundTilesOdd[5]
            };

            Assert.IsNotEmpty(positions);
            Assert.AreEqual(6, positions.Length);
            Assert.AreEqual(positions, expectedPos);

            // Test For uneven x Coordinate
            startPosition = new Core.Models.PositionI(1, 0);
            positions = Core.Models.LogicRules.GetSurroundedFields(startPosition);

            expectedPos = new Core.Models.PositionI[]
                {
                    startPosition + SurroundTilesEven[0],
                    startPosition + SurroundTilesEven[1],
                    startPosition + SurroundTilesEven[2],
                    startPosition + SurroundTilesEven[3],
                    startPosition + SurroundTilesEven[4],
                    startPosition + SurroundTilesEven[5]
                };
            
            Assert.AreEqual(positions, expectedPos);
        }

        /// <summary>
        /// Tests the Surrounded Positions function.
        /// </summary>
        [Test]
        public void GetSurroundedPositions()
        {
            var startPosition = new Core.Models.PositionI(0, 0);
            int range = 1;
            var positions = Core.Models.LogicRules.GetSurroundedPositions(startPosition, range);

            Assert.AreEqual(positions.Count, 7);

            range = 2;
            positions = Core.Models.LogicRules.GetSurroundedPositions(startPosition, range);

            Assert.AreEqual(positions.Count, 19);

            range = 3;
            positions = Core.Models.LogicRules.GetSurroundedPositions(startPosition, range);

            Assert.AreEqual(positions.Count, 37);

            range = 4;
            positions = Core.Models.LogicRules.GetSurroundedPositions(startPosition, range);

            Assert.AreEqual(positions.Count, 61);

            range = 5;
            positions = Core.Models.LogicRules.GetSurroundedPositions(startPosition, range);

            Assert.AreEqual(positions.Count, 91);
        }

        [Test]
        public void Storage()
        {
        }
    }
}