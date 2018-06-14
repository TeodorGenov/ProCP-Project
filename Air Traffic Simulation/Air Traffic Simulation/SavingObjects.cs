using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_Traffic_Simulation
{
    [Serializable]
    public class SavingObjects
    {
        List<Airplane> airplanes;
        List<Checkpoint> checkpoints;
        List<Airplane> groundplanes;


        public SavingObjects()
        {
            airplanes = null;
            checkpoints = null;
        }
        public SavingObjects(List<Airplane> ap, List<Airplane> gp, List<Checkpoint> cp)
        {
            groundplanes = gp;
            airplanes = ap;
            checkpoints = cp;
        }

        /// <summary>
        /// Returns list of airplanes
        /// </summary>
        public List<Airplane> getAirplanes
        {
            get { return airplanes; }
        }

        /// <summary>
        /// Returns list of checkpoints
        /// </summary>
        public List<Checkpoint> getCheckpoints
        {
            get { return checkpoints; }
        }

        /// <summary>
        /// Return list of ground planes
        /// </summary>
        public List<Airplane> getGroundplanes
        {
            get { return groundplanes; }
        }

    }
}
