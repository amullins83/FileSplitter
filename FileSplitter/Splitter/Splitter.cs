using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileSplitter
{
    public class Splitter
    {
        public static Task
        Split(
            string filePath,
            string outputPath,
            int chunkSize)
        {
            return new Task(() => SplitSync(filePath, outputPath, chunkSize));
        }

        public static void SplitSync(
            string filePath,
            string outputPath,
            int chunkSize)
        {
            if (File.Exists(filePath))
            {
                using (FileStream fs =
                    new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader reader =
                        new BinaryReader(fs))
                    {
                        byte[] chunk = reader.ReadBytes(chunkSize);
                        int chunkCounter = 0;
                        while (chunk.Length > 0)
                        {
                            string chunkPath = String.Format(
                                Resources.SplitterStrings.ChunkPathFormat,
                                outputPath,
                                chunkCounter++);
                            using (FileStream os =
                                new FileStream(chunkPath,
                                    FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                os.Write(chunk, 0, chunk.Length);
                            }
                            chunk = reader.ReadBytes(chunkSize);
                        }
                    }
                }
            }
        }
    }
}
