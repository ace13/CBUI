using System;
using System.Collections.Generic;
using System.Linq;

namespace CBUI
{
	public static class ItemExtensions
	{
		public static IItem GetTopmostParent(this IItem control)
		{
			return control?.Parent ?? control;
		}

		public static IEnumerable<IItem> GetSelfAndParents(this IItem control)
		{
			var cur = control;
			
			while (cur != null)
			{
				yield return cur;
				cur = cur.Parent;
			}
		}

		public static IEnumerable<IItem> GetSelfAndChildren(this IItem control)
		{
			yield return control;

			if (control.Children != null)
				foreach (var p in control.Children)
					yield return p;
		}

		public static string GetBorderBrush(this IStyled styled)
		{
			switch (styled.Border)
			{
			case BorderStyle.Filled:
				return "████ ████";
			case BorderStyle.BlockOutline:
				return "▄▄▄█ █▀▀▀";
			case BorderStyle.Outline:
				return "┌─┐│ │└─┘";
			case BorderStyle.DoubleOutline:
				return "╔═╗║ ║╚═╝";

			default:
				return null;
			}
		}


		public static ConsoleColor GetEffectiveBackground(this IItem control)
		{
			ConsoleColor? background = control.GetSelfAndParents()
				.Where(p => (p is IStyled))
				.Select(p => (p as IStyled).Background)
				.FirstOrDefault(b => b.HasValue);

			return background.HasValue ? background.Value : ConsoleColor.Black;
		}

		public static ConsoleColor GetEffectiveForeground(this IItem control)
		{
			ConsoleColor? foreground = control.GetSelfAndParents()
				.Where(p => (p is IStyled))
				.Select(p => (p as IStyled).Foreground)
				.FirstOrDefault(f => f.HasValue);

			return foreground.HasValue ? foreground.Value : ConsoleColor.White;
		}
	}
}
