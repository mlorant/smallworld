using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SmallWorld
{

    /// <summary>
    /// Viking nation
    /// </summary>
    [Serializable()]
    public class NationViking : Nation, INationViking
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public NationViking()
            : base()
        {
        }

        /// <summary>
        /// Constructor for the deserialization
        /// </summary>
        public NationViking(SerializationInfo info, StreamingContext context)
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
            return new Viking(id);
        }

        /// <summary>
        /// Check if the unit given is a member
        /// of this faction.
        /// </summary>
        public override bool hasUnit(IUnit unit)
        {
            return unit is Viking;
        }
    }
}
