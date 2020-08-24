#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.Activities;

#endregion

namespace SymuDNATests.Classes
{
    public class TestAgentActivity : IAgentActivity
    {
        public TestAgentActivity(IAgentId id, IActivity activity)
        {
            Id = id;
            Activity = activity;
        }

        public IAgentId Id { get; }
        public IActivity Activity { get; set; }
    }
}