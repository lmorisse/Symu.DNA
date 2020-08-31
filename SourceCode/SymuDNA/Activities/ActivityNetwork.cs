#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Symu.Common.Interfaces.Agent;
using Symu.Common.Interfaces.Entity;
using IKnowledge = Symu.DNA.OneModeNetworks.Knowledge.IKnowledge;

#endregion

namespace Symu.DNA.Activities
{
    /// <summary>
    ///     Dictionary of all the activities of the network
    ///     for every groupId, the list of all the AgentActivity having activities in a group
    ///     Key => GroupId
    ///     Value => List of AgentActivity : AgentId, activity
    /// </summary>
    public class ActivityNetwork
    {
        /// <summary>
        ///     List of all GroupIds and their activities
        /// </summary>
        public readonly ConcurrentDictionary<IAgentId, List<IActivity>> GroupActivities =
            new ConcurrentDictionary<IAgentId, List<IActivity>>();

        /// <summary>
        ///     Key => GroupId
        ///     Value => list of AgentActivity : AgentId, activity
        /// </summary>
        public ConcurrentDictionary<IAgentId, List<IAgentActivity>> AgentActivities { get; } =
            new ConcurrentDictionary<IAgentId, List<IAgentActivity>>();

        public bool Any()
        {
            return GroupActivities.Any();
        }

        public void Clear()
        {
            GroupActivities.Clear();
            AgentActivities.Clear();
        }

        /// <summary>
        ///     Remove agent from network,
        ///     either it is a Group or an agent
        /// </summary>
        /// <param name="agentId"></param>
        public void RemoveAgent(IAgentId agentId)
        {
            if (Exists(agentId))
            {
                RemoveGroup(agentId);
            }

            RemoveMember(agentId);
        }

        #region for Group

        public IEnumerable<IAgentId> GetGroupIds()
        {
            return GroupActivities.Any() ? GroupActivities.Keys : null;
        }

        /// <summary>
        ///     Check that group exist
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public bool Exists(IAgentId groupId)
        {
            return GroupActivities.ContainsKey(groupId);
        }

        public void RemoveGroup(IAgentId groupId)
        {
            GroupActivities.TryRemove(groupId, out _);
            AgentActivities.TryRemove(groupId, out _);
        }

        /// <summary>
        ///     Add Group and initialize the dictionaries
        ///     To add the activities of the group, directly use AddActivities or AddActivity
        ///     which call AddGroup
        /// </summary>
        /// <param name="groupId"></param>
        public void AddGroup(IAgentId groupId)
        {
            if (Exists(groupId))
            {
                return;
            }

            GroupActivities.TryAdd(groupId, new List<IActivity>());
            AgentActivities.TryAdd(groupId, new List<IAgentActivity>());
        }

        /// <summary>
        ///     Add an activity to a group
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="groupId"></param>
        public void AddActivity(IActivity activity, IAgentId groupId)
        {
            AddGroup(groupId);
            if (!GroupActivities[groupId].Contains(activity))
            {
                GroupActivities[groupId].Add(activity);
            }
        }

        /// <summary>
        ///     Remove an activity from a group
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="groupId"></param>
        public void RemoveActivity(IAgentId groupId, IActivity activity)
        {
            if (Exists(groupId))
            {
                GroupActivities[groupId].Remove(activity);
            }
        }

        /// <summary>
        ///     Add an activity to a group
        /// </summary>
        /// <param name="activities"></param>
        /// <param name="groupId"></param>
        public void AddActivities(IEnumerable<IActivity> activities, IAgentId groupId)
        {
            if (activities is null)
            {
                throw new ArgumentNullException(nameof(activities));
            }

            AddGroup(groupId);
            foreach (var activity in activities)
            {
                if (!GroupActivities[groupId].Contains(activity))
                {
                    GroupActivities[groupId].Add(activity);
                }
            }
        }

        /// <summary>
        ///     check if a group has activities
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public bool GroupHasActivities(IAgentId groupId)
        {
            return Exists(groupId) && GroupActivities[groupId].Any();
        }

        /// <summary>
        ///     Get all the activities of a Group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IEnumerable<IActivity> GetGroupActivities(IAgentId groupId)
        {
            return Exists(groupId) ? GroupActivities[groupId] : new List<IActivity>();
        }

        #region Knowledge
        /// <summary>
        ///     Get the all the knowledges of a group categorize by activity
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>Dictionary</returns>
        public IDictionary<IActivity, List<IKnowledge>> GetActivitiesKnowledgesByActivity(
            IAgentId groupId)
        {
            if (!Exists(groupId))
            {
                return null;
            }

            var activitiesKnowledges = new Dictionary<IActivity, List<IKnowledge>>();
            foreach (var activity in GroupActivities[groupId])
            {
                activitiesKnowledges[activity] = activity.Knowledges;
            }

            return activitiesKnowledges;
        }

