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
    public class DisplayableString : IDisplayable
    {
        public string String { get; }

        public DisplayableString(string str)
        {
            String = str;
        }

        public string ToDisplayString() => String;

        public override string ToString() => String;
    }
}