using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IGame
    {
        void initPlayers();

        void createMap();

        void placePlayers();
    }
}
