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
        private enum DialogType
        {
            DialogOpen,
            DialogSave
        }

        private static string getPath(DialogType pathType)
        {
            string path = "";

            FileDialog opener;

            switch (pathType)
            {
                case DialogType.DialogOpen:
                    opener = new OpenFileDialog();
                    break;
                case DialogType.DialogSave:
                    opener = new SaveFileDialog();
                    break;
                default:
                    return path;
            }

            bool? fileOpened = opener.ShowDialog(App.Current.MainWindow);

            if(fileOpened.HasValue && fileOpened.Value)
            {
                path = opener.FileName;
            }

            return path;
        }

        public static string GetSavePath()
        {
            return getPath(DialogType.DialogSave);
        }

        public static string GetPath()
        {
            return getPath(DialogType.DialogOpen);
        }

        public static string[] GetPaths()
        {
            string[] paths = null;

            OpenFileDialog opener = new OpenFileDialog();
            opener.Multiselect = true;

            bool? fileOpened = opener.ShowDialog(App.Current.MainWindow);

            if(fileOpened.HasValue && fileOpened.Value)
            {
                paths = opener.FileNames;
            }

            return paths;
        }
    }
}
