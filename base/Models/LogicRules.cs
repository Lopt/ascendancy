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
        /// The ranged in melee minus point.
        /// </summary>
        /// <returns>The in melee minus point.</returns>
        /// <param name="entity">Current Entity.</param>
        public static int RangedInMeeleMalus(Entity entity)
        {
            entity.ModfiedAttackValue -= Constants.RANGE_IN_MEELE_MALUS;
            return 0;
        }

        /// <summary>
        /// All the attack modifier.
        /// </summary>
        /// <returns>The attack modifier.</returns>
        /// <param name="entity">Current Entity.</param>
        public static IList AllAttackModifier(Entity entity)
        {
            List<int> allMod = new List<int>();
            // Dice should be the last method in the list !
            allMod.Add(TerrainAttackModifier(entity));
            allMod.Add(Dice(entity));
            return allMod;
        }

        /// <summary>
        /// All the attack modifier.
        /// </summary>
        /// <returns>The attack modifier.</returns>
        /// <param name="entity">Current Entity.</param>
        public static IList AllAttackModifierRangedInMeele(Entity entity)
        {
            List<int> allMod = new List<int>();
            // Dice should be the last method in the list !
            allMod.Add(RangedInMeeleMalus(entity));
            allMod.Add(TerrainAttackModifier(entity));
            allMod.Add(Dice(entity));
            return allMod;
        }

        /// <summary>
        /// All the defense modifier.
        /// </summary>
        /// <returns>The defense modifier.</returns>
        /// <param name="entity">Current Entity.</param>
       public static IList AllDefenseModifier(Entity entity)
       {
            List<int> allMod = new List<int>();
            allMod.Add(TerrainDefenseModifier(entity));
            return allMod;
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
                new RegionPosition(0,  1),
                new RegionPosition(1,  1),
                new RegionPosition(1,  0),
                new RegionPosition(1, -1),
                new RegionPosition(0, -1)
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
        /// Gets the surrounded territory around a given Position.
        /// </summary>
        /// <returns>The surrounded positions.</returns>
        /// <param name="entity">Current Entity.</param>
        /// <param name="range">Range around the position.</param>
        public static HashSet<PositionI> GetSurroundedPositions(PositionI entity, int range)
        {
            var fieldSet = new HashSet<PositionI>();
            var fieldSetHelper = new HashSet<PositionI>();

            fieldSet.Add(entity);
            fieldSetHelper.Add(entity);

            for (int index = 0; index != range; ++index)
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
        /// <returns>0 instead it change the value via pointer.</returns>
        /// <param name="entity">Current Entity.</param>
        public static int Dice(Entity entity)
        {
            var rand = new Random(entity.ID + entity.OwnerID + entity.Position.GetHashCode());
            double result = entity.ModfiedAttackValue;
            var randomValue = rand.NextDouble() * 2;    
            result *= randomValue;
            entity.ModfiedAttackValue = (int)result;
            return 0;
        }

        /// <summary>
        /// Terrains the defense modifier.
        /// </summary>
        /// <returns>0 instead it change the value via pointer.</returns>
        /// <param name="entity">Current entity.</param>
        public static int TerrainDefenseModifier(Entity entity)
        {
            entity.ModfiedAttackValue = ((Definitions.UnitDefinition)entity.Definition).Defense * 
                                            World.Instance.RegionManager.GetRegion(entity.Position.RegionPosition).GetTerrain(entity.Position.CellPosition).DefenseModifier;
            return 0;
        }

        /// <summary>
        /// Terrains the attack modifier.
        /// </summary>
        /// <returns>0 instead it change the value via pointer.</returns>
        /// <returns>The attack modifier.</returns>
        /// <param name="entity">Current Entity.</param>
        public static int TerrainAttackModifier(Entity entity)
        {
            entity.ModfiedAttackValue = ((Definitions.UnitDefinition)entity.Definition).Attack * 
                                           World.Instance.RegionManager.GetRegion(entity.Position.RegionPosition).GetTerrain(entity.Position.CellPosition).AttackModifier;
            return 0;
        }

        /// <summary>
        /// Enables the headquarter build options headquarter = 276.
        /// </summary>
        /// <returns>The headquarter build options.</returns>
        public static List<long> EnableHeadquarterBuildOptions()
        {
            var list = new List<long>();
            list.Add((long)Models.Definitions.EntityType.Barracks);
            list.Add((long)Models.Definitions.EntityType.Factory);
            list.Add((long)Models.Definitions.EntityType.Attachment);
            list.Add((long)Models.Definitions.EntityType.GuardTower);
            list.Add((long)Models.Definitions.EntityType.Hospital);
            list.Add((long)Models.Definitions.EntityType.TradingPost);
            return list;
        }

        /// <summary>
        /// Enables the barracks build options barracks = 282.
        /// </summary>
        /// <returns>The barracks build options.</returns>
        public static List<long> EnableBarracksBuildOptions()
        {
            var list = new List<long>();           
            list.Add((long)Models.Definitions.EntityType.Mage);
            list.Add((long)Models.Definitions.EntityType.Warrior);
            list.Add((long)Models.Definitions.EntityType.Archer);
            list.Add((long)Models.Definitions.EntityType.Scout);
            list.Add((long)Models.Definitions.EntityType.Unknown3);
            list.Add((long)Models.Definitions.EntityType.Hero);
            return list;
        }

        /// <summary>
        /// Enables the build options.
        /// </summary>
        /// <param name="entityType">Entity type.</param>
        /// <param name="account">Current Account.</param>
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
        /// <param name="account">Current Account.</param>
        public static void IncreaseHoleStorage(Account account)
        {            
            account.Scrap.MaximumValue += Constants.HEADQUARTER_STORAGE_VALUE;
            account.Population.MaximumValue += Constants.HEADQUARTER_STORAGE_VALUE;
            account.Technology.MaximumValue += Constants.TECHNOLOGY_MAX_VALUE;
            account.Energy.MaximumValue += Constants.HEADQUARTER_STORAGE_VALUE;
            account.Plutonium.MaximumValue += Constants.HEADQUARTER_STORAGE_VALUE;
        }

        /// <summary>
        /// Decreases the hole storage.
        /// </summary>
        /// <param name="account">Current Account.</param>
        public static void DecreaseHoleStorage(Account account)
        {
            account.Scrap.MaximumValue -= Constants.HEADQUARTER_STORAGE_VALUE;
            account.Population.MaximumValue -= Constants.HEADQUARTER_STORAGE_VALUE;
            account.Technology.MaximumValue -= Constants.TECHNOLOGY_MAX_VALUE;
            account.Energy.MaximumValue -= Constants.HEADQUARTER_STORAGE_VALUE;
            account.Plutonium.MaximumValue -= Constants.HEADQUARTER_STORAGE_VALUE;
        }

        /// <summary>
        /// Increases the population.
        /// </summary>
        /// <param name="account">Current Account.</param>
        /// <param name="entity">Current Entity.</param>
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
        /// <param name="account">Current Account.</param>
        /// <param name="entity">Current Entity.</param>
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
        /// <param name="account">Current Account.</param>
        /// <param name="entity">Current Entity.</param>
        public static void IncreaseStorage(Account account, Entity entity)
        {
            IncreasePopulation(account, entity);
            IncreaseScrap(account, entity);
        }

        /// <summary>
        /// Gathers the resources.
        /// </summary>
        /// <param name="account">Current Account.</param>
        /// <param name="regionManagerC">Region manager c.</param>
        public static void GatherResources(Account account, Controllers.RegionManagerController regionManagerC)
        {
            if (account.TerritoryBuildings.ContainsKey((long)Core.Models.Definitions.EntityType.Headquarter))
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
