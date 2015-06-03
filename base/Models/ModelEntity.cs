using System;
using Newtonsoft.Json;

namespace @base.model
{
    public class ModelEntity
    {
        public ModelEntity()
        {
        }

        [JsonIgnore]
        public view.ViewEntity View;
        [JsonIgnore]
        public control.ControlEntity Control;
    }
}

