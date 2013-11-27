using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IMap
    {
        ICase getCase(int i);

        int getSize();
    }
}
