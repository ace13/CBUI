using System.Collections.Generic;

namespace CBUI.Controls
{
	public interface IControl : IInputElement, ILayoutable, INamed, IVisual
	{
		IControl Parent { get; }

		IEnumerable<IControl> Parents { get; }
		ControlCollection Children { get; }
	}
}
