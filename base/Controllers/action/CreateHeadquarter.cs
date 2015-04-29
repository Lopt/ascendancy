using System;
using System.Collections.Concurrent;

using @base.model;
using @base.control;
using @base.model.definitions;

namespace @base.control.action
{
    public class CreateHeadquarter : Action
    {
        public const string CREATE_POSITION = "CreatePosition";
        /// <summary>
        /// Initializes a new instance of the <see cref="base.control.action.Action"/> class.
        /// </summary>
        /// <param name="actionType">Action type.</param>
        /// <param name="regions">Affected Regions of this action.</param>
        /// <param name="parameters">Parameters.</param>
        public CreateHeadquarter(Account account,
            ConcurrentDictionary<string, object> parameters)
            : base(account, ActionType.Create, parameters)
        {
        }

        public override Region GetMainRegion()
        {
            var combinedPos = (CombinedPosition) Parameters[CREATE_POSITION];
            return Controller.Instance.RegionManagerController.GetRegion(combinedPos.RegionPosition); 
        }

        /// <summary>
        /// Returns if the action is even possible.
        /// </summary>
        public override bool Possible()
        {
            var regionManagerC = Controller.Instance.RegionManagerController;
            if (Account.Headquarters.Count == 0)
            {
                /*
                if (Parameters.ContainsKey(CREATE_POSITION))
                {
                    var positionC = (CombinedPosition) Parameters[CREATE_POSITION];
                    var region = regionManagerC.GetRegion(positionC.RegionPosition);
                    if (region.Exist && this.Regions.Length == 1 && this.Regions[0] == region &&
                        region.GetEntity(positionC.CellPosition) != null)
                    {
                        return true;
                    }
                }*/
                return true;
            }
            return false;
        }

        /// <summary>
        /// Apply action-related changes to the world.
        /// Returns false if something went terrible wrong
        /// </summary>
        public override bool Do()
        {   
            var regionManagerC = Controller.Instance.RegionManagerController;
            var combinedPos = (CombinedPosition) Parameters[CREATE_POSITION];
            var region = regionManagerC.GetRegion(combinedPos.RegionPosition);

            var entity = new Entity(Guid.NewGuid(), 
                new UnitDefinition(Guid.NewGuid(),
                    Definition.DefinitionType.building,
                    UnitDefinition.UnitDefinitionType.Hero,
                    new Action[0], 
                    0, 0, 0, 0),
                combinedPos);

            entity.Position = combinedPos;
            region.AddEntity(ActionTime, entity);

            return true;
        }

        /// <summary>
        /// In case of errors, revert the world data to a valid state.
        /// </summary>        public bool Catch()
        public override bool Catch()
        {
            throw new NotImplementedException();
        }
    }
}

