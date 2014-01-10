using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IGameCreation
    {
        Game createGame(string player1, NationType nation1, string player2, NationType nation2);
    }
}
