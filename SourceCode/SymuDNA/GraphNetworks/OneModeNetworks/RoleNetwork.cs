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
using Symu.DNA.Entities;

namespace Symu.DNA.GraphNetworks.OneModeNetworks
{
    public class RoleNetwork : OneModeNetwork<IRole>
    {
       
        ///// <summary>
        /////     Returns a list with the ids of all the roles
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