//
// ViewModelProperty.cs
//
// Author:
//       Greg Munn <greg.munn@xamarin.com>
//
// Copyright (c) 2015 Xamarin Inc
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.VisualStudioUI.Options.Models
{
    public static class DisplayableUtil
    {
        public static TItem? FindMatch<TItem>(TItem[] displayables, string? displayString) where TItem : class, IDisplayable
        {
            if (displayString == null)
                return null;
            
            foreach (var displayable in displayables)
            {
                if (displayable.ToDisplayString().Equals(displayString))
                    return displayable;
            }
            return null;
        }
        
        public static DisplayableString[] DisplayableStrings(string[] strings)
        {
            List<DisplayableString> displayableStrings = new List<DisplayableString>();
            foreach (string s in strings)
            {
                displayableStrings.Add(new DisplayableString(s));
            }

            return displayableStrings.ToArray();
        }
    }
}