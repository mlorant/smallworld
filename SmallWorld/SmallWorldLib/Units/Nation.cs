using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SmallWorld
{
    public enum NationType { VIKING, DWARF, GALLIC };

    [Serializable()]
    public abstract class Nation : INation, ISerializable
    {
        public abstract IUnit fabricUnit(int id);

        public abstract bool hasUnit(IUnit unit);



        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        public Nation() { }

        // Deserialization constructor.
        public Nation(SerializationInfo info, StreamingContext ctxt)
        {
        }
    }
}
