using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{

    /// <summary>
    /// Core class of SmallWorld : directs the
    /// game, and let play each player when it's
    /// their turn.
    /// </summary>
    public interface IGame
    {
        void initPlayer(int i, string nickname, NationType nation);

        void createMap(int mapSize);

        void placePlayers();

        void determinePlayOrder();

        bool isGameFinished();

        void setWinner();
    }
}
