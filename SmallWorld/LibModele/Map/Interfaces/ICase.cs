using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{

    /// <summary>
    /// Base interface that represent a tile
    /// </summary>
    public interface ICase { }

    /// <summary>
    /// Desert tile, full of sands and storm
    /// </summary>
    public interface IDesert : ICase   {}

    /// <summary>
    /// Forest tile, with big trees and dwarfs
    /// </summary>
    public interface IForest : ICase   {}

    /// <summary>
    /// Mountain tile, with high hills
    /// </summary>
    public interface IMountain : ICase {}

    /// <summary>
    /// Plain tile
    /// </summary>
    public interface IPlain : ICase    {}

    /// <summary>
    /// Sea tile, unreachable for many nations
    /// </summary>
    public interface ISea : ICase      {}
}
