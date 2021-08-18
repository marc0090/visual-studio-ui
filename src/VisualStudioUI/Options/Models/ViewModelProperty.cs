//
// ViewModelProperty.cs
//
// Author:
//       Greg Munn <greg.munn@xamarin.com>
//
// Copyright (c) 2015 Xamarin Inc
//

using System;
using System.ComponentModel;

namespace Microsoft.VisualStudioUI.Options.Models
{
    /// <summary>
    /// Simple view model property that sends change notifications
    /// </summary>
    public abstract class ViewModelProperty : INotifyPropertyChanged
    {
        private object _propertyValue;

        private event PropertyChangedEventHandler inpcPropertyChanged;

        protected ViewModelProperty(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the name of the property
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the value of the property
        /// </summary>
        public object Value
        {
            get { return _propertyValue; }

            set
            {
                var oldValue = _propertyValue;
                _propertyValue = value;

                if (!SafeEquals(oldValue, value))
                {
                    if (IsBound)
                    {
                        // only set this if we are bound for change notifications
                        HasChanged = true;
                    }

                    OnPropertyChanged(this, oldValue, value);
                }
            }
        }

        public bool HasChanged { get; private set; }

        public bool IsBound { get; private set; }

        public event ViewModelPropertyChangedEventHandler PropertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { inpcPropertyChanged += value; }
            remove { inpcPropertyChanged -= value; }
        }

        /// <summary>
        /// Notifies the property that it is bound and that it should start sending change notifications
        /// </summary>
        public void Bind()
        {
            if (IsBound)
                return;

            IsBound = true;
            OnBind();
            OnPropertyChanged(this, _propertyValue, _propertyValue);
        }

        /// <summary>
        /// Notifies the property that it is no longer bound and that it should stop sending change notifications
        /// </summary>
        public void Unbind()
        {
            IsBound = false;
            HasChanged = false;
        }

        /// <summary>
        /// Unbinds the property and rebinds when the returned handle is disposed
        /// </summary>
        public IDisposable LoadAndBind()
        {
            return new Unbinder(this);
        }

        public override string ToString()
        {
            return string.Format("[ViewModelProperty: Name={0}, Value={1}]", Name, Value);
        }

        protected virtual void OnBind()
        {
        }

        protected void OnPropertyChanged(ViewModelProperty property, object oldValue, object newValue)
        {
            var handler = PropertyChanged;
            if (IsBound && handler != null)
            {
                handler(this, new ViewModelPropertyChangedEventArgs(property, oldValue, newValue));
            }

            var h2 = inpcPropertyChanged;
            if (IsBound && h2 != null)
            {
                h2(this, new PropertyChangedEventArgs(property.Name));
            }
        }

        private static bool SafeEquals(object obj1, object obj2)
        {
            if (obj1 == null && obj2 == null)
            {
                return true;
            }

            if (obj1 == null && obj2 == null)
            {
                return true;
            }

            if ((obj1 == null && obj2 != null) || (obj1 != null && obj2 == null))
            {
                return false;
            }

            return obj1.Equals(obj2);
        }

        private class Unbinder : IDisposable
        {
            private readonly ViewModelProperty _property;

            public Unbinder(ViewModelProperty property)
            {
                _property = property;
                property.Unbind();
            }

            public void Dispose()
            {
                _property.Bind();
            }
        }
    }
}
