using System;

namespace CBUI
{
	public interface IStyled
	{
		BorderStyle Border { get; set; }

		ConsoleColor? Background { get; set; }
		ConsoleColor? Foreground { get; set; }
	}
}
