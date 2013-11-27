using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public abstract class Case : ICase
    {
        private const int SIZE = 50;

        public IUnit getBestDefensiveUnit()
        {
            throw new NotImplementedException();
        }
    }
}
