using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class DemoGame : GameCreation, IDemoGame
    {
        public new const int NB_ROUNDS = 5;
        public new const int MAP_SIZE = 5;
        public new const int NB_UNITS = 4;
    }
}
