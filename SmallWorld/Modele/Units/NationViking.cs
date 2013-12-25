using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SmallWorld
{
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

        public override IUnit fabricUnit(int id)
        {
            return new Viking(id);
        }

        public override bool hasUnit(IUnit unit)
        {
            return unit is Viking;
        }
    }
}
