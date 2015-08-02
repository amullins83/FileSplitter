namespace WalkeDesigns.FileSplitter.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    /// <summary>
    ///  Interface for view models that combine files.
    /// </summary>
    public interface ICombinerViewModel : INotifyPropertyChanged, ICancellable
    {
        /// <summary>
        ///  Gets or sets the destination path.
        /// </summary>
        string CombinedPath { get; set; }

        /// <summary>
        ///  Gets a collection of split source files.
        /// </summary>
        ObservableCollection<string> SplitPaths { get; }

        /// <summary>
        ///  Gets or sets the progress report provider.
        /// </summary>
        IProgress<int> ProgressReporter { get; set; }

        /// <summary>
        ///  Combine the selected files.
        /// </summary>
        void Combine();
    }
}
