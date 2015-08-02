namespace WalkeDesigns.FileSplitter
{
    using System;
    using System.Windows;
 
    using Interfaces;

    /// <summary>
    ///  Interaction logic for the <see cref="ProgressReportWindow"/> class.
    /// </summary>
    public partial class ProgressReportWindow : Window, IProgress<int>
    {
        /// <summary>
        ///  The view model for the window.
        /// </summary>
        private ProgressWindowViewModel viewModel;

        /// <summary>
        ///  Initializes a new instance of the
        ///  <see cref="ProgressReportWindow"/> class.
        /// </summary>
        public ProgressReportWindow()
        {
            viewModel = new ProgressWindowViewModel();
            viewModel.TaskCompleted += ViewModel_TaskCompleted;
            DataContext = viewModel;
            InitializeComponent();
        }

        /// <summary>
        ///  Initializes a new instance of the
        ///  <see cref="ProgressReportWindow"/> class.
        /// </summary>
        /// <param name="taskName">The name of the task performed.</param>
        public ProgressReportWindow(string taskName) : this()
        {
            viewModel.TaskName = taskName;
        }

        /// <summary>
        ///  Gets or sets the name of the task performed.
        /// </summary>
        public string TaskName
        {
            get
            {
                return viewModel.TaskName;
            }

            set
            {
                viewModel.TaskName = value;
            }
        }

        /// <summary>
        ///  Gets or sets the target of a cancellation request.
        /// </summary>
        public ICancellable CancelTarget
        {
            get;
            set;
        }

        /// <summary>
        ///  Pass along progress reports to the view model.
        /// </summary>
        /// <param name="percentComplete">The percentage completed.</param>
        public void Report(int percentComplete)
        {
            viewModel.Report(percentComplete);
        }

        /// <summary>
        ///  Close the window upon completion.
        /// </summary>
        /// <param name="sender">
        ///  The view model that raised the event (ignored).
        /// </param>
        /// <param name="e">The event arguments (ignored).</param>
        private void ViewModel_TaskCompleted(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        ///  Cancel the current task.
        /// </summary>
        /// <param name="sender">The button clicked (ignored).</param>
        /// <param name="e">The event arguments (ignored).</param>
        private void Cancel(object sender, EventArgs e)
        {
            CancelTarget.Cancel();
        }
    }
}
