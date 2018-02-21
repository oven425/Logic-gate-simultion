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
        public int PinIndex { set; get; }
        public CQSimulateData()
        {
            this.Nexts = new List<CQSimulateData>();
        }

        public override string ToString()
        {
            return string.Format("PinIndex:{0} {1}"
                , this.PinIndex
                , this.GateData.ToString());
        }
    }
}
