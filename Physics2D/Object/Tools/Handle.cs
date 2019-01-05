namespace Physics2D.Object.Tools
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Physics2D.Common;

    public class Handle : INotifyPropertyChanged
    {
        /// <summary>
        /// The position.
        /// </summary>
        private Vector2D position;

        /// <summary>
        /// The position.
        /// </summary>
        public Vector2D Position
        {
            get { return this.position; }
            set { this.SetProperty(ref this.position, value); }
        }

        public Handle(Vector2D position)
        {
            this.position = position;
        }

        /// <summary>
        /// Set property's value.
        /// Trigger property changed event when value changed.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="storge">Current value.</param>
        /// <param name="value">Aim value.</param>
        /// <param name="propertyName">The property's name.</param>
        /// <returns>True if property value updated.</returns>
        protected bool SetProperty<T>(ref T storge, T value, [CallerMemberName]string propertyName = null)
        {
            if (object.Equals(storge, value))
            {
                return false;
            }

            storge = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// The event of property changed.
        /// </summary>
        /// <param name="propertyName">The property's name.</param>
        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Release all events.
        /// </summary>
        public void Release()
        {
            var delegates = this.PropertyChanged.GetInvocationList();
            foreach (var d in delegates)
            {
                this.PropertyChanged -= d as PropertyChangedEventHandler;
            }
        }

        /// <summary>
        /// The property changed event.
        /// Registed delegation will fired on property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
