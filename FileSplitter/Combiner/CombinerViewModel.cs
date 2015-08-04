namespace WalkeDesigns.FileSplitter
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;

    using Interfaces;
    using Wpf;

    public class CombinerViewModel : Model, ICombinerViewModel
    {
        /// <summary>
        ///  The path to the destination file.
        /// </summary>
        private string destinationPath;

        /// <summary>
        ///  The combiner to perform the task.
        /// </summary>
        private Combiner combiner;

        /// <summary>
        ///  The provider for progress updates.
        /// </summary>
        private IProgressDescriber progressReporter;

        /// <summary>
        ///  The collection of split file paths.
        /// </summary>
        private ObservableCollection<string> splitPaths =
            new ObservableCollection<string>();

        /// <summary>
        ///  Gets or sets the path to the destination for the combined file.
        /// </summary>
        public string CombinedPath
        {
            get
            {
                return destinationPath;
            }

            set
            {
                if (destinationPath != value)
                {
                    destinationPath = value;
                    Update("CombinedPath");
                }
            }
        }

        /// <summary>
        ///  Gets the collection of split file paths.
        /// </summary>
        public ObservableCollection<string> SplitPaths
        {
            get { return splitPaths; }
        }

        /// <summary>
        ///  Gets or sets the progress reporter.
        /// </summary>
        public IProgressDescriber ProgressReporter
        {
            get
            {
                return progressReporter;
            }

            set
            {
                if (progressReporter != value)
                {
                    progressReporter = value;
                    Update("ProgressReporter");
                }
            }
        }

        /// <summary>
        ///  Combine the split files and store the output to the
        ///  destination path.
        /// </summary>
        public void Combine()
        {
            combiner = new Combiner(
                splitPaths,
                destinationPath,
                progressReporter);
            combiner.Combine();
        }

        /// <summary>
        ///  Cancel the current task.
        /// </summary>
        public void Cancel()
        {
            if (combiner != null && !combiner.IsDone)
            {
                combiner.Cancel();
            }
        }
    }
}