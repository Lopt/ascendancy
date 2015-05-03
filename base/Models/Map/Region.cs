using System;
using @base.model.definitions;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.ComponentModel;

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
            m_inQueue   = new ObservableCollection<model.Action>();
            m_exist     = false;
        }

        public Region (RegionPosition regionPosition, TerrainDefinition[ , ] terrains)
        {
            m_regionPosition = regionPosition;
            m_terrains  = terrains;
            m_entities  = new DatedEntities();
            m_actions   = new DatedActions();
            m_inQueue   = new ObservableCollection<model.Action>();
            m_exist     = true;
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

        public void AddAction(model.Action action)
        {
            m_inQueue.Add(action);
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

        public DatedEntities GetEntities()
        {
            return m_entities;
        }

        public DatedActions GetCompletedActions(DateTime startTime)
        {
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
            
        /// <summary>
        /// Returns the first action
        /// </summary>
        /// <returns>Action which should be executed</returns>
        public model.Action GetAction()
        {
            if (m_inQueue.Count > 0)
            {
                return m_inQueue[0];
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


        bool m_exist;
        RegionPosition m_regionPosition;
        TerrainDefinition[ , ] m_terrains;

        DatedEntities m_entities;
        DatedActions m_actions;
        ObservableCollection<model.Action> m_inQueue;

	} 
}

