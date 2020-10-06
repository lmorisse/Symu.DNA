#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.DNA.Edges;

#endregion

namespace Symu.DNA.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Actor x Task network, called assignment network
    ///     Who (Actor) does what (Tasks)
    ///     Source : Actor
    ///     Target : Task
    /// </summary>
    public class ActorTaskNetwork : TwoModesNetwork<IActorTask> //ConcurrentTwoModesNetwork<IAgentId, IActorTask>
    {
    }
}