#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using System.Data.SqlTypes;
using Symu.Common.Interfaces.Agent;

namespace Symu.DNA.Networks.OneModeNetworks
{
    /// <summary>
    ///     An agent is an individual decision makers
    /// </summary>
    /// <remarks>Also named Actor in social network analysis</remarks>
    public interface IAgent: INullable, IDisposable
    {
        IAgentId AgentId {get;
            set;
        }
    }
}