namespace WalkeDesigns.FileSplitter.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    ///  Interface for view models that split files into pieces.
    /// </summary>
    public interface ISplitterViewModel : INotifyPropertyChanged, ICancellable
    {
        /// <summary>
        ///  Gets or sets the source file path.
        /// </summary>
        string FilePath { get; set; }

        /// <summary>
        ///  Gets or sets the output file base path.
        /// </summary>
        string OutputPath { get; set; }

        /// <summary>
        ///  Gets or sets the size of the split files.
        /// </summary>
        int ChunkSize { get; set; }

        /// <summary>
        ///  Gets or sets the progress report provider.
        /// </summary>
        IProgress<int> ProgressReporter { get; set; }

        /// <summary>
        ///  Splits the source file into chunks.
        /// </summary>
        void Split();
    }
}
