using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class SmallGame : GameCreation, ISmallGame
    {
        public new const int NB_ROUNDS = 20;
        public new const int MAP_SIZE = 10;
        public new const int NB_UNITS = 6;
    }
}
