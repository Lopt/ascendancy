﻿using @base.control;
using @base.model;
using client.Common.Controllers;
using client.Common.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;
using Microsoft.Xna.Framework.Graphics;


namespace client.Common.Manager
{
    public class EntityManagerController
    {

        // TODO: find better singleton implementation
        // http://csharpindepth.com/articles/general/singleton.aspx
        // NOT lazy-singletons: throws useless exceptions when initialisation failed
        private static EntityManagerController instance = null;

        public static EntityManagerController Instance {
            get {
                if (instance == null) {
                    instance = new EntityManagerController ();
                }
                return instance;
            }
        }



        private EntityManagerController ()
        {
            Entities = new Dictionary<int, Entity> ();
        }


        #region Entities

        public async Task LoadEntitiesAsync (Position currentGamePosition, RegionPosition centerRegionPosition)
        {
            var regionManagerC = (Manager.RegionManagerController)Controller.Instance.RegionManagerController;

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

            var response = await NetworkController.GetInstance.LoadEntitiesAsync (currentGamePosition, listRegions);
            var entities = response.Entities;
            if (entities != null) {
                foreach (var regionEntities in entities) {
                    foreach (var entity in regionEntities) {
                        var region = Controller.Instance.RegionManagerController.GetRegion (entity.Position.RegionPosition);
                        region.AddEntity (DateTime.Now, entity);
                        Add (entity);

                    }
                }
            }

            if (response.Actions != null)
            {
                var sortedActions = new HashSet<@base.model.Action> ();
                foreach (var regions in response.Actions)
                {
                    foreach (var action in regions)
                    {
                        sortedActions.Add (action);
                    }   
                }

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
        public static Worker Worker;
    }
}
