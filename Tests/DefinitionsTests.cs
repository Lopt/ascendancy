namespace Tests
{
    using System;
    using NUnit.Framework;
    using Core;

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
            int[] res = { 0, 0, 0, 0, 0 };
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
}