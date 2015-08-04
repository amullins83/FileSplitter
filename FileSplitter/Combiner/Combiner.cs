namespace WalkeDesigns.FileSplitter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    using Interfaces;
    using Wpf;

    public class Combiner : Model, ICancellable
    {
        /// <summary>
        ///  The maximum number of bytes to read from a file at a time.
        /// </summary>
        private const int LoopBytes = 4096;

        /// <summary>
        ///  The collection of paths to combine.
        /// </summary>
        private IEnumerable<string> splitPaths;

        /// <summary>
        ///  The path to the combined output file.
        /// </summary>
        private string destinationPath;

        /// <summary>
        ///  The name of the current file processed.
        /// </summary>
        private string currentFile;

        /// <summary>
        ///  The provider for progress updates.
        /// </summary>
        private IProgressDescriber progressReporter;

        /// <summary>
        ///  A value indicating whether the task is done.
        /// </summary>
        private bool isDone;

        /// <summary>
        ///  The worker for performing the combination task.
        /// </summary>
        private BackgroundWorker worker;

        /// <summary>
        ///  Initializes a new instance of the
        ///  <see cref="Combiner"/> class.
        /// </summary>
        public Combiner()
        {
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += CombineInBackground;
            worker.ProgressChanged += ReportProgress;
            worker.RunWorkerCompleted += ReportCompleted;
        }

        /// <summary>
        ///  Initializes a new instance of the
        ///  <see cref="Combiner"/> class.
        /// </summary>
        /// <param name="splitPaths">
        ///  A collection of split file paths.
        /// </param>
        /// <param name="destinationPath">
        ///  The path to the combined file destination.
        /// </param>
        /// <param name="progressReporter">
        ///  The progress report provider.
        /// </param>
        public Combiner(
            IEnumerable<string> splitPaths,
            string destinationPath,
            IProgressDescriber progressReporter) : this()
        {
            this.splitPaths = splitPaths;
            this.destinationPath = destinationPath;
            this.progressReporter = progressReporter;
        }

        /// <summary>
        ///  Gets or sets the collection of split file paths.
        /// </summary>
        public IEnumerable<string> SplitPaths
        {
            get
            {
                return splitPaths;
            }

            set
            {
                if (splitPaths != value)
                {
                    splitPaths = value;
                    Update("SplitPaths");
                }
            }
        }

        /// <summary>
        ///  Gets or sets the path to the combined file
        ///  destination.
        /// </summary>
        public string DestinationPath
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
                    Update("DestinationPath");
                }
            }
        }

        /// <summary>
        ///  Gets or sets the provider for progress reports.
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
        ///  Gets a value indicating whether the task is done.
        /// </summary>
        public bool IsDone
        {
            get
            {
                return isDone;
            }

            private set
            {
                if (isDone != value)
                {
                    isDone = value;
                    Update("IsDone");
                }
            }
        }

        /// <summary>
        ///  Combine the split files.
        /// </summary>
        public void Combine()
        {
            if (splitPaths != null)
            {
                IsDone = false;
                worker.RunWorkerAsync();
            }
        }

        /// <summary>
        ///  Cancel the combine operation.
        /// </summary>
        public void Cancel()
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }
        }

        /// <summary>
        ///  Report that the task is completed.
        /// </summary>
        /// <param name="sender">
        ///  The worker that raised the event (ignored).
        /// </param>
        /// <param name="e">
        ///  The event arguments (ignored).
        /// </param>
        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsDone = true;
            progressReporter.Report(100);
        }

        /// <summary>
        ///  Report progress changes to the reporter.
        /// </summary>
        /// <param name="sender">
        ///  The worker that raised the event (ignored).
        /// </param>
        /// <param name="e">
        ///  The event arguments, including the completion percentage.
        /// </param>
        private void ReportProgress(object sender, ProgressChangedEventArgs e)
        {
            progressReporter.Report(
                e.ProgressPercentage,
                currentFile);
        }

        /// <summary>
        ///  Perform the combination task.
        /// </summary>
        /// <param name="sender">
        ///  The worker that raised the event (ignored).
        /// </param>
        /// <param name="e">
        ///  The event arguments, which may be used to set the cancel state.
        /// </param>
        private void CombineInBackground(object sender, DoWorkEventArgs e)
        {
            var buffer = new byte[LoopBytes];
            try
            {
                using (var writeStream = new FileStream(destinationPath, FileMode.Create))
                {
                    int fileCounter = 0;
                    int numFiles = splitPaths.Count();
                    int bytesRead = 0;
                    foreach (var path in splitPaths)
                    {
                        using (var readStream = new FileStream(path, FileMode.Open))
                        {
                            do
                            {
                                bytesRead = readStream.Read(buffer, 0, LoopBytes);
                                writeStream.Write(buffer, 0, bytesRead);
                            } while (
                                bytesRead > 0 &&
                                writeStream.CanWrite &&
                                !worker.CancellationPending);
                        }

                        if (worker.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }

                        currentFile = Path.GetFileName(path);
                        worker.ReportProgress(++fileCounter * 100 / numFiles);
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, Resources.CombinerStrings.FileError);
                e.Cancel = true;
            }
        }
    }
}