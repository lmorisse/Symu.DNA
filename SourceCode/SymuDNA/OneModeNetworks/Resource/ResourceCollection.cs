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

namespace Symu.DNA.OneModeNetworks.Resource
{
    /// <summary>
    ///     List of all the resources 
    ///     Used by ResourceNetwork
    /// </summary>
    public class ResourceCollection
    {
        /// <summary>
        ///     Key => ResourceId
        ///     Values => List of Resources
        /// </summary>
        public List<IResource> List { get; } = new List<IResource>();

        public int Count => List.Count;

        public bool Any()
        {
            return List.Any();
        }

        public void Add(IResource resource)
        {
            if (!Contains(resource))
            {
                List.Add(resource);
            }
        }

        public bool Contains(IResource resource)
        {
            if (resource is null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            return Exists(resource.Id);
        }

        public IResource Get(IId resourceId)
        {
            return List.Find(k => k.Id.Equals(resourceId));
        }

        public bool Exists(IId resourceId)
        {
            return List.Exists(k => k.Id.Equals(resourceId));
        }

        public void Clear()
        {
            List.Clear();
        }
    }
}