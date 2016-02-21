using Client.Common.Models;

namespace Client.Common.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    using Client.Common.Controllers;
    using Client.Common.Helper;
    using Core.Models;

    /// <summary>
    /// The Entity manager controller to control(load,remove,get and save) the entities.
    /// </summary>
    public sealed class EntityManagerController
    {
        /// <summary>
        /// The lazy singleton.
        /// </summary>
        private static readonly Lazy<EntityManagerController> Singleton =
            new Lazy<EntityManagerController>(() => new EntityManagerController());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static EntityManagerController Instance
        {
            get
            {
                return Singleton.Value;
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="EntityManagerController"/> class from being created.
        /// </summary>
        private EntityManagerController()
        {
            m_entities = new Dictionary<int, Entity>();
        }

        #region Entities

        /// <summary>
        /// Loads the entities async to the regions around the surrender center region and add the entities to these regions.
        /// </summary>
        /// <returns>The task async.</returns>
        /// <param name="currentGamePosition">Current game position.</param>
        /// <param name="centerRegionPosition">Center region position.</param>
        public async Task LoadEntitiesAsync(RegionPosition regionPosition)
        {
            var listRegions = new RegionPosition[1];
            listRegions[0] = regionPosition;
           
            await LoadEntitiesAsync(listRegions);
        }

        /// <summary>
        /// Loads the entities async from the list of regions and add the entities to these regions.
        /// Also add the loaded actions to the view worker queue.
        /// </summary>
        /// <returns>The task async.</returns>
        /// <param name="currentGamePosition">Current game position.</param>
        /// <param name="listRegions">List regions.</param>
        public async Task LoadEntitiesAsync(RegionPosition[] listRegions)
        {
            var response = await NetworkController.Instance.LoadEntitiesAsync(Geolocation.Instance.CurrentGamePosition, listRegions);
           
            if (response.Actions != null)
            {
                var actions = new HashSet<Core.Models.Action>();
                foreach (var regions in response.Actions)
                {
                    foreach (var action in regions)
                    {
                        actions.Add(action);
                    }   
                }

                var sortedActions = new SortedSet<Core.Models.Action>(actions, new Core.Models.Action.ActionComparer());

                foreach (var action in sortedActions)
                {
                    Views.Worker.Instance.Queue.Enqueue(action);
                }
            }
        }

        #endregion

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="id">Identifier which entity should be returned.</param>
        private Entity GetEntity(int id)
        {
            Entity entity = null;
            m_entities.TryGetValue(id, out entity);
            return entity;
        }

        /// <summary>
        /// Remove the specified entity.
        /// </summary>
        /// <param name="entity">Entity which should be removed.</param>
        private void Remove(Entity entity)
        {
            if (m_entities.ContainsKey(entity.ID))
            {
                m_entities.Remove(entity.ID);
            }
        }

        /// <summary>
        /// Add the specified entity.
        /// </summary>
        /// <param name="entity">Entity which should be added.</param>
        private void Add(Entity entity)
        {
            m_entities[entity.ID] = entity;
        }

        /// <summary>
        /// The entities.
        /// </summary>
        private Dictionary<int, Entity> m_entities;

    }
}