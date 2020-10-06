#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces;
using Symu.DNA.GraphNetworks;

#endregion

namespace Symu.DNA.Entities
{
    /// <summary>
    ///     An Event is occurrences or phenomena that happen
    ///     Default implementation of IEvent
    /// </summary>
    public class EventEntity : Entity<EventEntity>, IEvent
    {
        public const byte Class = ClassIdCollection.Event;
        public static IClassId ClassId => new ClassId(Class);
        public EventEntity() 
        {
        }
        public EventEntity(MetaNetwork metaNetwork) : base(metaNetwork, metaNetwork?.Event, Class)
        {
        }
        public EventEntity(MetaNetwork metaNetwork, string name) : base(metaNetwork, metaNetwork?.Event, Class, name)
        {
        }
    }
}