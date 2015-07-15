using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Core.Models;
using Core.Models.Definitions;

namespace Core.Controllers
{
    public class RegionManagerController
    {
        public RegionManagerController()
        {
        }

        virtual public Region GetRegion(RegionPosition regionPosition)
        {
            throw new NotImplementedException();
        }

    }
}

