using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FileSplitter
{
    public class SplitterViewModel : ISplitterViewModel
    {
        private string filePath;
        public string FilePath {
            get
            {
                return filePath;
            }
            set
            {
                if(filePath != value)
                {
                    filePath = value;
                    OnPropertyChanged("FilePath");
                }
            }
        }

        private string outputPath;
        public string OutputPath
        {
            get
            {
                return outputPath;
            }
            set
            {
                if (outputPath != value)
                {
                    outputPath = value;
                    OnPropertyChanged("OutputPath");
                }
            }
        }

        private int chunkSize;
        public int ChunkSize
        {
            get
            {
                return chunkSize;
            }
            set
            {
                if (chunkSize != value)
                {
                    chunkSize = value;
                    OnPropertyChanged("ChunkSize");
                }
            }
        }

        public async Task Split()
        {
            await Splitter.Split(filePath, outputPath, chunkSize);
        }

        public void SplitSync()
        {
            Splitter.SplitSync(filePath, outputPath, chunkSize);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            var propChanged = PropertyChanged;
            if(propChanged != null)
            {
                propChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