        /// <summary>
        ///     Get the all the knowledgesIds for all the activities of a group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>Dictionary</returns>
        public IEnumerable<IId> GetActivitiesKnowledgeIds(IAgentId groupId)
        {
            if (!Exists(groupId))
            {
                return null;
            }
            var activitiesKnowledges = new List<IId>();
            foreach (var activity in GroupActivities[groupId])
            {
                activitiesKnowledges.AddRange(activity.Knowledges.Select(x => x.Id));
            }

            return activitiesKnowledges.Distinct();
        }

        #endregion
        #endregion

        #region for agent

        public void RemoveMember(IAgentId agentId)
        {
            var groupIds = GetGroupIds();
            if (groupIds == null)
            {
                return;
            }

            foreach (var groupId in groupIds)
            {
                RemoveMember(agentId, groupId);
            }
        }

        public void RemoveMember(IAgentId agentId, IAgentId groupId)
        {
            if (Exists(groupId))
            {
                AgentActivities[groupId].RemoveAll(g => g.Id.Equals(agentId));
            }
        }

        /// <summary>
        ///     Add a worker to an activity of a group
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="groupId"></param>
        /// <param name="agentActivity"></param>
        public void AddAgentActivity(IAgentId agentId, IAgentId groupId, IAgentActivity agentActivity)
        {
            AddGroup(groupId);
            if (AgentHasAnActivityOn(agentId, groupId, agentActivity.Activity))
            {
                return;
            }

            AgentActivities[groupId].Add(agentActivity);
        }

        public bool AgentHasAnActivityOn(IAgentId agentId, IAgentId groupId, IActivity activity)
        {
            return Exists(groupId) && AgentActivities[groupId]
                .Exists(g => g.Id.Equals(agentId) && g.Activity.Equals(activity));
        }

        public bool AgentHasActivitiesOn(IAgentId agentId, IAgentId groupId)
        {
            return Exists(groupId) && AgentActivities[groupId].Exists(g => g.Id.Equals(agentId));
        }


        /// <summary>
        ///     Get all the activities of a groupId and an agentId is working on
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IEnumerable<IAgentActivity> GetAgentActivities(IAgentId agentId, IAgentId groupId)
        {
            return Exists(groupId)
                ? AgentActivities[groupId].FindAll(g => g.Id.Equals(agentId))
                : new List<IAgentActivity>();
        }
        /// <summary>
        ///     Get all the activities of a groupId and an agentId is working on
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IEnumerable<IActivity> GetActivities(IAgentId agentId, IAgentId groupId)
        {
            return Exists(groupId)
                ? AgentActivities[groupId].FindAll(g => g.Id.Equals(agentId)).Select(x => x.Activity)
                : new List<IActivity>();
        }

        /// <summary>
        ///     Get all the activities of an agentId
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public IEnumerable<IActivity> GetAgentActivities(IAgentId agentId)
        {
            var activities = new List<IActivity>();
            foreach (var agentActivities in AgentActivities)
            {
                activities.AddRange(agentActivities.Value.Where(x => x.Id.Equals(agentId))
                    .Select(agentActivity => agentActivity.Activity));
            }

            return activities;
        }

        public IEnumerable<IAgentId> FilterAgentIdsWithActivity(IEnumerable<IAgentId> agentIds, IAgentId groupId,
            IActivity activity)
        {
            if (agentIds is null)
            {
                throw new ArgumentNullException(nameof(agentIds));
            }

            return agentIds.Where(agentId => AgentHasAnActivityOn(agentId, groupId, activity)).ToList();
        }

        /// <summary>
        ///     Add a list of activities to AgentId for the groupId
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="groupId"></param>
        /// <param name="agentActivities"></param>
        public void AddActivities(IAgentId agentId, IAgentId groupId, IEnumerable<IAgentActivity> agentActivities)
        {
            if (agentActivities is null)
            {
                throw new ArgumentNullException(nameof(agentActivities));
            }

            foreach (var activity in agentActivities)
            {
                AddAgentActivity(agentId, groupId, activity);
            }
        }


        /// <summary>
        ///     Transfer the agentId activities from groupSourceId to groupTargetId
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="groupSourceId"></param>
        /// <param name="groupTargetId"></param>
        public void TransferTo(IAgentId agentId, IAgentId groupSourceId, IAgentId groupTargetId)
        {
            AddActivities(agentId, groupTargetId, GetAgentActivities(agentId, groupSourceId));
            RemoveMember(agentId, groupSourceId);
        }

        /// <summary>
        ///     Check if an agent has some activities in any group
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public bool HasAgentActivities(IAgentId agentId)
        {
            return AgentActivities.Any(a => a.Value.Exists(v => v != null && v.Id.Equals(agentId)));
        }

        #endregion
    }
}