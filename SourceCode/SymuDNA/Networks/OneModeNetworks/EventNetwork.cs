#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

#endregion

#region using directives

using System.Collections.Generic;
using System.Linq;
using Symu.Common.Interfaces.Entity;

#endregion

namespace Symu.DNA.Networks.OneModeNetworks
{
    /// <summary>
    ///     EventNetwork helps you schedule one shot events that happen during the simulation
    /// </summary>
    public class EventNetwork
    {

        /// <summary>
        /// List of Events that will be triggered during the simulation
        /// occurrences or phenomena that happen
        /// Specific Events are one time occurrences with a specific date
        /// </summary>
        public List<IEvent> List { get; } = new List<IEvent>();

        public void Add(IEvent @event)
        {
            if (!Exists(@event))
            {
                List.Add(@event);
            }
        }

        public bool Exists(IEvent @event)
        {
            return List.Contains(@event);
        }

        public void Clear()
        {
            List.Clear();
        }

        /// <summary>
        ///     Returns a list with the ids of all the beliefs
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IId> GetIds()
        {
            return List.Select(x => x.Id);
        }

        public IReadOnlyList<IId> ToVector()
        {
            return GetIds().OrderBy(x => x).ToList();
        }
    }
}