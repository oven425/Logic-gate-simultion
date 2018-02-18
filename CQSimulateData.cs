using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_LogicSimulation
{
    public class CQSimulateData
    {
        public CQGateBaseUI GateData { set; get; }
        public List<CQSimulateData> Nexts { set; get; }
        public int Col { set; get; }
        public CQSimulateData()
        {
            this.Nexts = new List<CQSimulateData>();
        }
    }
}
