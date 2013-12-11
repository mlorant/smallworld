using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface ICase { }

    public interface IDesert : ICase   {}

    public interface IForest : ICase   {}

    public interface IMountain : ICase {}

    public interface IPlain : ICase    {}

    public interface ISea : ICase      {}
}
