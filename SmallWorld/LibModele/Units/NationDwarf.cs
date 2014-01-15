using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SmallWorld
{

    /// <summary>
    /// Dwarfs nation
    /// </summary>
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

        /// <summary>
        /// Create a unit with the given unique id
        /// for the nation
        /// </summary>
        /// <param name="id">Unique identifier</param>
        /// <returns></returns>
        public override IUnit fabricUnit(int id)
        {
            return new Dwarf(id);
        }

        /// <summary>
        /// Check if the unit given is a member
        /// of this faction.
        /// </summary>
        public override bool hasUnit(IUnit unit)
        {
            return unit is Dwarf;
        }
    }
}
