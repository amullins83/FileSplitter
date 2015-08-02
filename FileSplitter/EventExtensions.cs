namespace WalkeDesigns.Utilities
{ 
    using System;

    /// <summary>
    ///  Utilitdes for events.
    /// </summary>
    public static class EventExtensions
    {
        /// <summary>
        ///  Raise the event if not null.
        /// </summary>
        /// <param name="e">The event to raise.</param>
        /// <param name="sender">The sender of the event.</param>
        public static void Raise(this EventHandler e, object sender)
        {
            var handler = e;
            if (handler != null)
            {
                handler(sender, EventArgs.Empty);
            }
        }

        /// <summary>
        ///  Raise the event if not null with the given arguments.
        /// </summary>
        /// <typeparam name="TEventArgs">
        ///  The type of event arguments passed.
        /// </typeparam>
        /// <param name="e">The event to raise.</param>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        public static void Raise<TEventArgs>(
            this EventHandler<TEventArgs> e,
            object sender,
            TEventArgs args) where TEventArgs : EventArgs
        {
            var handler = e;
            if (handler != null)
            {
                handler(sender, args);
            }
        }
    }
}
