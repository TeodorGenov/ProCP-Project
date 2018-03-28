using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_Traffic_Simulation
{
    interface ICheckpoint
    {
        string Name();
        double CoordinateX();
        double CoordinateY();
    }
}
