using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class Game : IGame
    {
        private Player[] players;
        private int currentPlayer;
        private int currentRound;
        private Map map;
        private int NB_ROUNDS;
        private int NB_UNITS;
        private int MAP_SIZE;
        private GameCreation builder;

        public Game(int map_size, int nb_rounds, int nb_units)
        {
            throw new System.NotImplementedException();
        }

        void IGame.initPlayers()
        {
            throw new NotImplementedException();
        }

        void IGame.createMap()
        {
            throw new NotImplementedException();
        }

        void IGame.placePlayers()
        {
            throw new NotImplementedException();
        }
    }
}
