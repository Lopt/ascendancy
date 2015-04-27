using System;
using @base.model.definitions;
using System.Collections.ObjectModel;

namespace @base.model
{
	public class Region
	{
        public Region (RegionPosition regionPosition)
        {
            m_regionPosition = regionPosition;
            m_terrains = new TerrainDefinition[Constants.REGION_SIZE_X, Constants.REGION_SIZE_Y];
            m_entities = new ObservableCollection<Entity>();
            m_inQueue = new ObservableCollection<control.action.Action>();
            m_completed = new ObservableCollection<control.action.Action>();
            m_exist = false;
        }

        public Region (RegionPosition regionPosition, TerrainDefinition[ , ] terrains)
        {
            m_regionPosition = regionPosition;
            m_terrains = terrains;
            m_entities = new ObservableCollection<Entity>();
            m_inQueue = new ObservableCollection<control.action.Action>();
            m_completed = new ObservableCollection<control.action.Action>();
            m_exist = true;
        }

        public void AddTerrain(TerrainDefinition[ , ] terrains)
        {
            m_terrains = terrains;
            m_exist = true;
        }

        public RegionPosition RegionPosition
        {
            get { return m_regionPosition; }
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

        public void AddAction(control.action.Action action)
        {
            m_inQueue.Add(action);
        }

        public Entity GetEntity(CellPosition cellPosition)
        {
            foreach (var entity in m_entities)
            {
                if (entity.Position.CellPosition == cellPosition)
                {
                    return entity;
                }
            }
            return null;
        }

        public void AddEntity(Entity entity)
        {
            m_entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            m_entities.Remove(entity);
        }

        public void ActionCompleted()
        {
            var action = m_inQueue[0];
            m_inQueue.RemoveAt(0);
            m_completed.Add(action);
        }
            
        /// <summary>
        /// Returns the first action
        /// </summary>
        /// <returns>Action which should be executed</returns>
        public @base.control.action.Action GetAction()
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

        private RegionPosition m_regionPosition;
        private TerrainDefinition[ , ] m_terrains;
        private ObservableCollection<Entity> m_entities;
        bool m_exist;

        // special break of the MVC Pattern
        private ObservableCollection<control.action.Action> m_completed;
        private ObservableCollection<control.action.Action> m_inQueue;

	} 
}

