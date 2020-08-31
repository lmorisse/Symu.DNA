#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using System.Data.SqlTypes;
using Symu.Common.Interfaces.Agent;

namespace Symu.DNA.OneModeNetworks.Agent
{
    /// <summary>
    /// Interface for agent used in the MetaNetwork
    /// </summary>
    public interface IAgent: INullable, IDisposable
    {
        IAgentId AgentId {get;
            set;
        }
    }
}