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
        private ISplitterViewModel splitterViewModel;
        private ICombinerViewModel combinerViewModel;

        public MainWindow()
        {
            InitializeComponent();
            splitterViewModel = (ISplitterViewModel)Resources["SplitterViewModel"];
            combinerViewModel = (ICombinerViewModel)Resources["CombinerViewModel"];
        }
        
        private void split_Click(object sender, RoutedEventArgs e)
        {
            var vm = (SplitterViewModel)splitterViewModel;
            var split = new Action(vm.SplitSync);
            split.BeginInvoke(null, null);
        }

        private void browseFile_Click(object sender, RoutedEventArgs e)
        {
            splitterViewModel.FilePath = OpenBrowser.GetPath();
        }

        private void browseOutput_Click(object sender, RoutedEventArgs e)
        {
            splitterViewModel.OutputPath = OpenBrowser.GetSavePath();
        }

        private void browseCombinedFile_Click(object sender, RoutedEventArgs e)
        {
            combinerViewModel.CombinedPath = OpenBrowser.GetSavePath();
        }

        private void addSplitPath_Click(object sender, RoutedEventArgs e)
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

        private void removeSplitPaths_Click(object sender, RoutedEventArgs e)
        {
            var removeList = new List<string>();

            foreach(var item in pathsBox.SelectedItems)
            {
                removeList.Add((string)item);
            }

            foreach(var item in removeList)
            {
                combinerViewModel.SplitPaths.Remove(item);
            }
        }

        private void combine_Click(object sender, RoutedEventArgs e)
        {
            var vm = (CombinerViewModel)combinerViewModel;
            var combine = new Action(vm.CombineSync);
            combine.BeginInvoke(null, null);
        }
    }
}
