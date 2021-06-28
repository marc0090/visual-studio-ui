using System;
using System.Collections.Generic;

namespace Microsoft.VisualStudioUI.Options
{
    /// <summary>
    /// 
    /// </summary>
    public class RadioButtonGroup
    {
        private readonly List<RadioButtonOption> _radioButtons = new List<RadioButtonOption>();
        private bool _updatingSelection = false;

        public IEnumerable<RadioButtonOption> RadioButtons => _radioButtons;

        public void Select(RadioButtonOption radioButton)
        {
            if (_updatingSelection)
                throw new InvalidOperationException("Can't recursively call Select in middle of updating selection");
            
            try
            {
                _updatingSelection = true;
                foreach (RadioButtonOption currRadioButton in _radioButtons)
                {
                    bool currIsSelected = object.ReferenceEquals(currRadioButton, radioButton);
                    currRadioButton.Property.Value = currIsSelected;
                }
            }
            finally
            {
                _updatingSelection = false;
            }
        }

        public void Deselect(RadioButtonOption radioButton)
        {
            int count = _radioButtons.Count;
            
            int index = -1;
            for (int i = 0; i < count; ++i)
            {
                if (object.ReferenceEquals(_radioButtons[i], radioButton))
                {
                    index = i;
                    break;
                }
            }

            // If the radio button isn't found, just do nothing
            if (index == -1)
                return;

            // If there's a single radio button, just deselect it
            if (count == 1)
            {
                radioButton.Property.Value = false;
                return;
            }

            // Otherwise, select the next radio button, so something is selected. By doing this,
            // when there are only two radio buttons, either can be set to false to toggle the
            // other to true.
            int nextIndex = (index + 1) % count;
            RadioButtonOption nextRadioButton = _radioButtons[nextIndex];
            Select(nextRadioButton);
        }

        internal void AddRadioButton(RadioButtonOption radioButton)
        {
            _radioButtons.Add(radioButton);

            radioButton.Property.PropertyChanged += delegate
            {
                if (_updatingSelection)
                    return;
                
                if (radioButton.Property.Value)
                {
                    Select(radioButton);
                }
                else
                {
                    Deselect(radioButton);
                }
            };
        }
    }
}
