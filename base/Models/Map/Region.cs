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
            m_terrains = new TerrainDefinition[Constants.regionSizeX, Constants.regionSizeY];
            m_entities = new ObservableCollection<Entity>();
        }

        public Region (RegionPosition regionPosition, TerrainDefinition[ , ] terrains)
        {
            m_regionPosition = regionPosition;
            m_terrains = terrains;
            //new TerrainDefinition[Constants.regionSizeX,
            //    Constants.regionSizeY];
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

        public void AddAction(@base.control.action.Action action)
        {
            m_inQueue.Add(action);
        }

        public void ActionCompleted()
        {
            var action = m_inQueue[0];
            m_inQueue.RemoveAt(0);
            m_completed.Add(action);
        }
            

        private RegionPosition m_regionPosition;
        private TerrainDefinition[ , ] m_terrains;
        private ObservableCollection<Entity> m_entities;
        private ObservableCollection<@base.control.action.Action> m_completed;
        private ObservableCollection<@base.control.action.Action> m_inQueue;
	} 
}

