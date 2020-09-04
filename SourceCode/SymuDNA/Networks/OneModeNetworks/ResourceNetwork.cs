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
    ///     All resources in the network
    /// </summary>
    /// <example>database, products, routines, processes, ...</example>
    public class ResourceNetwork
    {
        /// <summary>
        ///     Repository of all the resources used during the simulation
        /// </summary>
        public List<IResource> List { get; } = new List<IResource>();

        public int Count => List.Count;

        public bool Any()
        {
            return List.Any();
        }

        public void Clear()
        {
            List.Clear();
        }


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

        /// <summary>
        ///     Add a resource to the repository
        /// </summary>
        public void Add(IResource resource)
        {
            if (Exists(resource))
            {
                return;
            }

            List.Add(resource);
        }

        public bool Exists(IResource resource)
        {
            return List.Contains(resource);
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