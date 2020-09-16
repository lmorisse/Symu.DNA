#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using Symu.Common.Interfaces.Agent;

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Actor x Actor network, called interaction network, social network
    ///     network of social links between agents, with their interaction type
    ///     Who interacts to / knows who
    ///     link are bidirectional.
    ///     AgentId1 has the smallest key
    ///     AgentId2 has the highest key
    /// </summary>
    /// <remarks>You can define your own definition of a passive/active interaction</remarks>
    public interface IActorActor
    {
        /// <summary>
        ///     Unique key of the actor with the smallest key
        /// </summary>
        IAgentId Id1 { get; }

        /// <summary>
        ///     Unique key of the actor with the highest key
        /// </summary>
        IAgentId Id2 { get; }
        float Weight { get; }

        bool IsActive {get; }
        bool IsPassive { get; }

        bool HasLink(IAgentId actorId1, IAgentId actorId2);
        bool Equals(object obj);
        bool Equals(IActorActor obj);

        /// <summary>
        /// Increase the weight of the interaction - if interaction are weighted
        /// </summary>
        void IncreaseWeight();
        /// <summary>
        /// Decrease the weight of the interaction - if interaction are weighted
        /// </summary>
        void DecreaseWeight();
        /// <summary>
        /// Actor has active interaction based on the weight of the interaction
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        bool HasActiveInteractions(IAgentId actorId);
        /// <summary>
        /// Actor has active interaction based on the weight of the interaction
        /// </summary>
        /// <param name="actorId1"></param>
        /// <param name="actorId2"></param>
        /// <returns></returns>
        bool HasActiveInteraction(IAgentId actorId1, IAgentId actorId2);
        /// <summary>
        /// Actor has passive interaction based on the weight of the interaction
        /// </summary>
        /// <param name="actorId1"></param>
        /// <param name="actorId2"></param>
        /// <returns></returns>
        bool HasPassiveInteraction(IAgentId actorId1, IAgentId actorId2);
    }
}