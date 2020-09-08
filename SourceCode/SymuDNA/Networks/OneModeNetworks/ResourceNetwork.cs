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
using Symu.Common.Interfaces.Entity;

#endregion

namespace Symu.DNA.Networks.OneModeNetworks
{
    /// <summary>
    ///     List of the resources of the meta network:
    ///     Resources are products, materials, or goods that are necessary to perform Tasks and Events
    /// </summary>
    /// <example>database, products, routines, processes, ...</example>
    public class ResourceNetwork : OneModeNetwork<IResource>
    {
       
        public IResource Get(IId resourceId)
        {
            return List.Find(k => k.Id.Equals(resourceId));
        }

        /// <summary>
        /// Get the resource from its Id
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public TResource Get<TResource >(IId resourceId) where TResource : IResource
        {
            return (TResource)Get(resourceId);
        }

        

        public bool Exists(IId resourceId)
        {
            return List.Exists(k => k.Id.Equals(resourceId));
        }

        public void Remove(IResource resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            Remove(resource.Id);
        }
        public void Remove(IId resourceId)
        {
            List.RemoveAll(x => x.Id.Equals(resourceId));
        }

        /// <summary>
        ///     Returns a list with the ids of all the Resource
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