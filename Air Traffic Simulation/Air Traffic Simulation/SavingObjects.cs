using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_Traffic_Simulation
{
    [Serializable]
    class SavingObjects
    {
        List<Airplane> airplanes;
        List<Checkpoint> checkpoints;

        public SavingObjects()
        {
            airplanes = null;
            checkpoints = null;
        }
        public SavingObjects(List<Airplane> ap, List<Checkpoint> cp)
        {
            airplanes = ap;
            checkpoints = cp;
        }

        public List<Airplane> getAirplanes
        {
            get { return airplanes; }
        }
        public List<Checkpoint> getCheckpoints
        {
            get { return checkpoints; }
        }

    }
}
