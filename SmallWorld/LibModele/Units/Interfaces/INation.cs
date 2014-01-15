using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{

    /// <summary>
    /// Base interface for nation.
    /// </summary>
    public interface INation
    {
        /// <summary>
        /// Create a unit with the given unique id
        /// for the nation
        /// </summary>
        /// <param name="id">Unique identifier</param>
        /// <returns></returns>
        IUnit fabricUnit(int id);

        /// <summary>
        /// Check if the unit given is a member
        /// of this faction.
        /// </summary>
        bool hasUnit(IUnit unit);
    }


    /// <summary>
    /// Dwarfs nation interface
    /// </summary>
    public interface INationDwarf : INation    {}

    /// <summary>
    /// Gallic nation interface
    /// </summary>
    public interface INationGallic : INation   {}

    /// <summary>
    /// Viking nation interface
    /// </summary>
    public interface INationViking : INation   {}
}
