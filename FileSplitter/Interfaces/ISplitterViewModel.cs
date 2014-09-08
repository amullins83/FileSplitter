using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FileSplitter
{
    public interface ISplitterViewModel : INotifyPropertyChanged
    {
        string FilePath { get; set; }
        string OutputPath { get; set; }
        int ChunkSize { get; set; }
        Task Split();
    }
}
