using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IPlayer
    {

        String Nickname { get; }

        List<IUnit> Units { get; }

        INation Nation { get; }

    }
}
