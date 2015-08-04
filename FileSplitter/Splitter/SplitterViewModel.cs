namespace WalkeDesigns.FileSplitter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;

    using Interfaces;
    using Wpf;

    public class SplitterViewModel : Model, ISplitterViewModel
    {
        /// <summary>
        ///  The file splitter to perform the split task.
        /// </summary>
        private Splitter splitter;

        /// <summary>
        ///  The size of the split files.
        /// </summary>
        private int chunkSize;

        /// <summary>
        ///  The source file path.
        /// </summary>
        private string filePath;

        /// <summary>
        ///  The destination file base path.
        /// </summary>
        private string outputPath;

        /// <summary>
        ///  The progress report provider.
        /// </summary>
        private IProgressDescriber progress;
        
        /// <summary>
        ///  Gets or sets the source file path.
        /// </summary>
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
                    Update("FilePath");
                }
            }
        }

        /// <summary>
        ///  Gets or sets the output file base path.
        /// </summary>
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
                    Update("OutputPath");
                }
            }
        }

        /// <summary>
        ///  Gets or sets the size of the split files.
        /// </summary>
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
                    Update("ChunkSize");
                }
            }
        }

        /// <summary>
        ///  Gets or sets the progress provider.
        /// </summary>
        public IProgressDescriber ProgressReporter
        {
            get
            {
                return progress;
            }

            set
            {
                if (progress != value)
                {
                    progress = value;
                    Update("ProgressReporter");
                }
            }
        }

        /// <summary>
        ///  Perform the split operation, directing progress updates to the
        ///  given progress reporter.
        /// </summary>
        public void Split()
        {
            splitter =
                new Splitter(filePath, outputPath, chunkSize, progress);
            splitter.Split();
        }

        /// <summary>
        ///  Cancel the split operation.
        /// </summary>
        public void Cancel()
        {
            if (splitter != null && !splitter.IsDone)
            {
                splitter.Cancel();
            }
        }
    }
}
