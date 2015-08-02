namespace WalkeDesigns.Wpf
{
    using System;
    using System.ComponentModel;
    using System.Windows.Threading;

    using Utilities;

    /// <summary>
    ///  Base class for model classes.
    /// </summary>
    public class Model : INotifyPropertyChanged
    {
        /// <summary>
        ///  The dispatcher that created the model.
        /// </summary>
        private Dispatcher mainDispatcher;

        /// <summary>
        ///  Initializes a new instance of the <see cref="Model"/> class.
        /// </summary>
        public Model()
        {
            mainDispatcher = Dispatcher.CurrentDispatcher;
        }

        /// <summary>
        ///  Raised when an observable property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///  Gets the dispatcher for the thread that created the model.
        /// </summary>
        protected Dispatcher MainDispatcher
        {
            get { return mainDispatcher; }
        }

        /// <summary>
        ///  Raise the <c>ProyertyChanged</c> event.
        /// </summary>
        /// <param name="name"><The name of the modified property./param>
        protected virtual void Update(string name)
        {
            mainDispatcher.BeginInvoke(
                new Action(() => RaisePropertyChanged(name)));
        }

        /// <summary>
        ///  Raise the <c>PropertyChanged</c> event immediately.
        /// </summary>
        /// <param name="name">The name of the modified property.</param>
        private void RaisePropertyChanged(string name)
        {
            var propChanged = PropertyChanged;
            if (propChanged != null)
            {
                propChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
