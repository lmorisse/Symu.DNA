#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using Symu.DNA.OneModeNetworks.Resource;

namespace SymuDNATests.Classes
{
    /// <summary>
    /// Default implementation of IResourceUsage
    /// </summary>
    public class TestResourceUsage : IResourceUsage
    {
        public TestResourceUsage(byte usage)
        {
            Usage = usage;
        }
        public byte Usage { get; }
        public bool Equals(IResourceUsage resourceUsage)
        {
            if (resourceUsage == null)
            {
                throw new ArgumentNullException(nameof(resourceUsage));
            }

            return resourceUsage is TestResourceUsage usage &&
                   Usage == usage.Usage;
        }
    }
}