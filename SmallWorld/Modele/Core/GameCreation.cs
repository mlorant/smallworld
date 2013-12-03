using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{

    public enum GameSize { DEMO, SMALL, NORMAL };

    public abstract class GameCreation : IGameCreation
    {
        public int NB_ROUNDS, MAP_SIZE, NB_UNITS;
    }
}
