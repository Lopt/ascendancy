using Core.Controllers.Actions;
using Core.Models;
using client.Common.Controllers;
using client.Common.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;
using Microsoft.Xna.Framework.Graphics;


namespace client.Common.Manager
{
    public sealed class EntityManagerController
    {
        private static readonly Lazy<EntityManagerController> lazy =
            new Lazy<EntityManagerController>(() => new EntityManagerController());

        public static EntityManagerController Instance { get { return lazy.Value; } }



        private EntityManagerController ()
        {
            Entities = new Dictionary<int, Entity> ();
        }


        #region Entities

        public async Task LoadEntitiesAsync (Position currentGamePosition, RegionPosition centerRegionPosition)
        {
			var regionManagerC = (Manager.RegionManagerController) Core.Controllers.Controller.Instance.RegionManagerController;

            var worldRegions = regionManagerC.GetWorldNearRegionPositions (centerRegionPosition);
            var listRegions = new RegionPosition[25];
            int index = 0;
            foreach (var regionPosition in worldRegions) {
                listRegions [index] = regionPosition;
                ++index;
            }

            await LoadEntitiesAsync (currentGamePosition, listRegions);
        }



        public async Task LoadEntitiesAsync (Position currentGamePosition, RegionPosition[] listRegions)
        {

            var response = await NetworkController.Instance.LoadEntitiesAsync (currentGamePosition, listRegions);
            var entities = response.Entities;
            if (entities != null) {
                foreach (var regionEntities in entities) {
                    foreach (var entity in regionEntities) {
						var region = Core.Controllers.Controller.Instance.RegionManagerController.GetRegion (entity.Position.RegionPosition);
                        region.AddEntity (DateTime.Now, entity);
                        Add (entity);

                    }
                }
            }

            if (response.Actions != null)
            {
                var actions = new HashSet<Core.Models.Action> ();
                foreach (var regions in response.Actions)
                {
                    foreach (var action in regions)
                    {
                        actions.Add (action);
                    }   
                }

                var sortedActions = new SortedSet<Core.Models.Action> (actions, new Core.Models.Action.ActionComparer());

                foreach (var action in sortedActions)
                {
                    Worker.Queue.Enqueue (action);
                }
            }
        }

        #endregion

        Entity GetEntity (int Id)
        {
            Entity entity = null;
            Entities.TryGetValue (Id, out entity);
            return entity;
        }

        void Remove (Entity entity)
        {
            if (Entities.ContainsKey (entity.ID)) {
                Entities.Remove (entity.ID);
            }
        }

        void Add (Entity entity)
        {
            Entities [entity.ID] = entity;
        }

        public static Dictionary<int, Entity> Entities;
        public static Views.Worker Worker;
    }
}

