﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.Gate
{
    partial class Setup
    {
        private void Initialize()
        {
            Version = ProjectFile<Setup>.CURRENT_VERSION;
            Server = new Server();
        }
    }
}
