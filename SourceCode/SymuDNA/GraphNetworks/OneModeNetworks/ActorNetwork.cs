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
using Symu.DNA.Entities;

#endregion

namespace Symu.DNA.GraphNetworks.OneModeNetworks
{
   
    /// <summary>
    ///     List of the actors of the meta network
    ///     An actor is an individual decision makers
    /// </summary>
    /// <remarks>Also named agent in social network analysis, but agent is used for multi agents system</remarks>
    public class ActorNetwork : OneModeNetwork<IActor>
    {
        //public ConcurrentDictionary<IAgentId, IActor> List = new ConcurrentDictionary<IAgentId, IActor>();
        //public int Count => List.Count;

        //public bool Any()
        //{
        //    return List.Count > 0;
        //}
        //public void Clear()
        //{
        //    List.Clear();
        //}
        ///// <summary>
        /////     Latest unique index of task
        ///// </summary>
        //private ushort _entityIndex;
        //public ushort NextId()
        //{
        //    return _entityIndex++;
        //}

        //public void Add(IActor actor)
        //{
        //    if (actor == null)
        //    {
        //        throw new ArgumentNullException(nameof(actor));
        //    }

        //    if (!Exists(actor.EntityId))
        //    {
        //        List[actor.EntityId] = actor;
        //    }
        //}
        //public void Remove(IActor actor)
        //{
        //    if (Exists(actor.EntityId))
        //    {
        //        var remove = List.TryRemove(actor.EntityId, out _);
        //        if (!remove)
        //        {
        //            throw new Exception("Concurrent access");
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("Agent " + actor.EntityId + " does not exist (ConcurrentEnvironment.Remove)");
        //    }
        //}

        //public void Remove(IAgentId actorId)
        //{      
        //    if (Exists(actorId))
        //    {
        //        var remove = List.TryRemove(actorId, out _);
        //        if (!remove)
        //        {
        //            throw new Exception("Concurrent access");
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("Agent " + actorId + " does not exist (ConcurrentEnvironment.Remove)");
        //    }
        //}

        //public bool Exists(IAgentId actorId)
        //{
        //    if (actorId == null)
        //    {
        //        throw new ArgumentNullException(nameof(actorId));
        //    }

        //    return List.ContainsKey(actorId);
        //}

        ///// <summary>
        /////     Get a typed actor by its agentId
        ///// </summary>
        ///// <typeparam name="TAgent"></typeparam>
        ///// <param name="actorId"></param>
        ///// <returns>The typed agent</returns>
        //public TAgent GetEntity<TAgent>(IAgentId actorId) where TAgent : IActor
        //{
        //    return (TAgent) GetEntity(actorId);
        //}
        

        //public IActor GetEntity(IAgentId actorId)
        //{
        //    return Exists(actorId) ? List[actorId] : default;
        //}

        ///// <summary>
        /////     Returns a list with the agentIds.
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<IAgentId> GetEtityIds()
        //{
        //    return List.Keys;
        //}

        ///// <summary>
        /////     Returns a list with the actors.
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<IActor> GetEntities()
        //{
        //    return List.Values;
        //}

        ///// <summary>
        /////     Returns a typed list with the actors of Type TActor
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<TActor> GetEntities<TActor>() where TActor: IActor
        //{
        //    return List.Values.OfType<TActor>();
        //}

        ///// <summary>
        /////     The number of actors in the environment
        ///// </summary>
        ////todo refactor with .OfType<TAgent>() and remove IClassId
        //public ushort CountByClassId(IClassId classId)
        //{
        //    var count = List.Values.Count(a => a.EntityId.Equals(classId));
        //    return Convert.ToUInt16(count);
        //}

        ///// <summary>
        /////     Returns a list of all the actorIds filtered by their ClassId.
        ///// </summary>
        ////todo refactor with .OfType<TAgent>()and remove IClassId
        //public IEnumerable<IAgentId> FilteredIdsByClassId(IClassId classId)
        //{
        //    return List.Keys.Where(a => a.Equals(classId));
        //}

        ///// <summary>
        /////     Returns a list of all the actors filtered by their ClassId.
        ///// </summary>
        ////todo refactor with .OfType<TAgent>()and remove IClassId
        //public IEnumerable<IActor> FilteredByClassId(IClassId classId)
        //{
        //    return List.Values.Where(a => a.EntityId.Equals(classId));
        //}

        //public IAgentId GetId(IId id)
        //{
        //    return List.Keys.ToList().Find(x => x.Id.Equals(id));
        //}

        //public IReadOnlyList<IAgentId> ToVector()
        //{
        //    return GetEtityIds().OrderBy(x => x.Id).ToList();
        //}
        //public void CopyTo(MetaNetwork metaNetwork, ActorNetwork entity)
        //{
        //    foreach (var agent in List)
        //    {
        //        if (!(agent.Value.Clone() is IActor clone))
        //        {
        //            throw new NullReferenceException(nameof(clone));
        //        }
        //        clone.Set(metaNetwork);
        //        entity.List.TryAdd(agent.Key, clone);
        //    }
        //    entity._entityIndex = _entityIndex;
        //}
    }
}