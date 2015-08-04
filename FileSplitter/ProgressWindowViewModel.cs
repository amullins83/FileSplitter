namespace WalkeDesigns.FileSplitter
{
    using System;
    using System.ComponentModel;

    using Interfaces;
    using Utilities;
    using Wpf;

    public class ProgressWindowViewModel : Model, IProgressDescriber
    {
        /// <summary>
        ///  The completion percentage to display.
        /// </summary>
        private int percentComplete = 0;

        /// <summary>
        ///  The name of the current task.
        /// </summary>
        private string taskName = string.Empty;

        /// <summary>
        ///  Raised when the current task is complete.
        /// </summary>
        public event EventHandler TaskCompleted;

        /// <summary>
        ///  Gets the percentage completion.
        /// </summary>
        public int PercentComplete
        {
            get
            {
                return percentComplete;
            }

            private set
            {
                if (percentComplete != value)
                {
                    percentComplete = value;
                    Update("PercentComplete");
                }
            }
        }

        /// <summary>
        ///  Gets or sets the name of the current task.
        /// </summary>
        public string TaskName
        {
            get
            {
                return taskName;
            }

            set
            {
                if (taskName != value)
                {
                    taskName = value;
                    Update("TaskName");
                }
            }
        }

        /// <summary>
        ///  Update the progress bar value.
        /// </summary>
        /// <param name="value">
        ///  The fraction of the task which is currently completed,
        ///  which should always be between 0 and 100, inclusive.
        /// </param>
        public void Report(int value)
        {
            PercentComplete = value;
            if (percentComplete >= 100)
            {
                TaskCompleted.Raise(this);
            }
        }

        /// <summary>
        ///  Update the progress bar value and the task description.
        /// </summary>
        /// <param name="percentComplete">
        ///  The fraction of the task which is currently completed,
        ///  which should always be between 0 and 100, inclusive.
        /// </param>
        /// <param name="description">
        ///  The current sub-task description.
        /// </param>
        public void Report(int percentComplete, string description)
        {
            Report(percentComplete);
            TaskName = description;
        }
    }
}
