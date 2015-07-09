using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using @base.model;
using @base.model.definitions;

namespace @base.control
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

