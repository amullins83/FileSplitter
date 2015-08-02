namespace WalkeDesigns.FileSplitter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    using Interfaces;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        ///  The default split file size.
        /// </summary>
        private const int DefaultChunkSize = 1 << 20;

        /// <summary>
        ///  The splitter view model.
        /// </summary>
        private ISplitterViewModel splitterViewModel;

        /// <summary>
        ///  The combiner view model.
        /// </summary>
        private ICombinerViewModel combinerViewModel;

        /// <summary>
        ///  The progress window for file operations.
        /// </summary>
        private ProgressReportWindow progressWindow;

        /// <summary>
        ///  Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            splitterViewModel = (ISplitterViewModel)Resources["SplitterViewModel"];
            combinerViewModel = (ICombinerViewModel)Resources["CombinerViewModel"];
            splitterViewModel.ChunkSize = DefaultChunkSize;
        }
        
        /// <summary>
        ///  Begin a split file operation.
        /// </summary>
        /// <param name="sender">The button clicked (ignored).</param>
        /// <param name="e">The event arguments (ignored).</param>
        private void Split_Click(object sender, RoutedEventArgs e)
        {
            progressWindow = new ProgressReportWindow();
            splitterViewModel.ProgressReporter = progressWindow;
            progressWindow.CancelTarget = splitterViewModel;
            progressWindow.TaskName = FileSplitter.Resources.TaskNames.Split;
            splitterViewModel.Split();
            progressWindow.Show();
            progressWindow.Owner = this;
        }

        /// <summary>
        ///  Open a file browser to find the source path for a split
        ///  operation.
        /// </summary>
        /// <param name="sender">The button clicked (ignored).</param>
        /// <param name="e">The event arguments (ignored).</param>
        private void BrowseFile_Click(object sender, RoutedEventArgs e)
        {
            splitterViewModel.FilePath = OpenBrowser.GetPath();
        }

        /// <summary>
        ///  Open a file browser for the base destination path of
        ///  a split operation.
        /// </summary>
        /// <param name="sender">The button clicked (ignored).</param>
        /// <param name="e">The event arguments (ignored).</param>
        private void BrowseOutput_Click(object sender, RoutedEventArgs e)
        {
            splitterViewModel.OutputPath = OpenBrowser.GetSavePath();
        }

        /// <summary>
        ///  Open a file browser for the destination of a file combine
        ///  operation.
        /// </summary>
        /// <param name="sender">The button clicked (ignored).</param>
        /// <param name="e">The event arguments (ignored).</param>
        private void BrowseCombinedFile_Click(
            object sender,
            RoutedEventArgs e)
        {
            combinerViewModel.CombinedPath = OpenBrowser.GetSavePath();
        }

        /// <summary>
        ///  Add a path to the list to combine.
        /// </summary>
        /// <param name="sender">The button clicked (ignored).</param>
        /// <param name="e">The event arguments (ignored).</param>
        private void AddSplitPath_Click(object sender, RoutedEventArgs e)
        {
            string[] paths = OpenBrowser.GetPaths();
            if(paths != null)
            {
                foreach (string path in paths)
                {
                    combinerViewModel.SplitPaths.Add(path);
                }
            }
        }

        /// <summary>
        ///  Remove the selected paths from the combine paths list.
        /// </summary>
        /// <param name="sender">The button clicked (ignored).</param>
        /// <param name="e">The event arguments (ignored).</param>
        private void RemoveSplitPaths_Click(object sender, RoutedEventArgs e)
        {
            var removeList = new List<string>();

            foreach(var item in PathsBox.SelectedItems)
            {
                removeList.Add((string)item);
            }

            foreach(var item in removeList)
            {
                combinerViewModel.SplitPaths.Remove(item);
            }
        }

        /// <summary>
        ///  Combine the selected files.
        /// </summary>
        /// <param name="sender">The button that raised the event (ignored).</param>
        /// <param name="e">The event arguments (ignored).</param>
        private void Combine_Click(object sender, RoutedEventArgs e)
        {
            progressWindow = new ProgressReportWindow();
            combinerViewModel.ProgressReporter = progressWindow;
            progressWindow.CancelTarget = combinerViewModel;
            progressWindow.TaskName = FileSplitter.Resources.TaskNames.Combine;
            combinerViewModel.Combine();
            progressWindow.Show();
            progressWindow.Owner = this;
        }
    }
}
