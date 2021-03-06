﻿namespace CBUI.Controls
{
	public interface IContentControl : IControl
	{
		object Content { get; set; }

		HorizontalAlignment HorizontalContentAlignment { get; set; }
		VerticalAlignment VerticalContentAlignment { get; set; }
	}
}
