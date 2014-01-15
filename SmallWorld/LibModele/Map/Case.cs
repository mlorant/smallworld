using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    /// <summary>
    /// Representation of tile in the map of the game.
    /// Each tile is represented only by its type, which
    /// is the type of the subclass instanciate.
    /// </summary>
    public abstract class Case : ICase
    {
    }
}
