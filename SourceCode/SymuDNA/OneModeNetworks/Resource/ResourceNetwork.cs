#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.Common.Interfaces.Entity;

#endregion

namespace Symu.DNA.OneModeNetworks.Resource
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
        public ResourceCollection Repository { get; } = new ResourceCollection();

        public int Count => Repository.Count;

        public bool Any()
        {
            return Repository.Any();
        }

        public void Clear()
        {
            Repository.Clear();
        }

        /// <summary>
        /// Get the resource from its Id
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public IResource GetResource(IId resourceId)
        {
            return Repository.Get(resourceId);
        }

        /// <summary>
        /// Get the resource from its Id
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public TResource GetResource<TResource >(IId resourceId) where TResource : IResource
        {
            return (TResource)Repository.Get(resourceId);
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

            Repository.Add(resource);
        }

        public bool Exists(IResource resource)
        {
            return Repository.Contains(resource);
        }

        public bool Exists(IId resourceId)
        {
            return Repository.Exists(resourceId);
        }

        public void Remove(IResource resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            RemoveResource(resource.Id);
        }
        public void RemoveResource(IId resourceId)
        {
            Repository.List.RemoveAll(x => x.Id.Equals(resourceId));
        }
    }
}