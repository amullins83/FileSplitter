using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FileSplitter
{
    public interface ICombinerViewModel : INotifyPropertyChanged
    {
        string CombinedPath { get; set; }
        ObservableCollection<string> SplitPaths { get; }
        Task Combine();
    }
}
