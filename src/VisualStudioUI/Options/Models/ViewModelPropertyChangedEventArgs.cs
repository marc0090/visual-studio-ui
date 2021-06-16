//
// ViewModelPropertyChangedEventArgs.cs
//
// Author:
//       Greg Munn <greg.munn@xamarin.com>
//
// Copyright (c) 2015 Xamarin Inc
//

using System;

namespace Microsoft.VisualStudioUI.Options.Models
{
    public sealed class ViewModelPropertyChangedEventArgs : EventArgs
    {
        public ViewModelPropertyChangedEventArgs(ViewModelProperty property, object oldValue, object newValue)
        {
            this.Property = property;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        /// <summary>
        /// Gets the property that changed
        /// </summary>
        public ViewModelProperty Property { get; private set; }

        /// <summary>
        /// Gets the old value of the property
        /// </summary>
        public object OldValue { get; private set; }

        /// <summary>
        /// Gets the new value of the property
        /// </summary>
        public object NewValue { get; private set; }
    }

    public delegate void ViewModelPropertyChangedEventHandler(object sender, ViewModelPropertyChangedEventArgs e);
}