//
// ViewModelProperty.cs
//
// Author:
//       Greg Munn <greg.munn@xamarin.com>
//
// Copyright (c) 2015 Xamarin Inc
//

using System;

namespace Microsoft.VisualStudioUI.Options.Models
{
    /// <summary>
    /// Generic view model property
    /// </summary>
    public sealed class ViewModelProperty<T> : ViewModelProperty
    {
        public ViewModelProperty(string name, T defaultValue) : base(name)
        {
            this.Value = defaultValue;
        }

        public ViewModelProperty(string name) : base(name)
        {
            this.Value = default(T);
        }

        /// <summary>
        /// Gets or sets the value of the property
        /// </summary>
        public new T Value
        {
            get { return (T) base.Value; }

            set { base.Value = value; }
        }
    }
}