#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System.Collections.Generic;
using Symu.Common.Interfaces.Entity;

namespace Symu.DNA.Entities
{
    /// <summary>
    ///     A Task is a well defined procedures or goals of an organization, scheduled or planned activities
    /// </summary>
    public interface ITask : IEntity
    {
        //todo refactor with a TaskKnowledgeNetwork
        /// <summary>
        ///     List of knowledges required to work on this activity
        /// </summary>
        List<IKnowledge> Knowledge { get; }

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
    }
}