#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using Symu.Common.Interfaces;
using Symu.DNA.Edges;
using Symu.DNA.GraphNetworks;

#endregion

namespace Symu.DNA.Entities
{
    /// <summary>
    ///     A Task is a well defined procedures or goals of an organization, scheduled or planned activities
    ///     Default implementation of ITask
    /// </summary>
    /// <remarks>
    ///     This entity is called a Task in classical organization network analysis theory, but it's confusing with a task
    ///     on which agent works
    /// </remarks>
    public class TaskEntity : Entity<TaskEntity>, ITask
    {
        public const byte Class = ClassIdCollection.Task;
        public static IClassId ClassId => new ClassId(Class);
        public TaskEntity() {}
        public TaskEntity(MetaNetwork metaNetwork) : base(metaNetwork, metaNetwork?.Task, Class)
        {
        }
        public TaskEntity(MetaNetwork metaNetwork, string name) : base(metaNetwork, metaNetwork?.Task, Class,name)
        {
        }
        /// <summary>
        /// Copy the metaNetwork, the two modes networks where the entity exists
        /// </summary>
        /// <param name="entityId"></param>
        public override void CopyMetaNetworkTo(IAgentId entityId)
        {
            MetaNetwork.TaskKnowledge.CopyToFromSource(EntityId, entityId);
            MetaNetwork.ResourceTask.CopyToFromTarget(EntityId, entityId);
            MetaNetwork.ActorTask.CopyToFromTarget(EntityId, entityId);
        }

        public override void Remove()
        {
            base.Remove();
            MetaNetwork.TaskKnowledge.RemoveSource(EntityId);
            MetaNetwork.ResourceTask.RemoveTarget(EntityId);
            MetaNetwork.ActorTask.RemoveTarget(EntityId);
        }

        #region Task - Knowledge

        /// <summary>
        ///     List of knowledge required to work on this activity
        /// </summary>
        public List<IKnowledge> Knowledge
        {
            get
            {
                var knowledges = new List<IKnowledge>();
                var knowledgeIds = MetaNetwork.TaskKnowledge.TargetsFilteredBySource(EntityId);
                foreach (var knowledgeId in knowledgeIds)
                {
                    var knowledge = (IKnowledge) MetaNetwork.Knowledge.GetEntity(knowledgeId);
                    if (knowledge != null)
                    {
                        knowledges.Add(knowledge);
                    }
                    else
                    {
                        throw new NullReferenceException(nameof(knowledge));
                    }
                }

                return knowledges;
            }
        }

        /// <summary>
        ///     Add knowledge to an activity
        /// </summary>
        /// <param name="taskKnowledge"></param>
        public void AddKnowledge(IEntityKnowledge taskKnowledge)
        {
            MetaNetwork.TaskKnowledge.Add(taskKnowledge);
        }

        /// <summary>
        ///     Check that agent has the required knowledges to work on the activity
        /// </summary>
        /// <param name="actorKnowledgeIds"></param>
        /// <returns></returns>
        public bool CheckKnowledgeIds(List<IAgentId> actorKnowledgeIds)
        {
            if (actorKnowledgeIds is null)
            {
                throw new ArgumentNullException(nameof(actorKnowledgeIds));
            }

            return Knowledge.Any(knowledge => actorKnowledgeIds.Contains(knowledge.EntityId));
        }

        #endregion
    }
}