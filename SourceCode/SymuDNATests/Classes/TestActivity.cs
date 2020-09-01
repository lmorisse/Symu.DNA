#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.OneModeNetworks;

namespace SymuDNATests.Classes
{
    /// <summary>
    ///     Defines an activity (a task type)
    /// </summary>
    /// <remarks>This entity is called a Task in classical organization network analysis theory, but it's confusing with a task on which agent works</remarks>
    public class TestActivity: IActivity
    {
        public TestActivity(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        /// <summary>
        ///     List of knowledges required to work on this activity
        /// </summary>
        public List<IKnowledge> Knowledges { get; } = new List<IKnowledge>();

        /// <summary>
        /// Add knowledge to an activity
        /// </summary>
        /// <param name="knowledge"></param>
        public void AddKnowledge(IKnowledge knowledge)
        {
            if (Knowledges.Contains(knowledge))
            {
                return;
            }

            Knowledges.Add(knowledge);
        }

        /// <summary>
        ///     Check that agent has the required knowledges to work on the activity
        /// </summary>
        /// <param name="agentKnowledgeIds"></param>
        /// <returns></returns>
        public bool CheckKnowledgeIds(List<IId> agentKnowledgeIds)
        {
            if (agentKnowledgeIds is null)
            {
                throw new ArgumentNullException(nameof(agentKnowledgeIds));
            }

            return Knowledges.Any(knowledge => agentKnowledgeIds.Contains(knowledge.Id));
        }

        public bool Equals(IActivity activity)
        {
            return activity is TestActivity test &&
                   Name == test.Name;
        }
    }
}