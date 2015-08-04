namespace WalkeDesigns.FileSplitter
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Windows;

    using Interfaces;
    using Utilities;
    using Wpf;

    public class Splitter : Model, ICancellable
    {
        /// <summary>
        ///  Max chunks of 2MB.
        /// </summary>
        private const int MaxChunkSize = 2097152;

        /// <summary>
        ///  Don't bother with chunks less than 4k.
        /// </summary>
        private const int MinChunkSize = 4096;

        /// <summary>
        ///  The bytes to process in each worker loop iteration.
        /// </summary>
        private const int LoopChunkSize = MinChunkSize;

        /// <summary>
        ///  Provider for progress updates.
        /// </summary>
        private IProgressDescriber progress;

        /// <summary>
        ///  The source file path.
        /// </summary>
        private string sourcePath;

        /// <summary>
        ///  The base path for destination files.
        /// </summary>
        private string outputPath;

        /// <summary>
        ///  The size of each chunk.
        /// </summary>
        private int chunkSize;

        /// <summary>
        ///  The worker to perform the split task.
        /// </summary>
        private BackgroundWorker worker;

        /// <summary>
        ///  A value indicating whether the split work is done.
        /// </summary>
        private bool isDone;

        /// <summary>
        ///  The current file being processed.
        /// </summary>
        private string currentFile;

        /// <summary>
        ///  Initializes a new instance of the <see cref="Splitter"/> class.
        /// </summary>
        public Splitter()
        {
            worker = new BackgroundWorker();
            worker.DoWork += SplitInBackground;
            worker.ProgressChanged += SendProgressReport;
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.RunWorkerCompleted += ReportCompleted;
            isDone = false;
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="Splitter"/> class.
        /// </summary>
        /// <param name="sourcePath">The path to the source file.</param>
        /// <param name="outputPath">
        ///  The base path for the destination files.
        /// </param>
        /// <param name="chunkSize">The size of each chunk.</param>
        /// <param name="progress"></param>
        public Splitter(
            string sourcePath,
            string outputPath,
            int chunkSize,
            IProgressDescriber progress) : this()
        {
            this.sourcePath = sourcePath;
            this.outputPath = outputPath;
            this.chunkSize = chunkSize;
            this.progress = progress;
        }

        /// <summary>
        ///  Gets or sets the source file path.
        /// </summary>
        public string SourcePath
        {
            get
            {
                return sourcePath;
            }

            set
            {
                if (sourcePath != value)
                {
                    sourcePath = value;
                    Update("SourcePath");
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
        ///  Gets or sets the size of each chunk.
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
        ///  Gets or sets the progress report provider.
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
        ///  Gets a value indicating whether the split task is done.
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
        ///  Split the file at the given path into chunks of the given size.
        /// </summary>
        public void Split()
        {
            if (File.Exists(sourcePath))
            {
                PerformSplit();
            }
            else
            {
                // There is no source file, so we're done.
                MessageBox.Show(Resources.SplitterStrings.FileMissing);
                progress.Report(100);
            }
        }

        /// <summary>
        ///  Stop the split operation.
        /// </summary>
        public void Cancel()
        {
            worker.CancelAsync();
        }

        /// <summary>
        ///  Perform the split operation on a worker thread.
        /// </summary>
        private void PerformSplit()
        {
            IsDone = false;
            worker.RunWorkerAsync();
        }

        /// <summary>
        ///  Perform the split operation.
        /// </summary>
        /// <param name="sender">
        ///  The worker that raised the event (ignored).
        /// </param>
        /// <param name="e">The event arguments (ignored).</param>
        private void SplitInBackground(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (FileStream fs =
                    new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
                {
                    int realChunkSize =
                            Utilities.Math.Clamp(
                                chunkSize,
                                MinChunkSize,
                                MaxChunkSize);
                    int numChunks = (int)(fs.Length / realChunkSize) + 1;
                    byte[] buffer = new byte[LoopChunkSize];
                    int chunkCounter = 0;
                    int bytesRead = fs.Read(buffer, 0, LoopChunkSize);
                    while (bytesRead > 0 && !worker.CancellationPending)
                    {
                        string chunkPath = String.Format(
                            Resources.SplitterStrings.ChunkPathFormat,
                            outputPath,
                            chunkCounter++);
                        using (FileStream os =
                            new FileStream(chunkPath,
                                FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            int bytesToRead = realChunkSize - bytesRead;
                            do
                            {
                                os.Write(buffer, 0, bytesRead);
                                int nextReadSize =
                                    Utilities.Math.Clamp(
                                        LoopChunkSize,
                                        LoopChunkSize,
                                        bytesToRead);
                                bytesRead = fs.Read(buffer, 0, LoopChunkSize);
                                bytesToRead -= bytesRead;
                            } while (bytesRead > 0 &&
                                     bytesToRead > 0 &&
                                     !worker.CancellationPending);
                        }

                        currentFile = Path.GetFileName(chunkPath);
                        worker.ReportProgress(chunkCounter * 100 / numChunks);
                    }
                }

                currentFile = "Done.";
                worker.ReportProgress(100);
            }
            catch (IOException)
            {
                e.Cancel = true;
                currentFile = "Error Occurred.";
                worker.ReportProgress(100);
            }
        }

        /// <summary>
        ///  Report the task completion percentage.
        /// </summary>
        /// <param name="sender">
        ///  The worker that raised the event (ignored).
        ///  </param>
        /// <param name="e">
        ///  The event arguments, includng completion percentage.
        /// </param>
        private void SendProgressReport(
            object sender,
            ProgressChangedEventArgs e)
        {
            progress.Report(e.ProgressPercentage, currentFile);
        }

        /// <summary>
        ///  Report that the task is complete.
        /// </summary>
        /// <param name="sender">
        ///  The worker that raised the event (ignored).
        /// </param>
        /// <param name="e">The event arguments (ignored).</param>
        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsDone = true;
        }
    }
}
