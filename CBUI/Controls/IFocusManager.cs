using System;

namespace CBUI.Controls
{
	public interface IFocusManager
	{
		IInputElement CurrentFocus { get; set; }
	}
}
