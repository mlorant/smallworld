using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class SmallGameBuilder : GameCreation, ISmallGameBuiler
    {
        public override int NB_ROUNDS
        {
            get { return 20; }
        }
        public override int MAP_SIZE
        {
            get { return 10; }
        }
        public override int NB_UNITS
        {
            get { return 6; }
        }
    }
}
