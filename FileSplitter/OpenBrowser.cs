using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace FileSplitter
{
    public class OpenBrowser
    {
        public static string GetPath()
        {
            string path = "";

            var opener = new OpenFileDialog();

            bool? fileOpened = opener.ShowDialog(App.Current.MainWindow);

            if(fileOpened.HasValue && fileOpened.Value)
            {
                path = opener.FileName;
            }

            return path;
        }
    }
}
