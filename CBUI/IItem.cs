using System.Collections.Generic;

namespace CBUI
{
	public interface IItem
	{
		Rect Rect { get; set; }
		Rect Padding { get; set; }
		Rect Margin { get; set; }

		Align AlignJustify { get; set; }
		Align AlignContent { get; set; }
		Align AlignItems { get; set; }
		Align AlignSelf { get; set; }

		Position Position { get; set; }
		Direction Direction { get; set; }
		Wrap Wrap { get; set; }

		float Grow { get; set; }
		float Shrink { get; set; }
		int Order { get; set; }
		float Basis { get; set; }

		IItem Parent { get; }
		IEnumerable<IItem> Children { get; }

		void UpdateLayout();
	}
}
