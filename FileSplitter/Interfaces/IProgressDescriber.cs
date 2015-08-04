namespace WalkeDesigns.FileSplitter.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    ///  An extension of the <c>IProgress</c> interface that includes
    ///  a method for updating the task description.
    /// </summary>
    public interface IProgressDescriber : IProgress<int>
    {
        /// <summary>
        ///  Report progress and include a description of the current task.
        /// </summary>
        /// <param name="percentComplete">
        ///  The percentage of the task completed. Must be between 0 and 100,
        ///  inclusive.
        /// </param>
        /// <param name="description">
        ///  A description of the current sub-task.
        /// </param>
        void Report(int percentComplete, string description);
    }
}
