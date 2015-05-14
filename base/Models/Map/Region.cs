using System;
using @base.model.definitions;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Threading;

namespace @base.model
{
    public class Region
	{
        public class DatedActions
        {
            public DateTime DateTime
            {
                get { return m_dateTime; }
                set { m_dateTime = value; }
            }

            public ObservableCollection<model.Action> Actions
            {
                get { return m_actions; }
                set { m_actions = value; }
            }

            public RegionPosition RegionPosition
            {
                get { return m_regionPosition; }
                set { m_regionPosition = value; }
            }

            RegionPosition m_regionPosition;
            DateTime m_dateTime;
            ObservableCollection<model.Action> m_actions;
        }

        public class DatedEntities
        {
            public DateTime DateTime
            {
                get { return m_dateTime; }
                set { m_dateTime = value; }
            }

            public ObservableCollection<Entity> Entities
            {
                get { return m_entities; }
                set { m_entities = value; }
            }

            public RegionPosition RegionPosition
            {
                get { return m_regionPosition; }
                set { m_regionPosition = value; }
            }


            RegionPosition m_regionPosition;
            DateTime m_dateTime;
            ObservableCollection<Entity> m_entities;
        }

        public Region (RegionPosition regionPosition)
        {
            m_regionPosition = regionPosition;
            m_terrains  = new TerrainDefinition[Constants.REGION_SIZE_X, Constants.REGION_SIZE_Y];
            m_entities  = new DatedEntities();
            m_entities.Entities = new ObservableCollection<Entity>();
            m_actions   = new DatedActions();
            m_actions.Actions = new ObservableCollection<model.Action>();
            m_exist     = false;
            m_mutex     = new Mutex();
        }

        public Region (RegionPosition regionPosition, TerrainDefinition[ , ] terrains)
        {
            m_regionPosition = regionPosition;
            m_terrains  = terrains;
            m_entities  = new DatedEntities();
            m_actions   = new DatedActions();
            m_exist     = true;
            m_mutex     = new Mutex();
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
                return World.Instance.TerrainManager.GetTerrainDefinition(
                    TerrainDefinition.TerrainDefinitionType.Forbidden);
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
            var newDatedEntities = new DatedEntities();
            var newEntities = new ObservableCollection<Entity>(m_entities.Entities);
            newEntities.Add(entity);

            newDatedEntities.DateTime = dateTime;
            newDatedEntities.Entities = newEntities;

            m_entities = newDatedEntities;
        }

        public void RemoveEntity(DateTime dateTime, Entity entity)
        {
            var newDatedEntities = new DatedEntities();
            var newEntities = new ObservableCollection<Entity>(m_entities.Entities);
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
            var newActions = new ObservableCollection<model.Action>(m_actions.Actions);
            newActions.Insert(0, action);

            newDatedActions.DateTime = action.ActionTime;
            newDatedActions.Actions = newActions;

            m_actions = newDatedActions;

            m_inQueue.RemoveAt(0);

        }
        */
        public DatedEntities GetEntities()
        {
            try
            {
                LockRegion();
                return m_entities;
            }
            catch
            {

            }
            finally
            {
                Release();
            }

            return null;
        }

        public DatedActions GetCompletedActions(DateTime startTime)
        {
            try
            {
                LockRegion();
                var returnActions = new DatedActions();
                var currentActions = m_actions;

                var actionsCollection = new ObservableCollection<model.Action>();
                foreach (var action in currentActions.Actions)
                {
                    if (action.ActionTime <= startTime)
                    {
                        break;
                    }
                    actionsCollection.Add(action);
                }

                returnActions.Actions = actionsCollection;
                returnActions.DateTime = currentActions.DateTime;
                returnActions.RegionPosition = RegionPosition;

                return returnActions;
            }
            catch
            {
                
            }
            finally
            {
                Release();
            }

            return null;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="base.model.Region"/> really exist.
        /// Empty regions always return false. A region is empty if the terrain can't be loaded.
        /// (as example, bescause there were connection issues at the client - or the region file don't exist)
        /// </summary>
        /// <value><c>true</c> if exist; otherwise, <c>false</c>.</value>
        public bool Exist
        {
            get { return m_exist; }
        }

        public RegionPosition RegionPosition
        {
            get { return m_regionPosition; }
        }

        public bool TryLockRegion()
        {
            return m_mutex.WaitOne(0);
        }

        public bool LockRegion()
        {
            return m_mutex.WaitOne(-1);
        }

        public void Release()
        {
            m_mutex.ReleaseMutex();
        }

        public void AddCompletedAction(model.Action action)
        {
            var newDatedActions = new DatedActions();
            var newActions = new ObservableCollection<Action>(m_actions.Actions);
            newActions.Insert(0, action);

            newDatedActions.DateTime = action.ActionTime;
            newDatedActions.Actions = newActions;

            m_actions = newDatedActions;
        }

        bool m_exist;
        RegionPosition m_regionPosition;
        TerrainDefinition[ , ] m_terrains;

        DatedEntities m_entities;
        DatedActions m_actions;

        Mutex m_mutex;
	} 
}

