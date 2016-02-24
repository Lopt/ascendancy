namespace Tests
{
    using System.Net.Sockets;
    using Core.Connection;
    using Core.Models;
    using Newtonsoft.Json;
    using NUnit.Framework;

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

        /// <summary>
        /// Tests the enabling and disabling of build options.
        /// </summary>
        [Test]
        public void BuildOptions()
        {
            var acc = GetTestAcc();

            Assert.IsNotNull(acc.BuildableBuildings);
            var buildOpt = acc.BuildableBuildings;

            LogicRules.EnableBuildOptions((long)Core.Models.Definitions.EntityType.Headquarter, acc);

            Assert.AreNotEqual(buildOpt, acc.BuildableBuildings);
            Assert.Contains((long)Core.Models.Definitions.EntityType.Headquarter, (System.Collections.ICollection)acc.BuildableBuildings);

            LogicRules.DisableBuildOptions((long)Core.Models.Definitions.EntityType.Headquarter, acc);

            Assert.AreEqual(buildOpt, acc.BuildableBuildings);
        }

        /// <summary>
        /// Tests the functions to increase and decrease the storage of an Account.
        /// </summary>
        [Test]
        public void StorageTest()
        {
            var acc = GetTestAcc();

            var oldEnergy = acc.Energy.MaximumValue;
            var oldPopulation = acc.Population.MaximumValue;
            var oldPlutonium = acc.Population.MaximumValue;
            var oldScrap = acc.Scrap.MaximumValue;
            var oldTechnology = acc.Technology.MaximumValue;

            LogicRules.IncreaseWholeStorage(acc);

            Assert.AreNotEqual(oldEnergy, acc.Energy.MaximumValue);
            Assert.AreNotEqual(oldPopulation, acc.Population.MaximumValue);
            Assert.AreNotEqual(oldPlutonium, acc.Plutonium.MaximumValue);
            Assert.AreNotEqual(oldScrap, acc.Scrap.MaximumValue);
            Assert.AreNotEqual(oldTechnology, acc.Technology.MaximumValue);

            LogicRules.DecreaseWholeStorage(acc);

            Assert.AreEqual(oldEnergy, acc.Energy.MaximumValue);
            Assert.AreEqual(oldPopulation, acc.Population.MaximumValue);
            Assert.AreEqual(oldPlutonium, acc.Plutonium.MaximumValue);
            Assert.AreEqual(oldScrap, acc.Scrap.MaximumValue);
            Assert.AreEqual(oldTechnology, acc.Technology.MaximumValue);

            // Increase and Decrease Scrap Strorage
            var entity = new Entity(336, Core.Models.Definitions.EntityType.Scrapyard, acc, new PositionI(new Position(new LatLon(50.97695325, 11.02396488))), 100, 0);

            LogicRules.IncreaseStorage(acc, entity);

            Assert.AreNotEqual(oldScrap, acc.Scrap.MaximumValue);

            LogicRules.DecreasStorage(acc, entity);

            Assert.AreEqual(oldScrap, acc.Scrap.MaximumValue);

            // Increase and Decrease Population Strorage
            entity = new Entity(342, Core.Models.Definitions.EntityType.Tent, acc, new PositionI(new Position(new LatLon(50.97695325, 11.02396488))), 100, 0);

            LogicRules.IncreaseStorage(acc, entity);

            Assert.AreNotEqual(oldPopulation, acc.Population.MaximumValue);

            LogicRules.DecreasStorage(acc, entity);

            Assert.AreEqual(oldPopulation, acc.Population.MaximumValue);

            // Increase and Decrease Energy Strorage
            entity = new Entity(330, Core.Models.Definitions.EntityType.Transformer, acc, new PositionI(new Position(new LatLon(50.97695325, 11.02396488))), 100, 0);

            LogicRules.IncreaseStorage(acc, entity);

            Assert.AreNotEqual(oldEnergy, acc.Energy.MaximumValue);

            LogicRules.DecreasStorage(acc, entity);

            Assert.AreEqual(oldEnergy, acc.Energy.MaximumValue);
        }

        /// <summary>
        /// Gets account for testing the Logic.
        /// </summary>
        /// <returns> a Test account.</returns>
        private Account GetTestAcc()
        {
            var testLoginRequest = new Core.Connections.LoginRequest(
                new Core.Models.Position(new LatLon(50.97695325, 11.02396488)),
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

            var testStream = client.GetStream();

            testPackage.Send(testStream);

            var testPackageIn = Packet.Receive(testStream);

            client.Close();

            var data = JsonConvert.DeserializeObject<Core.Connections.LoginResponse>(testPackageIn.Content);

            return new Account(data.AccountId);
        }
    }
}