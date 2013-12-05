using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{

    public enum GameSize { DEMO, SMALL, NORMAL };

    public abstract class GameCreation : IGameCreation
    {
        public Game createGame()
        {
            Game g = new Game(this.NB_ROUNDS, this.NB_UNITS);
            g.createMap(MAP_SIZE);
            return g;
        }

        public abstract int NB_ROUNDS 
        {
            get; 
        }
        public abstract int MAP_SIZE
        {
            get;
        }
        public abstract int NB_UNITS
        {
            get;
        }
    }
}
