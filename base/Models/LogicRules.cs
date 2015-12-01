namespace Core.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Core.Models;

    /// <summary>
    /// Logic rules.
    /// </summary>
    public class LogicRules
    {
        /// <summary>
        /// The ranged in meele malus.
        /// </summary>
        public static int RangedInMeeleMalus(Entity entity)
        {
            entity.ModfifedAttackValue -= Constants.RANGE_IN_MEELE_MALUS;
            return 0;
        }

        /// <summary>
        /// Alls the attack modifier.
        /// </summary>
        /// <returns>The attack modifier.</returns>
        /// <param name="entity">Entity.</param>
        public static IList AllAttackModifier(Entity entity)
        {
            List<int> AllMod = new List<int>();
            // Dice should be the last method in the list !
            AllMod.Add(TerrainAttackModifier(entity));
            AllMod.Add(Dice(entity));
            return AllMod;
        }

        /// <summary>
        /// Alls the attack modifier.
        /// </summary>
        /// <returns>The attack modifier.</returns>
        /// <param name="entity">Entity.</param>
        public static IList AllAttackModifierRangedInMeele(Entity entity)
        {
            List<int> AllMod = new List<int>();
            // Dice should be the last method in the list !
            AllMod.Add(RangedInMeeleMalus(entity));
            AllMod.Add(TerrainAttackModifier(entity));
            AllMod.Add(Dice(entity));
            return AllMod;
        }

        /// <summary>
        /// Alls the defense modifier.
        /// </summary>
        /// <returns>The defense modifier.</returns>
        /// <param name="entity">Entity.</param>
       public static IList AllDefenseModifier(Entity entity)
       {
            List<int> AllMod = new List<int>();
            AllMod.Add(TerrainDefenseModifier(entity));
            return AllMod;
       }

        /// <summary>
        /// The surround tiles on even x positions.
        /// From North to NorthEast in clockwise
        /// </summary>
        public static readonly PositionI[] SurroundTilesEven =
        {
            new PositionI(0, -1),
            new PositionI(1, 0),
            new PositionI(1, 1),
            new PositionI(0, 1),
            new PositionI(-1, 1),
            new PositionI(-1, 0)
        };

        /// <summary>
        /// The surround tiles on odd x positions.
        /// From North to NorthEast in clockwise
        /// </summary>
        public static readonly PositionI[] SurroundTilesOdd =
        {
            new PositionI(0, -1),
            new PositionI(1, -1),
            new PositionI(1, 0),
            new PositionI(0, 1),
            new PositionI(-1, 0),
            new PositionI(-1, -1)
        };

        /// <summary>
        /// Surrounded Regions from top left clockwise
        /// </summary>
        public static readonly RegionPosition[] SurroundRegions =
        {
                new RegionPosition(-1, -1),
                new RegionPosition(-1,  0),
                new RegionPosition(-1,  1),
                new RegionPosition( 0,  1),
                new RegionPosition( 1,  1),
                new RegionPosition( 1,  0),
                new RegionPosition( 1, -1),
                new RegionPosition( 0, -1)
        };

        /// <summary>
        /// Gets the surrounded fields.
        /// </summary>
        /// <returns>The surrounded fields.</returns>
        /// <param name="pos">Center Position.</param>
        public static PositionI[] GetSurroundedFields(PositionI pos)
        {
            var surroundedFields = SurroundTilesEven;
            if (pos.X % 2 == 0)
            {
                surroundedFields = SurroundTilesOdd;
            }

            var surrounded = new PositionI[6];
            for (var index = 0; index < surroundedFields.Length; ++index)
            {
                surrounded[index] = pos + surroundedFields[index];
            }
            return surrounded;
        }
            
        /// <summary>
        /// Gets the surrounded territory arround a given Position.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="range">The range of an entity.</param>
        public static HashSet<PositionI> GetSurroundedPositions(PositionI entity, int range)
        {
            var fieldSet = new HashSet<PositionI>();
            var fieldSetHelper = new HashSet<PositionI>();

            fieldSet.Add(entity);
            fieldSetHelper.Add(entity);

            for (int i = 0; i != range; ++i)
            {
                var tempfieldSet = new HashSet<PositionI>();
                foreach (var item in fieldSetHelper)
                {
                    var surroundedTiles = GetSurroundedFields(item);
                    foreach (var tile in surroundedTiles)
                    {
                        if (!fieldSet.Contains(tile))
                        {
                            fieldSet.Add(tile);
                            tempfieldSet.Add(tile);
                        }
                    }
                }
                fieldSetHelper = tempfieldSet;
            }
            return fieldSet;
        }

        /// <summary>
        /// Dice the specified attack damage for the entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public static int Dice(Entity entity)
        {
            var rand = new Random(entity.ID + entity.OwnerID + entity.Position.GetHashCode());
            double result = entity.ModfifedAttackValue;
            var randomValue = rand.NextDouble() * (2);    
            result *= randomValue;
            entity.ModfifedAttackValue = (int)result;
            return 0;
        }

        /// <summary>
        /// Gets the defense modifier.
        /// </summary>
        /// <value>The defense modifier.</value>
        public static int  TerrainDefenseModifier(Entity entitiy)
        {
            entitiy.ModfifedAttackValue = ((Definitions.UnitDefinition)entitiy.Definition).Defense * 
                                            World.Instance.RegionManager.GetRegion(entitiy.Position.RegionPosition).GetTerrain(entitiy.Position.CellPosition).DefenseModifier;
            return 0;
        }

        /// <summary>
        /// Attacks the modifier.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public static int TerrainAttackModifier(Entity entity)
        {
            entity.ModfifedAttackValue = ((Definitions.UnitDefinition)entity.Definition).Attack * 
                                           World.Instance.RegionManager.GetRegion(entity.Position.RegionPosition).GetTerrain(entity.Position.CellPosition).AttackModifier;
            return 0;
        }

        /// <summary>
        /// Enables the headquarter build options headquarter = 276.
        /// </summary>
        /// <returns>The headquarter build options.</returns>
        public static List<long> EnableHeadquarterBuildOptions()
        {
            var List = new List<long>();
            List.Add((long)Models.Definitions.EntityType.Barracks);
            List.Add((long)Models.Definitions.EntityType.Factory);
            List.Add((long)Models.Definitions.EntityType.Attachment);
            List.Add((long)Models.Definitions.EntityType.GuardTower);
            List.Add((long)Models.Definitions.EntityType.Hospital);
            List.Add((long)Models.Definitions.EntityType.TradingPost);
            return List;
        }

        /// <summary>
        /// Enables the barracks build options barracks = 282.
        /// </summary>
        /// <returns>The barracks build options.</returns>
        public static List<long> EnableBarracksBuildOptions()
        {
            var List = new List<long>();           
            List.Add((long)Models.Definitions.EntityType.Hero);
            List.Add((long)Models.Definitions.EntityType.Mage);
            List.Add((long)Models.Definitions.EntityType.Warrior);
            List.Add((long)Models.Definitions.EntityType.Archer);
            List.Add((long)Models.Definitions.EntityType.Scout);
            List.Add((long)Models.Definitions.EntityType.Unknown3);
            return List;
        }

        /// <summary>
        /// Enables the build options.
        /// </summary>
        /// <param name="containedList">Contained list.</param>
        /// <param name="entityType">Entity type.</param>
        /// <param name="account">Account.</param>
        public static void EnableBuildOptions(long entityType, Account account)
        {
            if (!account.BuildableBuildings.ContainsKey(entityType))
            {
               if (entityType == (long)Models.Definitions.EntityType.Headquarter)
               {
                    account.BuildableBuildings.Add((long)Models.Definitions.EntityType.Headquarter, EnableHeadquarterBuildOptions());
               }
               else if (entityType == (long)Models.Definitions.EntityType.Barracks)
               {
                    account.BuildableBuildings.Add((long)Models.Definitions.EntityType.Barracks, EnableBarracksBuildOptions());
               }
            }
        }

        /// <summary>
        /// Increases the hole storage.
        /// </summary>
        /// <param name="account">Account.</param>
        public static void IncreaseHoleStorage(Account account)
        {            
            account.Scrap.MaximumValue += Constants.HEADQUARTER_STORAGE_VALUE;
            account.Population.MaximumValue += Constants.HEADQUARTER_STORAGE_VALUE;
            account.Technology.MaximumValue += Constants.HEADQUARTER_STORAGE_VALUE;
            account.Energy.MaximumValue += Constants.HEADQUARTER_STORAGE_VALUE;
            account.Plutonium.MaximumValue += Constants.HEADQUARTER_STORAGE_VALUE;
        }

        /// <summary>
        /// Increases the population.
        /// </summary>
        /// <param name="account">Account.</param>
        /// <param name="entity">Entity.</param>
        public static void IncreasePopulation(Account account, Entity entity)
        {
            if (entity.DefinitionID == (long)Core.Models.Definitions.EntityType.Tent)
            {
                account.Population.MaximumValue += Constants.POPULATION_STORAGE_VALUE;
            }
        }

        /// <summary>
        /// Increases the scrap.
        /// </summary>
        /// <param name="account">Account.</param>
        /// <param name="entity">Entity.</param>
        public static void IncreaseScrap(Account account, Entity entity)
        {
            if (entity.DefinitionID == (long)Core.Models.Definitions.EntityType.Scrapyard)
            {
                account.Scrap.MaximumValue += Constants.SCRAP_STORAGE_VALUE;
            }
        }

        /// <summary>
        /// Increases the storage.
        /// </summary>
        /// <param name="account">Account.</param>
        /// <param name="entity">Entity.</param>
        public static void IncreaseStorage(Account account, Entity entity)
        {
            IncreasePopulation(account, entity);
            IncreaseScrap(account, entity);
        }

        /// <summary>
        /// Gathers the resources.
        /// </summary>
        /// <param name="account">Account.</param>
        /// <param name="entity">Entity.</param>
        /// <param name="regionManagerC">Region manager c.</param>
        public static void GatherResources(Account account, Controllers.RegionManagerController regionManagerC)
        {
            if (account.TerritoryBuildings.ContainsKey((long)Core.Models.Definitions.EntityType.Headquarter))//if (account.Headquarters.Count >= 1)
            {                 
                    foreach (var element in account.TerritoryBuildings)
                {
                    var list = LogicRules.GetSurroundedPositions(element.Value, Constants.HEADQUARTER_TERRITORY_RANGE);
                    int scrapAmount = 0;
                    int plutoniumAmount = 0;

                    foreach (var item in list)
                    {
                        // TODO: add ressources in Terrain
                        var resources = regionManagerC.GetRegion(item.RegionPosition).GetTerrain(item.CellPosition).Resources;
                        scrapAmount += 2; // resources[0];
                        plutoniumAmount += 1; // resources[1];
                    }
                    account.Scrap.Set(scrapAmount, account.Scrap.Increments);
                    account.Plutonium.Set(plutoniumAmount, account.Plutonium.Increments);
                }                   
            }

            foreach (var element in account.Buildings)
            {
                switch (regionManagerC.GetRegion(element.RegionPosition).GetEntity(element.CellPosition).DefinitionID)
                {
                    case (int)Core.Models.Definitions.EntityType.Lab:                         
                        account.Technology.Set(1, 1);                   
                        break;

                    case (int)Core.Models.Definitions.EntityType.Furnace:
                        account.Scrap.Set(account.Scrap.Value, 2);
                        break;

                    case (int)Core.Models.Definitions.EntityType.Transformer:

                        account.Energy.Value = Constants.ENERGY_VALUE;
                        break;
                }

            }
        }

     
    }        
}
