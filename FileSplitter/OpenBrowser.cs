namespace WalkeDesigns.FileSplitter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Win32;

    /// <summary>
    ///  Browse for a file to open.
    /// </summary>
    public class OpenBrowser
    {
        /// <summary>
        ///  Designator for file browser types.
        /// </summary>
        private enum DialogType
        {
            /// <summary>
            ///  An open file dialog.
            /// </summary>
            DialogOpen,

            /// <summary>
            ///  A save file dialog.
            /// </summary>
            DialogSave
        }

        /// <summary>
        ///  Get a path to a file to save.
        /// </summary>
        /// <returns>The selected path.</returns>
        public static string GetSavePath()
        {
            return GetPath(DialogType.DialogSave);
        }

        /// <summary>
        ///  Get a path to a file to open.
        /// </summary>
        /// <returns>The selected path.</returns>
        public static string GetPath()
        {
            return GetPath(DialogType.DialogOpen);
        }

        /// <summary>
        ///  Get a collection of files to open.
        /// </summary>
        /// <returns>The selected collection of paths.</returns>
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

        /// <summary>
        ///  Get a path to file from a browse dialog.
        /// </summary>
        /// <param name="pathType">The type of the dialog to show.</param>
        /// <returns>The selected path.</returns>
        private static string GetPath(DialogType pathType)
        {
            string path = string.Empty;

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

            if (fileOpened.HasValue && fileOpened.Value)
            {
                path = opener.FileName;
            }

            return path;
        }
    }
}
