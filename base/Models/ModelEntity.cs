using System;
using Newtonsoft.Json;

namespace Core.Models
{
    public class ModelEntity
    {
        public ModelEntity()
        {
        }

        [JsonIgnore]
        public Views.ViewEntity View;
        [JsonIgnore]
        public Controllers.ControlEntity Control;
    }
}

