namespace WalkeDesigns.FileSplitter.Interfaces
{
    /// <summary>
    ///  An interface to expose a cancel method.
    /// </summary>
    public interface ICancellable 
    {
        /// <summary>
        ///  Cancel the current operation.
        /// </summary>
        void Cancel();
    }
}
