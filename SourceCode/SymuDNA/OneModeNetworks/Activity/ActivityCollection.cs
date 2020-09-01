#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using Symu.DNA.OneModeNetworks.Role;

#endregion

namespace Symu.DNA.OneModeNetworks.Activity
{
    /// <summary>
    ///     List of all the roles 
    ///     Used by roleNetwork
    /// </summary>
    public class ActivityCollection
    {
        /// <summary>
        ///     List of all the roles used in the network
        /// </summary>
        public List<IActivity> List { get; } = new List<IActivity>();

        public void Add(IActivity activity)
        {
            if (!Contains(activity))
            {
                List.Add(activity);
            }
        }

        public bool Contains(IActivity activity)
        {
            if (activity is null)
            {
                throw new ArgumentNullException(nameof(activity));
            }

            return List.Contains(activity);
        }

        public void Clear()
        {
            List.Clear();
        }
        public bool Any()
        {
            return List.Any();
        }
    }
}