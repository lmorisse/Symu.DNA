#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using Symu.DNA.GraphNetworks;

namespace Symu.DNA.Entities
{
    /// <summary>
    ///     Default implementation of ITask
    /// </summary>
    /// <remarks>This entity is called a Task in classical organization network analysis theory, but it's confusing with a task on which agent works</remarks>
    public class TaskEntity: ITask
    {

        public static TaskEntity CreateInstance()
        {
            return new TaskEntity();
        }

        private MetaNetwork _metaNetwork;
        /// <summary>
        /// Use for clone method
        /// </summary>
        private TaskEntity()
        {
        }
        public TaskEntity(MetaNetwork metaNetwork, byte classId)
        {
            Set(metaNetwork);
            EntityId = new AgentId(_metaNetwork.Task.NextId(), classId);
            _metaNetwork.Task.Add(this);
        }

        public void Remove()
        {
            _metaNetwork.TaskKnowledge.RemoveTask(EntityId);
            _metaNetwork.ResourceTask.RemoveTask(EntityId);
            _metaNetwork.ActorTask.RemoveTask(EntityId);
            _metaNetwork.Task.Remove(this);
        }

        public IAgentId EntityId { get; set; }

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public virtual object Clone()
        {
            var clone = CreateInstance();
            clone.EntityId = EntityId;
            return clone;
        }
        public void Set(MetaNetwork metaNetwork)
        {
            _metaNetwork = metaNetwork;
        }
        public override bool Equals(object obj)
        {
            return obj is TaskEntity task &&
                   EntityId.Equals(task.EntityId);
        }

        #region Task - Knowledge

        /// <summary>
        ///     List of knowledge required to work on this activity
        /// </summary>
        public List<IKnowledge> Knowledge => _metaNetwork.TaskKnowledge.GetKnowledgeIds(EntityId);//{ get; } = new List<IKnowledge>();

        /// <summary>
        /// Add knowledge to an activity
        /// </summary>
        /// <param name="knowledge"></param>
        public void AddKnowledge(IKnowledge knowledge)
        {
            //if (Knowledge.Contains(knowledge))
            //{
            //    return;
            //}

            //Knowledge.Add(knowledge);
            _metaNetwork.TaskKnowledge.Add(EntityId, knowledge);
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

            return Knowledge.Any(knowledge => agentKnowledgeIds.Contains(knowledge.EntityId));
        }
        #endregion
    }
}