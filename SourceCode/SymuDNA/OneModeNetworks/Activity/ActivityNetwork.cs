#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

#endregion

using Symu.DNA.OneModeNetworks.Role;

namespace Symu.DNA.OneModeNetworks.Activity
{
    public class ActivityNetwork
    {
        /// <summary>
        ///     Repository of all the activity (Tasks) used in the network
        /// </summary>
        public ActivityCollection Repository { get; } = new ActivityCollection();

        public void Clear()
        {
            Repository.Clear();
        }

        public bool Any()
        {
            return Repository.Any();
        }

        public void Add(IActivity activity)
        {
            if (Exists(activity))
            {
                return;
            }

            Repository.Add(activity);
        }

        public bool Exists(IActivity activity)
        {
            return Repository.Contains(activity);
        }
    }
}