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
            // between max and minimum
            var randomValue = rand.NextDouble() * (2 - 0.5f) + 0.5f;    
            result *= randomValue;
            entity.ModfiedAttackValue = (int)result;
            return 0;
        }

        /// <summary>
        /// Terrain for the defense modifier.
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
        /// Terrain terrain for the attack modifier.
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
            list.Add((long)Models.Definitions.EntityType.Tent);
            list.Add((long)Models.Definitions.EntityType.Lab);
            list.Add((long)Models.Definitions.EntityType.Furnace);
            list.Add((long)Models.Definitions.EntityType.Scrapyard);
            list.Add((long)Models.Definitions.EntityType.Transformer);
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
            list.Add((long)Models.Definitions.EntityType.Fencer);
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
                switch (entityType)
                {
                    case (long)Models.Definitions.EntityType.Headquarter:
                        account.BuildableBuildings.Add(entityType, EnableHeadquarterBuildOptions());
                        break;
                    case (long)Models.Definitions.EntityType.Barracks:
                        account.BuildableBuildings.Add(entityType, EnableBarracksBuildOptions());
                        break;
                }
            }
        }

        /// <summary>
        /// Disables the build options.
        /// </summary>
        /// <param name="entityType">Entity type.</param>
        /// <param name="account">Current account.</param>
        public static void DisableBuildOptions(long entityType, Account account)
        {
            if (account.BuildableBuildings.ContainsKey(entityType))
            {
                switch (entityType)
                {
                    case (long)Models.Definitions.EntityType.Headquarter:
                        account.BuildableBuildings.Remove(entityType);
                        break;
                    case (long)Models.Definitions.EntityType.Barracks:
                        account.BuildableBuildings.Remove(entityType);
                        break;
                }
            }
        }

        /// <summary>
        /// Increases the hole storage.
        /// </summary>
        /// <param name="account">Current Account.</param>
        public static void IncreaseWholeStorage(Account account)
        {            
            account.Scrap.MaximumValue += Constants.HEADQUARTER_STORAGE_VALUE;
            account.Population.MaximumValue += Constants.POPULATION_STORAGE_VALUE;
            account.Technology.MaximumValue += Constants.TECHNOLOGY_MAX_VALUE;
            account.Energy.MaximumValue += Constants.ENERGY_MAX_VALUE;
            account.Plutonium.MaximumValue += Constants.HEADQUARTER_STORAGE_VALUE;
        }

        /// <summary>
        /// Decreases the hole storage.
        /// </summary>
        /// <param name="account">Current Account.</param>
        public static void DecreaseWholeStorage(Account account)
        {
            account.Scrap.MaximumValue -= Constants.HEADQUARTER_STORAGE_VALUE;
            account.Population.MaximumValue -= Constants.POPULATION_STORAGE_VALUE;
            account.Technology.MaximumValue -= Constants.TECHNOLOGY_MAX_VALUE;
            account.Energy.MaximumValue -= Constants.ENERGY_MAX_VALUE;
            account.Plutonium.MaximumValue -= Constants.HEADQUARTER_STORAGE_VALUE;
        }

        /// <summary>
        /// Increases the population.
        /// </summary>
        /// <param name="account">Current Account.</param>
        /// <param name="entity">Current Entity.</param>
        public static void IncreaseMaxPopulation(Account account, Entity entity)
        {
            if (entity.DefinitionID == (long)Core.Models.Definitions.EntityType.Tent)
            {
                account.Population.MaximumValue += Constants.POPULATION_STORAGE_VALUE;
                account.Population.Value += Constants.POPULATION_STORAGE_VALUE;
            }
        }

        /// <summary>
        /// Increases the max energy.
        /// </summary>
        /// <param name="account">Account.</param>
        /// <param name="entity">Entity.</param>
        public static void IncreaseMaxEnergy(Account account, Entity entity)
        {
            if (entity.DefinitionID == (long)Core.Models.Definitions.EntityType.Transformer)
            {
                account.Energy.MaximumValue += Constants.ENERGY_MAX_VALUE;
                account.Energy.Value += Constants.ENERGY_MAX_VALUE;
            }
        }

        /// <summary>
        /// Sets the current max popultion.
        /// </summary>
        /// <param name="account">Account.</param>
        public static void SetCurrentMaxPopultion(Account account)
        {
            account.Population.Value = account.Population.MaximumValue;
        }

        /// <summary>
        /// Sets the current energy.
        /// </summary>
        /// <param name="account">Account.</param>
        public static void SetCurrentMaxEnergy(Account account)
        {
            account.Energy.Value = account.Energy.MaximumValue;
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
        /// Decreases the population.
        /// </summary>
        /// <param name="account">Account.</param>
        /// <param name="entity">Entity.</param>
        public static void DecreaseMaxPopulation(Account account, Entity entity)
        {
            if (entity.DefinitionID == (long)Core.Models.Definitions.EntityType.Tent)
            {
                account.Population.MaximumValue -= Constants.POPULATION_STORAGE_VALUE;
                account.Population.Value -= Constants.POPULATION_STORAGE_VALUE;
            } 
        }

        /// <summary>
        /// Decreases the max energy.
        /// </summary>
        /// <param name="account">Account.</param>
        /// <param name="entity">Entity.</param>
        public static void DecreaseMaxEnergy(Account account, Entity entity)
        {
            if (entity.DefinitionID == (long)Core.Models.Definitions.EntityType.Transformer)
            {
                account.Energy.MaximumValue -= Constants.ENERGY_MAX_VALUE;
                account.Energy.Value -= Constants.ENERGY_MAX_VALUE;
            }
        }

        /// <summary>
        /// Decreases the population.
        /// </summary>
        /// <param name="account">Account.</param>
        /// <param name="entity">Entity.</param>
        public static void DecreaseScrap(Account account, Entity entity)
        {
            if (entity.DefinitionID == (long)Core.Models.Definitions.EntityType.Scrapyard)
            {
                account.Scrap.MaximumValue -= Constants.SCRAP_STORAGE_VALUE;
            }
        }

        /// <summary>
        /// Increases the storage.
        /// </summary>
        /// <param name="account">Current Account.</param>
        /// <param name="entity">Current Entity.</param>
        public static void IncreaseStorage(Account account, Entity entity)
        {
            IncreaseMaxPopulation(account, entity);
            IncreaseScrap(account, entity);
            IncreaseMaxEnergy(account, entity);
        }

        /// <summary>
        /// Decreases the storage.
        /// </summary>
        /// <param name="account">Account.</param>
        /// <param name="entity">Entity.</param>
        public static void DecreasStorage(Account account, Entity entity)
        {
            DecreaseMaxPopulation(account, entity);
            DecreaseScrap(account, entity);
            DecreaseMaxEnergy(account, entity);
        }

        /// <summary>
        /// Gathers the resources.
        /// </summary>
        /// <param name="account">Current Account.</param>
        /// <param name="regionManagerC">Region manager c.</param>
        public static void GatherResources(Account account, Controllers.RegionManagerController regionManagerC, int range)
        {              
            foreach (var element in account.TerritoryBuildings)
            {
                var list = LogicRules.GetSurroundedPositions(element.Key, range);
                float scrapAmount = 0;
                float plutoniumAmount = 0;

            foreach (var item in list)
                {
                    // TODO: add ressources in Terrain
                    var resources = regionManagerC.GetRegion(item.RegionPosition).GetTerrain(item.CellPosition);
                    scrapAmount += 0.5f;//resources[0];
                    plutoniumAmount += 0.3f;//resources[1];
                }
                account.Scrap.Set(account.Scrap.Value, Constants.SCRAP_INCREMENT_VALUE);
                account.Plutonium.Set(account.Plutonium.Value, Constants.PLUTONIUM_INCREMENT_VALUE);
            }  
        }

        /// <summary>
        /// Resources that can generated.
        /// </summary>
        /// <param name="account">Account.</param>
        /// <param name="regionManagerC">Region manager c.</param>
        public static void IncreaseResourceGeneration(Account account,PositionI entitypos, Controllers.RegionManagerController regionManagerC)
        {
            switch (regionManagerC.GetRegion(entitypos.RegionPosition).GetEntity(entitypos.CellPosition).DefinitionID)
          {
              case (int)Core.Models.Definitions.EntityType.Lab:                         
                    account.Technology.Set(account.Technology.Value, Constants.TECHNOLOGY_INCREMENT_VALUE);                   
                    break;
    
              case (int)Core.Models.Definitions.EntityType.Furnace:
                    account.Scrap.Set(account.Scrap.Value, Constants.SCRAP_INCREMENT_VALUE);
                    break;
          }
        }

        /// <summary>
        /// Decreases the ressource generation.
        /// </summary>
        /// <param name="account">Current account.</param>
        /// <param name="entitypos">Entity position.</param>
        /// <param name="regionManagerC">Region manager controller.</param>
        public static void DecreaseRessourceGeneration(Account account,PositionI entitypos, Controllers.RegionManagerController regionManagerC)
        {
            switch (regionManagerC.GetRegion(entitypos.RegionPosition).GetEntity(entitypos.CellPosition).DefinitionID)
            {
                case (int)Core.Models.Definitions.EntityType.Lab:                         
                    account.Technology.Set(account.Technology.Value, -Constants.TECHNOLOGY_INCREMENT_VALUE);                   
                    break;

                case (int)Core.Models.Definitions.EntityType.Furnace:
                    account.Scrap.Set(account.Scrap.Value, -Constants.SCRAP_INCREMENT_VALUE);
                    break;
            }
        }

        /// <summary>
        /// Consumes the resources for creating an entity.
        /// </summary>
        /// <returns><c>true</c>, if resource was consumed, <c>false</c> otherwise.</returns>
        /// <param name="account">Current account.</param>
        /// <param name="entityDef">Entity definiton.</param>
        public static bool ConsumeResource(Account account, Definitions.Definition entityDef)
        {
            var definition = (Definitions.UnitDefinition)entityDef;

            if (account.Scrap.Value >= definition.Scrapecost &&
                account.Plutonium.Value >= definition.Plutoniumcost &&
                account.Technology.Value >= definition.Techcost &&
                account.Population.Value >= definition.Population &&
                account.Energy.Value >= definition.Energycost)
            {
                account.Scrap.Set(account.Scrap.Value - definition.Scrapecost, 0);           
                account.Plutonium.Set(account.Plutonium.Value - definition.Plutoniumcost, 0);            
                account.Technology.Set(account.Technology.Value - definition.Techcost, 0);            
                account.Population.Value -= definition.Population;
                account.Energy.Value -= definition.Energycost;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Destroy the building.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public static void DestroyBuilding(Entity entity, Region regionPos, Action action, Controllers.RegionManagerController regionManagerC)
        {
            // TODO: in pseudo klasse kapseln und generischer lösen
            switch((long)entity.DefinitionID)
            {
                case (long)Models.Definitions.EntityType.Headquarter:
                    regionPos.FreeClaimedTerritory(LogicRules.GetSurroundedPositions(entity.Position, Constants.HEADQUARTER_TERRITORY_RANGE), entity.Owner);                                                       
                    DecreaseWholeStorage(entity.Owner);
                    DisableBuildOptions(entity.DefinitionID, entity.Owner);
                    entity.Owner.TerritoryBuildings.Remove(entity.Position);
                    // TODO: bessere lösung als alles neu claimen finden  
                    foreach (var building in entity.Owner.TerritoryBuildings)
                    {
                        var list = new HashSet<PositionI>();
                        var range = 0;
                        if (building.Value == (long)Definitions.EntityType.Headquarter)
                        {
                            range = Constants.HEADQUARTER_TERRITORY_RANGE;
                        }
                        else if (building.Value == (long)Definitions.EntityType.GuardTower)
                        {
                            range = Constants.GUARDTOWER_TERRITORY_RANGE;
                        }
                        list = GetSurroundedPositions(building.Key, range);
                        var region = building.Key.RegionPosition;
                        regionManagerC.GetRegion(region).ClaimTerritory(list, entity.Owner, region, regionManagerC.RegionManager);
                    }
                    //DestroyAllBuildingsWithoutTerritory(entity.Owner, action, regionManagerC);
                    break;
                case (long)Models.Definitions.EntityType.GuardTower:
                    regionPos.FreeClaimedTerritory(LogicRules.GetSurroundedPositions(entity.Position, Constants.GUARDTOWER_TERRITORY_RANGE), entity.Owner);                   
                    entity.Owner.TerritoryBuildings.Remove(entity.Position);
                    // TODO: bessere lösung als alles neu claimen finden  
                    foreach (var building in entity.Owner.TerritoryBuildings)
                    {
                        var list = new HashSet<PositionI>();
                        var range = 0;
                        if (building.Value == (long)Definitions.EntityType.Headquarter)
                        {
                            range = Constants.HEADQUARTER_TERRITORY_RANGE;
                        }
                        else if (building.Value == (long)Definitions.EntityType.GuardTower)
                        {
                            range = Constants.GUARDTOWER_TERRITORY_RANGE;
                        }
                        list = GetSurroundedPositions(building.Key, range);
                        var region = building.Key.RegionPosition;
                        regionManagerC.GetRegion(region).ClaimTerritory(list, entity.Owner, region, regionManagerC.RegionManager);
                    }
                    //DestroyAllBuildingsWithoutTerritory(entity.Owner, action, regionManagerC); 
                    break;
                case (long)Models.Definitions.EntityType.Barracks:
                    var count = 0;
                    foreach (var element in entity.Owner.Buildings)
                    {
                        if (entity.Owner.Buildings.ContainsValue(element.Value))
                        {
                            count++;                                
                        }    
                    }
                    if (count == 1)
                    {
                        DisableBuildOptions(entity.DefinitionID, entity.Owner);
                    }
                    entity.Owner.Buildings.Remove(entity.Position);
                    break;
                case (long)Models.Definitions.EntityType.Furnace:
                    DecreaseRessourceGeneration(entity.Owner, entity.Position, regionManagerC);
                    entity.Owner.Buildings.Remove(entity.Position);
                    break;
                case (long)Models.Definitions.EntityType.Factory:
                    entity.Owner.Buildings.Remove(entity.Position);
                    break;
                case (long)Models.Definitions.EntityType.Attachment:
                    entity.Owner.Buildings.Remove(entity.Position);
                    break;
                case (long)Models.Definitions.EntityType.Hospital:
                    entity.Owner.Buildings.Remove(entity.Position);
                    break;
                case (long)Models.Definitions.EntityType.TradingPost:
                    entity.Owner.Buildings.Remove(entity.Position);
                    break;
                case (long)Models.Definitions.EntityType.Tent:
                    DecreaseMaxPopulation(entity.Owner, entity);
                    entity.Owner.Buildings.Remove(entity.Position);
                    break;
                case (long)Models.Definitions.EntityType.Lab:
                    DecreaseRessourceGeneration(entity.Owner, entity.Position, regionManagerC);
                    entity.Owner.Buildings.Remove(entity.Position);
                    break;
                case (long)Models.Definitions.EntityType.Scrapyard:
                    DecreaseScrap(entity.Owner, entity);
                    entity.Owner.Buildings.Remove(entity.Position);
                    break;
            }
        }

        /// <summary>
        /// Destroys all buildings without territory.
        /// </summary>
        /// <param name="account">Account.</param>
        /// <param name="regionPos">Region position.</param>
        /// <param name="regionManagerC">Region manager c.</param>
        public static void DestroyAllBuildingsWithoutTerritory(Account account, Action action, Controllers.RegionManagerController regionManagerC)
        {
            Dictionary<PositionI, long> copylist = new Dictionary<PositionI, long>(account.Buildings);
           
            foreach (var building in copylist)
            {
                var region = regionManagerC.GetRegion(building.Key.RegionPosition);
                if (region.GetClaimedTerritory(building.Key) == null)
                {
                    DestroyBuilding(region.GetEntity(building.Key.CellPosition), region, action, regionManagerC);
                    region.RemoveEntity(action.ActionTime, region.GetEntity(building.Key.CellPosition));
                }
            }
            account.Buildings.Clear();
            foreach (var test in copylist)
            {
                account.Buildings.Add(test.Key, test.Value);
            }
        }
    }
}