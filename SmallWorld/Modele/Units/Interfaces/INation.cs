using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface INation
    {
        IUnit fabricUnit(int id);
    }

    public interface INationDwarf : INation    {}
    public interface INationGallic : INation   {}
    public interface INationViking : INation   {}
}
