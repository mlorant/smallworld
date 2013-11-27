using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class SmallGame : GameCreation, ISmallGame
    {
        private const int NB_ROUNDS = 20;
        private const int MAP_SIZE = 10;
        private const int NB_UNITS = 6;
    }
}
