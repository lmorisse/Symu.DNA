#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System.Collections.Generic;
using Symu.Common.Interfaces.Entity;
using IKnowledge = Symu.DNA.OneModeNetworks.IKnowledge;

namespace Symu.DNA.OneModeNetworks
{
    /// <summary>
    ///     Defines an activity (a task type)
    /// </summary>
    /// <remarks>This entity is called a Task in classical organization network analysis theory, but it's confusing with a task on which agent works</remarks>
    public interface IActivity
    {
        /// <summary>
        ///     List of knowledges required to work on this activity
        /// </summary>
        List<IKnowledge> Knowledges { get; }

        /// <summary>
        /// Add knowledge to an activity
        /// </summary>
        /// <param name="knowledge"></param>
        void AddKnowledge(IKnowledge knowledge);

        /// <summary>
        ///     Check that agent has the required knowledges to work on the activity
        /// </summary>
        /// <param name="agentKnowledgeIds"></param>
        /// <returns></returns>
        bool CheckKnowledgeIds(List<IId> agentKnowledgeIds);
        bool Equals(IActivity activity);
    }
}