using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SmallWorld
{

    /// <summary>
    /// Nation possible in the game
    /// </summary>
    public enum NationType { VIKING, DWARF, GALLIC };

    /// <summary>
    /// Base nation class
    /// </summary>
    [Serializable()]
    public abstract class Nation : INation, ISerializable
    {

        /// <summary>
        /// Create a unit with the given unique id
        /// for the nation
        /// </summary>
        /// <param name="id">Unique identifier</param>
        /// <returns></returns>
        public abstract IUnit fabricUnit(int id);

        /// <summary>
        /// Check if the unit given is a member
        /// of this faction.
        /// </summary>
        public abstract bool hasUnit(IUnit unit);


        /// <summary>
        /// Base class for the serialization process
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Nation() { }

        // Deserialization constructor.
        public Nation(SerializationInfo info, StreamingContext ctxt)
        {
        }
    }
}
