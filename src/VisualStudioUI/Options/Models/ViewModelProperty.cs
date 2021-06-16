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
        object propertyValue;
        event PropertyChangedEventHandler inpcPropertyChanged;

        protected ViewModelProperty(string name)
        {
            this.Name = name;
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
            get { return this.propertyValue; }

            set
            {
                var oldValue = propertyValue;
                this.propertyValue = value;

                if (!SafeEquals(oldValue, value))
                {
                    if (this.IsBound)
                    {
                        // only set this if we are bound for change notifications
                        this.HasChanged = true;
                    }

                    this.OnPropertyChanged(this, oldValue, value);
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
            if (this.IsBound)
                return;

            this.IsBound = true;
            this.OnBind();
            this.OnPropertyChanged(this, this.propertyValue, this.propertyValue);
        }

        /// <summary>
        /// Notifies the property that it is no longer bound and that it should stop sending change notifications
        /// </summary>
        public void Unbind()
        {
            this.IsBound = false;
            this.HasChanged = false;
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
            var handler = this.PropertyChanged;
            if (this.IsBound && handler != null)
            {
                handler(this, new ViewModelPropertyChangedEventArgs(property, oldValue, newValue));
            }

            var h2 = this.inpcPropertyChanged;
            if (this.IsBound && h2 != null)
            {
                h2(this, new PropertyChangedEventArgs(property.Name));
            }
        }

        static bool SafeEquals(object obj1, object obj2)
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

        class Unbinder : IDisposable
        {
            readonly ViewModelProperty property;

            public Unbinder(ViewModelProperty property)
            {
                this.property = property;
                this.property.Unbind();
            }

            public void Dispose()
            {
                this.property.Bind();
            }
        }
    }
}