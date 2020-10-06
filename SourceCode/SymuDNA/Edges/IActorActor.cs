#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces;

#endregion

namespace Symu.DNA.Edges
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
    public interface IActorActor : IEdge
    {
        bool IsActive { get; }
        bool IsPassive { get; }

        bool HasLink(IAgentId actorId1, IAgentId actorId2);
        bool Equals(IAgentId actorId);

        /// <summary>
        ///     Increase the weight of the interaction - if interaction are weighted
        /// </summary>
        void IncreaseWeight();

        /// <summary>
        ///     Decrease the weight of the interaction - if interaction are weighted
        /// </summary>
        void DecreaseWeight();

        /// <summary>
        ///     Actor has active interaction based on the weight of the interaction
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        bool HasActiveInteraction(IAgentId actorId);

        /// <summary>
        ///     Actor has active interaction based on the weight of the interaction
        /// </summary>
        /// <param name="actorId1"></param>
        /// <param name="actorId2"></param>
        /// <returns></returns>
        bool HasActiveInteraction(IAgentId actorId1, IAgentId actorId2);

        /// <summary>
        ///     Actor has passive interaction based on the weight of the interaction
        /// </summary>
        /// <param name="actorId1"></param>
        /// <param name="actorId2"></param>
        /// <returns></returns>
        bool HasPassiveInteraction(IAgentId actorId1, IAgentId actorId2);
    }
}