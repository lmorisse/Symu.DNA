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
using Symu.DNA.Entities;

#endregion

namespace Symu.DNA.GraphNetworks.OneModeNetworks
{
    /// <summary>
    ///     List of the resources of the meta network:
    ///     Resources are products, materials, or goods that are necessary to perform Tasks and Events
    /// </summary>
    /// <example>database, products, routines, processes, ...</example>
    public class OrganizationNetwork : OneModeNetwork<IOrganization>
    {
       
        //public IOrganization GetEntity(IId organizationId)
        //{
        //    return List.Find(k => k.EntityId.Equals(organizationId));
        //}

        ///// <summary>
        ///// Get the organization from its Id
        ///// </summary>
        ///// <param name="organizationId"></param>
        ///// <returns></returns>
        //public TOrganization GetEntity<TOrganization>(IId organizationId) where TOrganization : IOrganization
        //{
        //    return (TOrganization)GetEntity(organizationId);
        //}

        

        //public bool Exists(IId organizationId)
        //{
        //    return List.Exists(k => k.EntityId.Equals(organizationId));
        //}

        //public void Remove(IOrganization organization)
        //{
        //    if (organization == null)
        //    {
        //        throw new ArgumentNullException(nameof(organization));
        //    }

        //    Remove(organization.EntityId);
        //}
        //public void Remove(IId organizationId)
        //{
        //    List.RemoveAll(x => x.EntityId.Equals(organizationId));
        //}

        ///// <summary>
        /////     Returns a list with the ids of all the organizations
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<IId> GetEntityIds()
        //{
        //    return List.Select(x => x.EntityId);
        //}

        //public IReadOnlyList<IId> ToVector()
        //{
        //    return GetEntityIds().OrderBy(x => x).ToList();
        //}
    }
}