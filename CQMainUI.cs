using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WPF_LogicSimulation.UIData
{
    public class CQMainUI : INotifyPropertyChanged
    {
        public ObservableCollection<CQSaveFile> SaveFiles { set; get; }
        public CQMainUI()
        {
            this.SaveFiles = new ObservableCollection<CQSaveFile>();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void Update(string name)
        {
            if(this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
