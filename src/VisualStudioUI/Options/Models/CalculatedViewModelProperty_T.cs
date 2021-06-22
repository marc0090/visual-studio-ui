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
	/// View model property with a calculated value
	/// </summary>
	public class CalculatedViewModelProperty<T> : ViewModelProperty<T>, IDisposable {
		readonly ViewModelProperty? dependsOn;
		readonly Func<T> calc;

		public CalculatedViewModelProperty (string name, Func<T> calc) : base (name)
		{
			if (calc == null)
				throw new ArgumentNullException (nameof (calc));

			this.calc = calc;
			base.Value = calc ();
		}

		public CalculatedViewModelProperty (string name, ViewModelProperty dependsOn, Func<T> calc) : base (name)
		{
			if (calc == null)
				throw new ArgumentNullException (nameof (calc));

			this.dependsOn = dependsOn;
            dependsOn.PropertyChanged += DependsOn_PropertyChanged;

			this.calc = calc;
			base.Value = calc ();
		}

		public void Dispose()	
        {
			if (dependsOn != null)
			{
				dependsOn.PropertyChanged -= DependsOn_PropertyChanged;
			}
		}

        private void DependsOn_PropertyChanged (object sender, ViewModelPropertyChangedEventArgs e)
        {
			UpdateValue ();
        }

		public void UpdateValue ()
		{
			base.Value = this.calc ();
		}

		protected override void OnBind ()
		{
			this.UpdateValue ();
		}
	}
}