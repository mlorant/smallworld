using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SmallWorld
{
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

        public override IUnit fabricUnit(int id)
        {
            return new Gallic(id);
        }

        public override bool hasUnit(IUnit unit)
        {
            return unit is Gallic;
        }
    }
}
