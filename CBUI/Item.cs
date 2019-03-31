using System.Collections.Generic;

namespace CBUI
{
	public class Item : IItem
	{
		public Rect Rect { get; set; }
		public Rect Padding { get; set; } = new Rect(0,0,0,0);
		public Rect Margin { get; set; } = new Rect(0,0,0,0);

		public Align AlignJustify { get; set; } = Align.Start;
		public Align AlignContent { get; set; } = Align.Stretch;
		public Align AlignItems { get; set; } = Align.Stretch;
		public Align AlignSelf { get; set; } = Align.Auto;

		public Position Position { get; set; } = Position.Relative;
		public Direction Direction { get; set; } = Direction.Column;
		public Wrap Wrap { get; set; } = Wrap.None;

		public float Grow { get; set; } = 0.0f;
		public float Shrink { get; set; } = 1.0f;
		public int Order { get; set; } = 0;
		public float Basis { get; set; } = float.NaN;

		public IItem Parent { get; private set; }
		public IEnumerable<IItem> Children { get { return _Children; } }

		private List<Item> _Children = new List<Item>();
		private Rect _Frame;
		private bool _ShouldOrderChildren;

		public void UpdateLayout()
		{
			_UpdateLayout(Rect.Width, Rect.Height);
		}

		private void _UpdateLayout(float width, float height)
		{
		}
	}
}
