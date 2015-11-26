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
        /// Gets the surrounded territory arround the building.
        /// </summary>
        /// <param name="entity">Entity.</param>
        public static List<PositionI> GetSurroundedTerritory(PositionI entity)
        {
            var tilelist = new List<PositionI>();

            tilelist.Add(entity);
           
            for (int i = 0; i != Constants.HEADQUARTER_TERRITORY_RANGE; i++)
            {
                List<PositionI> result = new List<PositionI>(tilelist);

                foreach (var item in tilelist)
                {
                    var currentPosition = GetSurroundedFields(item);
                    foreach (var tile in currentPosition)
                    {
                        if (!result.Contains(tile))
                        {
                            result.Add(tile);
                        }
                    }
                }

                tilelist = result;
            }

           return tilelist;
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
            List.Add(288);
            List.Add(294);
            List.Add(300);
            List.Add(306);
            List.Add(312);
            return List;
        }

        /// <summary>
        /// Enables the barracks build options barracks = 282.
        /// </summary>
        /// <returns>The barracks build options.</returns>
        public static List<long> EnableBarracksBuildOptions()
        {
            var List = new List<long>();           
            List.Add(60);
            List.Add(66);
            List.Add(72);
            List.Add(78);
            List.Add(84);
            List.Add(90);
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
                   account.BuildableBuildings.Add(276, EnableHeadquarterBuildOptions());
               }
               else if (entityType == (long)Models.Definitions.EntityType.Barracks)
               {
                   account.BuildableBuildings.Add(282, EnableBarracksBuildOptions());
               }
            }
        }
    }        
}
