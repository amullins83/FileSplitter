FileSplitter
============

.NET application for splitting large files into smaller chunks

Build
-----

This project requires Visual Studio to build. It was originally developed using VS2013, and it targets .NET framework 4.5.1.

Usage
-----

Once built in the Release configuration, the resulting executable can be run from anywhere.

File Splitting
-------------

1. Enter the path to a file to split or browse for a file on your computer.
2. Enter the base path for the output file chunks or browse to select a base path. File chunks will be saved as `base/path.0`, `base/path.1`, etc.
3. Enter the desired file chunk size in bytes in the next text box.
4. Click "Split".

File Combining
--------------

1. Enter the path to a desired combined output file, or browse for a desired path on your computer.
2. Click "Add Split Paths" to browse for file chunks to combine. You can select multiple files simultaneously.
3. Click "Combine".

TODO
----

- Allow reordering chunks in the "split paths" listbox
- Allow drag-and-drop paths
- Style: default
- Style: themes
- Expose automation interface (COM, Commanding, CLI, etc.)
