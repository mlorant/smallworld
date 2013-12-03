using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class NormalGame : GameCreation, INormalGame
    {
        public new const int NB_ROUNDS = 30;
        public new const int MAP_SIZE = 15;
        public new const int NB_UNITS = 8;
    }
}
