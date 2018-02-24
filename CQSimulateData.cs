using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_LogicSimulation
{
    public class CQSimlateEndData
    {
        public CQSimlateEndData()
        {

        }
        public CQSimlateEndData(int src, int dst)
        {
            this.Source = src;
            this.Destination = dst;
        }
        public int Source { set; get; }
        public int Destination { set; get; }
        public override string ToString()
        {
            return string.Format("Src:{0} Dst:{1}"
                , this.Source
                , this.Destination);
        }
    }
    public class CQSimulateData
    {
        public CQGateBaseUI GateData { set; get; }
        //public List<CQSimulateData> Nexts { set; get; }
        public int Col { set; get; }
        //public int PinIndex { set; get; }
        public Dictionary<CQSimulateData, CQSimlateEndData> Nexts{ set; get; }
        public CQSimulateData()
        {
            this.Nexts = new Dictionary<CQSimulateData, CQSimlateEndData>();
            //this.Nexts = new List<CQSimulateData>();
        }

        public override string ToString()
        {
            return string.Format("PinIndex:{0} {1}"
                , 0
                , this.GateData.ToString());
        }
    }
}
