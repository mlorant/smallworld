﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public class NationViking : Nation, INationViking
    {
        public IViking fabricUnit()
        {
            return new Viking();
        }
    }
}
