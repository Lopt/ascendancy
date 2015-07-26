using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;

using Microsoft.Xna.Framework.Graphics;

using Core.Controllers.Actions;
using Core.Models;
using Client.Common.Controllers;
using Client.Common.Helper;



namespace Client.Common.Manager
{
    /// <summary>
    /// The Entity manager controller to control(load,remove,get and save) the entities.
    /// </summary>
    public sealed class EntityManagerController
    {
        /// <summary>
        /// The lazy singleton.
        /// </summary>
        private static readonly Lazy<EntityManagerController> lazy =
            new Lazy<EntityManagerController>(() => new EntityManagerController());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static EntityManagerController Instance { get { return lazy.Value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Client.Common.Manager.EntityManagerController"/> class.
        /// </summary>
        private EntityManagerController()
        {
            Entities = new Dictionary<int, Entity>();
        }


        #region Entities

        /// <summary>
        /// Loads the entities async to the regions around the surrender center region and add the entities to these regions.
        /// </summary>
        /// <returns>The task async.</returns>
        /// <param name="currentGamePosition">Current game position.</param>
        /// <param name="centerRegionPosition">Center region position.</param>
        public async Task LoadEntitiesAsync(Position currentGamePosition, RegionPosition centerRegionPosition)
        {
            var regionManagerC = (Manager.RegionManagerController)Core.Controllers.Controller.Instance.RegionManagerController;

            var worldRegions = regionManagerC.GetWorldNearRegionPositions(centerRegionPosition);
            var listRegions = new RegionPosition[25];
            int index = 0;
            foreach (var regionPosition in worldRegions)
            {
                listRegions[index] = regionPosition;
                ++index;
            }

            await LoadEntitiesAsync(currentGamePosition, listRegions);
        }

        /// <summary>
        /// Loads the entities async from the list of regions and add the entities to these regions.
        /// Also add the loaded actions to the view worker queue.
        /// </summary>
        /// <returns>The task async.</returns>
        /// <param name="currentGamePosition">Current game position.</param>
        /// <param name="listRegions">List regions.</param>
        public async Task LoadEntitiesAsync(Position currentGamePosition, RegionPosition[] listRegions)
        {

            var response = await NetworkController.Instance.LoadEntitiesAsync(currentGamePosition, listRegions);
            var entities = response.Entities;
            if (entities != null)
            {
                foreach (var regionEntities in entities)
                {
                    foreach (var entity in regionEntities)
                    {
                        var region = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion(entity.Position.RegionPosition);
                        region.AddEntity(DateTime.Now, entity);
                        Add(entity);

                    }
                }
            }

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
                    Worker.Queue.Enqueue(action);
                }
            }
        }

        #endregion

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="Id">Identifier.</param>
        Entity GetEntity(int Id)
        {
            Entity entity = null;
            Entities.TryGetValue(Id, out entity);
            return entity;
        }

        /// <summary>
        /// Remove the specified entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        void Remove(Entity entity)
        {
            if (Entities.ContainsKey(entity.ID))
            {
                Entities.Remove(entity.ID);
            }
        }

        /// <summary>
        /// Add the specified entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        void Add(Entity entity)
        {
            Entities[entity.ID] = entity;
        }

        /// <summary>
        /// The entities.
        /// </summary>
        public static Dictionary<int, Entity> Entities;

        /// <summary>
        /// The worker.
        /// </summary>
        public static Views.Worker Worker;
    }
}

