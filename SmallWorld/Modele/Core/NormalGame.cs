using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class NormalGame : GameCreation, INormalGame
    {
        private const int NB_ROUNDS = 30;
        private const int MAP_SIZE = 15;
        private const int NB_UNITS = 8;
    }
}
