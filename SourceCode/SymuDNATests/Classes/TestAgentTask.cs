#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces.Agent;
using Symu.DNA.Networks.OneModeNetworks;
using Symu.DNA.Networks.TwoModesNetworks;

#endregion

namespace SymuDNATests.Classes
{
    public class TestAgentTask : IAgentTask
    {
        public TestAgentTask(IAgentId id, ITask task)
        {
            Id = id;
            Task = task;
        }

        public IAgentId Id { get; }
        public ITask Task { get; set; }

        /// <summary>
        /// The value used to feed the matrix network
        /// For a binary matrix network, the value is 1
        /// </summary>
        public float Value => 1;
    }
}