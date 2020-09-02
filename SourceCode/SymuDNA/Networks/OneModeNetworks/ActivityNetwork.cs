#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

#endregion

using System.Collections.Generic;
using System.Linq;
using Symu.Common.Interfaces.Entity;

namespace Symu.DNA.Networks.OneModeNetworks
{
    public class ActivityNetwork
    {
        /// <summary>
        ///     Repository of all the activity (Tasks) used in the network
        /// </summary>
        public List<IActivity> List { get; } = new List<IActivity>();

        public void Clear()
        {
            List.Clear();
        }

        public bool Any()
        {
            return List.Any();
        }

        public void Add(IActivity activity)
        {
            if (Exists(activity))
            {
                return;
            }

            List.Add(activity);
        }

        public bool Exists(IActivity activity)
        {
            return List.Contains(activity);
        }

        /// <summary>
        ///     Returns a list with the ids of all the beliefs
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IId> GetIds()
        {
            return List.Select(x => x.Id);
        }
    }
}