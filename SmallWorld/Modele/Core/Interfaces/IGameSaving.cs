using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallWorld
{
    interface IGameSaving
    {

        void saveCurrentGame(String destination);

        void restoreGame(String saveFile);

    }
}
