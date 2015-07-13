using System;
using Core.Models.Definitions;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading;

namespace Core.Models
{
    public class Region
    {
        public class DatedActions
        {
            public DateTime DateTime;
            public LinkedList<Core.Models.Action> Actions;
            public RegionPosition RegionPosition;
        }

        public class DatedEntities
        {
            public DateTime DateTime;
            public LinkedList<Entity> Entities;
            public RegionPosition RegionPosition;
        }

        public Region(Region region)
        {
            m_regionPosition = region.m_regionPosition;
            m_terrains = region.m_terrains;
            m_entities = region.m_entities;
            m_actions = new DatedActions();
            m_actions.Actions = new LinkedList<Core.Models.Action>();
            m_exist = region.m_exist;
            m_mutex = region.m_mutex;
        }

        public Region(RegionPosition regionPosition)
        {
            m_regionPosition = regionPosition;
            m_terrains = new TerrainDefinition[Constants.REGION_SIZE_X, Constants.REGION_SIZE_Y];
            m_entities = new DatedEntities();
            m_entities.Entities = new LinkedList<Entity>();
            m_actions = new DatedActions();
            m_actions.Actions = new LinkedList<Core.Models.Action>();
            m_exist = false;
            m_mutex = new ReaderWriterLockSlim();
        }

        public Region(RegionPosition regionPosition, TerrainDefinition[ , ] terrains)
        {
            m_regionPosition = regionPosition;
            m_terrains = terrains;
            m_entities = new DatedEntities();
            m_actions = new DatedActions();
            m_exist = true;
            m_mutex = new ReaderWriterLockSlim();
        }

        public void AddTerrain(TerrainDefinition[ , ] terrains)
        {
            m_terrains = terrains;
            m_exist = true;
        }

        public TerrainDefinition GetTerrain(CellPosition cellPosition)
        {
            var value = m_terrains[cellPosition.CellX, cellPosition.CellY];
            // standardvalue
            if (value == null)
            {
                return (TerrainDefinition)World.Instance.DefinitionManager.GetDefinition(
                    (int)EntityType.Forbidden);
            }
            return value;
        }

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

        public void AddEntity(DateTime dateTime, Entity entity)
        {
//            var newDatedEntities = new DatedEntities();
//            var newEntities = new LinkedList<Entity>(m_entities.Entities);
            m_entities.DateTime = dateTime;
            m_entities.Entities.AddFirst(entity);

//            m_entities = m_E;
        }

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

        /*
        public void ActionCompleted()
        {
            var action = m_inQueue[0];
            var newDatedActions = new DatedActions();
            var newActions = new LinkedList<model.Action>(m_actions.Actions);
            newActions.Insert(0, action);

            newDatedActions.DateTime = action.ActionTime;
            newDatedActions.Actions = newActions;

            m_actions = newDatedActions;

            m_inQueue.RemoveAt(0);

        }
        */
        public DatedEntities GetEntities()
        {
            return m_entities;
        }

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
        /// (as example, bescause there were connection issues at the client - or the region file don't exist)
        /// </summary>
        /// <value><c>true</c> if exist; otherwise, <c>false</c>.</value>
        public bool Exist
        {
            get
            {
                return m_exist;
            }
        }

        public RegionPosition RegionPosition
        {
            get
            {
                return m_regionPosition;
            }
        }

        public bool LockWriter()
        {
            return m_mutex.TryEnterWriteLock(Constants.REGION_LOCK_WAIT_TIME);
        }

        public void ReleaseWriter()
        {
            m_mutex.ExitWriteLock();
        }

        public bool LockReader()
        {
            return m_mutex.TryEnterReadLock(-1);
        }

        public void ReleaseReader()
        {
            m_mutex.ExitReadLock();
        }




        public void AddCompletedAction(Core.Models.Action action)
        {
            m_actions.DateTime = action.ActionTime;
            m_actions.Actions.AddFirst(action);
        }

        bool m_exist;
        RegionPosition m_regionPosition;
        TerrainDefinition[ , ] m_terrains;

        DatedEntities m_entities;
        DatedActions m_actions;

        ReaderWriterLockSlim m_mutex;
    }
}

