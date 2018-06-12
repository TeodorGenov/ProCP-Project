using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_Traffic_Simulation
{
    [Serializable]
    public class SavingObjects
    {
        ObservableCollection<Airplane> airplanes;
        ObservableCollection<Checkpoint> checkpoints;
        List<Airplane> groundplanes;

        public SavingObjects()
        {
            airplanes = null;
            checkpoints = null;
        }
        public SavingObjects(ObservableCollection<Airplane> ap, List<Airplane> gp, ObservableCollection<Checkpoint> cp)
        {
            groundplanes = gp;
            airplanes = ap;
            checkpoints = cp;
        }

        public ObservableCollection<Airplane> getAirplanes
        {
            get { return airplanes; }
        }
        public ObservableCollection<Checkpoint> getCheckpoints
        {
            get { return checkpoints; }
        }
        public List<Airplane> getGroundplanes
        {
            get { return groundplanes; }
        }

    }
}
