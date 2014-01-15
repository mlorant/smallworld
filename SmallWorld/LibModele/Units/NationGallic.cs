using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SmallWorld
{

    /// <summary>
    /// Gallic nation
    /// </summary>
    [Serializable()]
    public class NationGallic : Nation, INationGallic
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public NationGallic() : base() 
        { 
        }

        /// <summary>
        /// Constructor for the deserialization
        /// </summary>
        public NationGallic(SerializationInfo info, StreamingContext context)
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
            return new Gallic(id);
        }

        /// <summary>
        /// Check if the unit given is a member
        /// of this faction.
        /// </summary>
        public override bool hasUnit(IUnit unit)
        {
            return unit is Gallic;
        }
    }
}
