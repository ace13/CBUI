using System;
using CBUI.Controls;

namespace CBUI.Events
{
	public class FocusGainedEventArgs : EventArgs
	{
		IInputElement _LastFocus;

		IInputElement LastFocus => _LastFocus;

		public FocusGainedEventArgs()
		{
		}

		public FocusGainedEventArgs(IInputElement Last)
		{
			_LastFocus = Last;
		}
	}
}
