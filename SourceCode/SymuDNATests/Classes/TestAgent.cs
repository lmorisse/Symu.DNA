#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.Networks.OneModeNetworks;

namespace SymuDNATests.Classes
{
    /// <summary>
    ///     Defines a belief
    /// </summary>
    public class TestAgent : IAgent
    {
        public TestAgent(ushort id, byte classId)
        {
            AgentId= new AgentId(id, classId);
        }

        /// <summary>Indicates whether a structure is null. This property is read-only.</summary>
        /// <returns>
        /// <see cref="T:System.Data.SqlTypes.SqlBoolean" />
        /// <see langword="true" /> if the value of this object is null. Otherwise, <see langword="false" />.</returns>
        public bool IsNull => AgentId.IsNull;

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            
        }

        public IAgentId AgentId { get; set; }
    }
}