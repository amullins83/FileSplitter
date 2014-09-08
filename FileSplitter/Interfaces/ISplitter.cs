using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSplitter
{
    public interface ISplitter
    {
        Task Split(string filePath, string outputPath, int chunkSize);
    }
}
