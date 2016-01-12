namespace Core.Models
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;
    using Core.Models.Definitions;

    /// <summary>
    /// A part of the world. Contains a RegionPosition which determinates where (in the world) the region is.
    /// Also contains all entities to a specific time and all actions which were executed on the region.
    /// </summary>
    public class Region : ModelEntity
    {
        /// <summary>
        /// Actions and an date when the last action was executed.
        /// </summary>
        public class DatedActions
        {
            /// <summary>
            /// The date time of the last executed action.
            /// </summary>
            public DateTime DateTime;

            /// <summary>
            /// The actions.
            /// </summary>
            public LinkedList<Core.Models.Action> Actions;

            /// <summary>
            /// RegionPosition of the region which contains the actions
            /// </summary>
            public RegionPosition RegionPosition;
        }

        /// <summary>
        /// Entities and an date when the last entity was changed
        /// </summary>
        public class DatedEntities
        {
            /// <summary>
            /// The last date time when an entity was changed.
            /// </summary>
            public DateTime DateTime;

            /// <summary>
            /// The entities.
            /// </summary>
            public LinkedList<Entity> Entities;

            /// <summary>
            /// RegionPosition of the region which contains the entities
            /// </summary>
            public RegionPosition RegionPosition;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Region"/> class.
        /// </summary>
        /// <param name="region">Region which should be copied.</param>
        public Region(Region region)
        {
            m_regionPosition = region.m_regionPosition;
            m_terrains = region.m_terrains;
            m_entities = region.m_entities;
            m_territory = region.m_territory;
            m_actions = new DatedActions();
            m_actions.Actions = new LinkedList<Core.Models.Action>();
            m_exist = region.m_exist;
            m_mutex = region.m_mutex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Region"/> class.
        /// Use Only when there are not data. It will create an empty region where nothing can be changed.
        /// </summary>
        /// <param name="regionPosition">Region position.</param>
        public Region(RegionPosition regionPosition)
        {
            m_regionPosition = regionPosition;
            m_terrains = new TerrainDefinition[Constants.REGION_SIZE_X, Constants.REGION_SIZE_Y];
            m_entities = new DatedEntities();
            m_entities.Entities = new LinkedList<Entity>();
            m_territory = new Dictionary<CellPosition, Account>();
            m_actions = new DatedActions();
            m_actions.Actions = new LinkedList<Core.Models.Action>();
            m_exist = false;
            m_mutex = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Core.Models.Region"/> class.
        /// </summary>
        /// <param name="regionPosition">Region position.</param>
        /// <param name="terrains">Terrains of the region.</param>
        public Region(RegionPosition regionPosition, TerrainDefinition[,] terrains)
        {
            m_regionPosition = regionPosition;
            m_terrains = terrains;
            m_entities = new DatedEntities();
            m_territory = new Dictionary<CellPosition, Account>();
            throw new Exception("Territory need to be load");
            m_actions = new DatedActions();
            m_exist = true;
            m_mutex = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// Adds the terrain afterward if the region was created without.
        /// </summary>
        /// <param name="terrains">2D array of TerrainsType </param>
        public void AddTerrain(TerrainDefinition[,] terrains)
        {
            m_terrains = terrains;
            m_exist = true;
        }

        /// <summary>
        /// Returns the TerrainDefinition of the given CellPosition
        /// </summary>
        /// <returns>Returns the TerrainDefinition of the specific given cellPosition.</returns>
        /// <param name="cellPosition">Cell position.</param>
        public TerrainDefinition GetTerrain(CellPosition cellPosition)
        {
            var value = m_terrains[cellPosition.CellX, cellPosition.CellY];
            // standardvalue
            if (value == null)
            {
                return (TerrainDefinition)World.Instance.DefinitionManager.GetDefinition(EntityType.Forbidden);
            }
            return value;
        }

        /// <summary>
        /// Returns Buildings or Units at the given position
        /// </summary>
        /// <returns>The entity or null when there was no entity.</returns>
        /// <param name="cellPosition">Cell position.</param>
        public Entity GetEntity(CellPosition cellPosition)
        {
            foreach (var entity in m_entities.Entities)
            {
                if (entity.Position.CellPosition == cellPosition)
                {
                    return entity;
                }
            }
            return null;
        }

        /// <summary>
        /// Adds the entity to a specific time.
        /// </summary>
        /// <param name="dateTime">Date time when the action was called.</param>
        /// <param name="entity">Entity which should be added to this region.</param>
        public void AddEntity(DateTime dateTime, Entity entity)
        {
            m_entities.DateTime = dateTime;
            m_entities.Entities.AddFirst(entity);
        }

        /// <summary>
        /// Removes the entity to a specific time.
        /// </summary>
        /// <param name="dateTime">Date time when the action was called.</param>
        /// <param name="entity">Entity which should be removed from this region.</param>
        public void RemoveEntity(DateTime dateTime, Entity entity)
        {
            var newDatedEntities = new DatedEntities();
            var newEntities = new LinkedList<Entity>(m_entities.Entities);
            newEntities.Remove(entity);

            newDatedEntities.DateTime = dateTime;
            newDatedEntities.Entities = newEntities;
            newDatedEntities.RegionPosition = RegionPosition;

            m_entities = newDatedEntities;
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <returns>The entities.</returns>
        public DatedEntities GetEntities()
        {
            return m_entities;
        }

        /// <summary>
        /// Claims the territory.
        /// </summary>
        /// <param name="territoryList">Territory list.</param>
        /// <param name="account">Current Account.</param>
        /// <param name="entityRegionPos">Entity region position.</param>
        /// <param name="regionMan">Region man.</param>
        public void ClaimTerritory(HashSet<PositionI> territoryList, Account account, RegionPosition entityRegionPos, RegionManager regionMan)
        { 
            foreach (var position in territoryList)
            {   
                if (!m_territory.ContainsKey(position.CellPosition))
                {
                    if (position.RegionPosition == entityRegionPos)
                    {
                        m_territory.Add(position.CellPosition, account);
                    }
                    else
                    {
                        regionMan.GetRegion(position.RegionPosition).m_territory.Add(position.CellPosition, account);
                    }                   
                }
            }
        }

        /// <summary>
        /// Gets the claimed territory.
        /// </summary>
        /// <returns>The claimed territory.</returns>
        /// <param name="position">Current Position.</param>
        public Account GetClaimedTerritory(CellPosition position)
        {
            Account result;
            if (m_territory.TryGetValue(position, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Frees the claimed territory.
        /// </summary>
        /// <param name="territoryList">Territory list.</param>
        /// <param name="account">Current Account.</param>
        public void FreeClaimedTerritory(HashSet<PositionI> territoryList, Account account)
        {
            foreach (var position in territoryList)
            {
                if (m_territory.ContainsKey(position.CellPosition))
                {
                    m_territory.Remove(position.CellPosition);
                }                        
            }
        }

        /// <summary>
        /// Builds a new list with all actions which are lower then the given startTime.
        /// </summary>
        /// <param name="startTime">Date time when region was last time loaded.</param>
        /// <returns>DatedActions from startTime</returns>
        public DatedActions GetCompletedActions(DateTime startTime)
        {
            var returnActions = new DatedActions();
            var currentActions = m_actions;

            var actionsCollection = new LinkedList<Core.Models.Action>();
            foreach (var action in currentActions.Actions)
            {
                if (action.ActionTime <= startTime)
                {
                    break;
                }
                actionsCollection.AddFirst(action);
            }

            returnActions.Actions = actionsCollection;
            returnActions.DateTime = currentActions.DateTime;
            returnActions.RegionPosition = RegionPosition;

            return returnActions;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="base.model.Region"/> really exist.
        /// Empty regions always return false. A region is empty if the terrain can't be loaded.
        /// (as example, cause there were connection issues at the client - or the region file don't exist)
        /// </summary>
        /// <value><c>true</c> if exist; otherwise, <c>false</c>.</value>
        public bool Exist
        {
            get
            {
                return m_exist;
            }
        }

        /// <summary>
        /// Gets the region position.
        /// </summary>
        /// <value>The region position.</value>
        public RegionPosition RegionPosition
        {
            get
            {
                return m_regionPosition;
            }
        }

        /// <summary>
        /// Locks the region for writer.
        /// </summary>
        /// <returns><c>true</c>, if region was locked, <c>false</c> otherwise.</returns>
        public bool LockWriter()
        {
            return m_mutex.TryEnterWriteLock(Constants.REGION_LOCK_WAIT_TIME);
        }

        /// <summary>
        /// Releases the writer lock.
        /// </summary>
        public void ReleaseWriter()
        {
            m_mutex.ExitWriteLock();
        }

        /// <summary>
        /// Locks the lock for reader.
        /// </summary>
        /// <returns><c>true</c>, if reader lock was locked, <c>false</c> otherwise.</returns>
        public bool LockReader()
        {
            return m_mutex.TryEnterReadLock(-1);
        }

        /// <summary>
        /// Releases the reader lock.
        /// </summary>
        public void ReleaseReader()
        {
            m_mutex.ExitReadLock();
        }

        /// <summary>
        /// An Action was executed and affected this region, then it should be added with AddCompletedAction.
        /// </summary>
        /// <param name="action">Action which changed this region.</param>
        public void AddCompletedAction(Core.Models.Action action)
        {
            m_actions.DateTime = action.ActionTime;
            m_actions.Actions.AddFirst(action);
        }

        /// <summary>
        /// true if this region exist (and has data)
        /// </summary>
        private bool m_exist;

        /// <summary>
        /// The region position.
        /// </summary>
        private RegionPosition m_regionPosition;

        /// <summary>
        /// Terrain data of region
        /// </summary>
        private TerrainDefinition[,] m_terrains;

        /// <summary>
        /// Current active entities of the region
        /// </summary>
        private DatedEntities m_entities;

        /// <summary>
        /// Current owned territory from user.
        /// </summary>
        private Dictionary<CellPosition, Account> m_territory;

        /// <summary>
        /// already executed action in this region
        /// </summary>
        private DatedActions m_actions;

        /// <summary>
        /// writer/reader lock
        /// </summary>
        private ReaderWriterLockSlim m_mutex;
    }
}