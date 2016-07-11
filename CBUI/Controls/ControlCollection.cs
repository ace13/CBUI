using System.Collections.Generic;

namespace CBUI.Controls
{
	public class ControlCollection : List<IControl>
	{
		public ControlCollection()
		{
		}

		public ControlCollection(IEnumerable<IControl> list)
		{
			AddRange(list);
		}
	}
}
