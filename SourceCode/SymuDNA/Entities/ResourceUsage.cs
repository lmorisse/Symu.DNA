#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;

#endregion

namespace Symu.DNA.Entities
{
    /// <summary>
    ///     Default implementation of IResourceUsage
    /// </summary>
    public class ResourceUsage : IResourceUsage
    {
        public ResourceUsage(byte usage)
        {
            Usage = usage;
        }

        public byte Usage { get; }

        #region IResourceUsage Members

        public bool Equals(IResourceUsage resourceUsage)
        {
            if (resourceUsage == null)
            {
                throw new ArgumentNullException(nameof(resourceUsage));
            }

            return resourceUsage is ResourceUsage usage &&
                   Usage == usage.Usage;
        }

        #endregion
    }
}