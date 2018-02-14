using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace WPF_LogicSimulation
{
    [XmlRoot("AA")]
    public class CQSaveFile
    {
        [XmlArray("Gates")]
        [XmlArrayItem("Gate")]
        public List<CQSaveFile_Gate> Gates { set; get; }
        [XmlArray("Lines")]
        [XmlArrayItem("Line")]
        public List<CQSaveFile_Line> Lines { set; get; }
        public CQSaveFile()
        {
            this.Gates = new List<CQSaveFile_Gate>();
            this.Lines = new List<CQSaveFile_Line>();
        }
    }

    public class CQSaveFile_Gate
    {
        [XmlAttribute("ID")]
        public string ID { set; get; }
        [XmlAttribute("X")]
        public double X { set; get; }
        [XmlAttribute("Y")]
        public double Y { set; get; }
    }

    public class CQSaveFile_LinePoint
    {
        [XmlAttribute("GateID")]
        public string GateID { set; get; }
        [XmlAttribute("Index")]
        public int Index { set; get; }
        [XmlAttribute("Type")]
        public CQPin.Types Type { set; get; }
        public enum EndTypes
        {
            Start,
            End
        }
        [XmlAttribute("EndType")]
        public EndTypes EndType { set; get; }
    }

    public class CQSaveFile_Line
    {
        [XmlIgnore]
        public Line Line { set; get; }
        public CQSaveFile_LinePoint Begin { set; get; }
        public CQSaveFile_LinePoint End { set; get; }
        public CQSaveFile_Line()
        {
            this.Begin = new CQSaveFile_LinePoint();
            this.End = new CQSaveFile_LinePoint();
        }
    }
}
