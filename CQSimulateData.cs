using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_LogicSimulation
{
    public class CQSimulateData
    {
        public List<CQSimulateData> Nexts { set; get; }
        public CQSimulateData()
        {
            this.Nexts = new List<CQSimulateData>();
        }
    }
}
