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

namespace FileSplitter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ISplitterViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = (ISplitterViewModel)Resources["ViewModel"];
        }
        
        private void split_Click(object sender, RoutedEventArgs e)
        {
            var syncSplitter = viewModel as SplitterViewModel;
            if (syncSplitter != null)
            {
                syncSplitter.SplitSync();
            }
        }

        private void browseFile_Click(object sender, RoutedEventArgs e)
        {
            viewModel.FilePath = OpenBrowser.GetPath();
        }

        private void browseOutput_Click(object sender, RoutedEventArgs e)
        {
            viewModel.OutputPath = OpenBrowser.GetPath();
        }
    }
}
