#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using System.Data.SqlTypes;
using Symu.Common.Interfaces.Agent;

namespace Symu.DNA.Entities
{
    /// <summary>
    ///     An actor is an individual decision makers
    /// </summary>
    /// <remarks>Also named agent in social network analysis, but agent is used for multi agents system</remarks>
    public interface IActor: IEntity
    {
    }
}