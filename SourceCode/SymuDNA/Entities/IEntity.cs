#region Licence

// Description: SymuBiz - Symu
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using System.Data.SqlTypes;
using Symu.Common.Interfaces.Agent;
using Symu.DNA.GraphNetworks;

namespace Symu.DNA.Entities
{
    /// <summary>
    ///     Base interface for entities
    ///     Entities are used in One Mode Networks
    /// </summary>
    public interface IEntity: ICloneable
    {
        IAgentId EntityId
        {
            get;
            set;
        }
        /// <summary>
        /// Use to set the metaNetwork embedded in the Entity
        /// </summary>
        /// <param name="metaNetwork"></param>
        void Set(MetaNetwork metaNetwork);
        /// <summary>
        /// Triggered when the entity is removed
        /// At least must clean the MetaNetwork
        /// </summary>
        void Remove();
    }
}