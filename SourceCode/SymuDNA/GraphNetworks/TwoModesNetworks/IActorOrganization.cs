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
    ///     Who works where
    ///     By default how is characterized by an allocation of capacity to define part-time membership
    /// </summary>
    public interface IActorOrganization
    {
        /// <summary>
        /// actorId who belongs to an organization
        /// </summary>
        IAgentId ActorId { get; }

        /// <summary>
        ///% of time allocation of the actorId in the organization
        /// Range 0 - 100
        /// </summary>
        float Allocation { get; set; }
    }
}