using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SmallWorld
{
    [Serializable()]
    public class NationDwarf : Nation, INationDwarf
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public NationDwarf()
            : base() 
        { 
        }

        /// <summary>
        /// Constructor for the deserialization
        /// </summary>
        public NationDwarf(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override IUnit fabricUnit(int id)
        {
            return new Dwarf(id);
        }

        public override bool hasUnit(IUnit unit)
        {
            return unit is Dwarf;
        }
    }
}
