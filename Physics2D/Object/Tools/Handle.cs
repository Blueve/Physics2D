using Physics2D.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Physics2D.Object.Tools
{
    public class Handle : INotifyPropertyChanged
    {
        private Vector2D _position;

        public Vector2D Position
        {
            get { return _position; }
            set
            {
                if(value != _position)
                {
                    _position = value;
                    SetProperty(ref _position, value);
                }
            }
        }

        protected bool SetProperty<T>(ref T storge, T value, [CallerMemberName]string propertyName = null)
        {
            if (object.Equals(storge, value)) return false;
            storge = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
