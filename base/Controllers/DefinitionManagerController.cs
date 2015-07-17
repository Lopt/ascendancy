using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Core.Models;
using Core.Models.Definitions;

namespace Core.Controllers
{
    public class DefinitionManagerController
    {
        public DefinitionManagerController()
        {
            m_definitionManager = World.Instance.DefinitionManager;
        }

        public DefinitionManager DefinitionManager
        {
            get
            {
                return m_definitionManager;
            }
        }

        DefinitionManager m_definitionManager;
    }



}

