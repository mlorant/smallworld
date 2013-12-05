using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class DemoGameBuilder : GameCreation, IDemoGameBuilder
    {
        public override int NB_ROUNDS
        {
            get { return 5; }
        }
        public override int MAP_SIZE
        {
            get { return 5; }
        }
        public override int NB_UNITS
        {
            get { return 4; }
        }
    }
}
