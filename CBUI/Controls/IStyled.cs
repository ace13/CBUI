using System;

namespace CBUI.Controls
{
	public interface IStyled : IControl
	{
		BorderStyle Border { get; set; }

		ConsoleColor? Background { get; set; }
		ConsoleColor? Foreground { get; set; }

		Thickness Padding { get; set; }
	}
}
