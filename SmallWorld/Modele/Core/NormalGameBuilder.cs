using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class NormalGameBuilder : GameCreation, INormalGameBuilder
    {
        public override int NB_ROUNDS
        {
            get { return 30; }
        }
        public override int MAP_SIZE
        {
            get { return 15; }
        }
        public override int NB_UNITS
        {
            get { return 8; }
        }
    }
}
