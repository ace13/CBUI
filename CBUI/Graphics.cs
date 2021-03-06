﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace CBUI
{
	public static class StringHelper
	{
		public static int ANSILength(this string String)
		{
			var data = String.Split('\x1b');
			return string.Join(" ", data.First() + data.Skip(1).Select(s =>
			{
				int i = s.IndexOf('m');
				return i >= 0 ? s.Substring(i + 1) : s;
			})).Length;
		}

		public static string ANSIColor(this ConsoleColor color, bool background = false)
		{
			int c = 0;
			bool b = false;
			switch (color)
			{
				case ConsoleColor.Gray: c = 30; b = true; break;
				case ConsoleColor.Black: c = 30; break;
				case ConsoleColor.Red: c = 31; b = true; break;
				case ConsoleColor.DarkRed: c = 31; break;
				case ConsoleColor.Green: c = 32; b = true; break;
				case ConsoleColor.DarkGreen: c = 32; break;
				case ConsoleColor.Yellow: c = 33; b = true; break;
				case ConsoleColor.DarkYellow: c = 33; break;
				case ConsoleColor.Blue: c = 34; b = true; break;
				case ConsoleColor.DarkBlue: c = 34; break;
				case ConsoleColor.Magenta: c = 35; b = true; break;
				case ConsoleColor.DarkMagenta: c = 35; break;
				case ConsoleColor.Cyan: c = 36; b = true; break;
				case ConsoleColor.DarkCyan: c = 36; break;
				case ConsoleColor.White: c = 37; b = true; break;
				case ConsoleColor.DarkGray: c = 37; break;
			}

			return string.Format("\x1b[{0}{1}m", background ? c + 10 : c, b ? ";1" : "");
		}

		public static string BackgroundColor(this string String, ConsoleColor color)
		{
			if (String.EndsWith("\x1b[0m", StringComparison.OrdinalIgnoreCase))
				String = String.Substring(0, String.Length - 4);
			return string.Format("{0}{1}\x1b[0m", color.ANSIColor(true), String);
		}
		public static string Color(this string String, ConsoleColor color)
		{
			if (String.EndsWith("\x1b[0m", StringComparison.OrdinalIgnoreCase))
				String = String.Substring(0, String.Length - 4);
			return string.Format("{0}{1}\x1b[0m", color.ANSIColor(), String);
		}
	}

	class ColorChanger : IDisposable
	{
		readonly ConsoleColor Background;
		readonly ConsoleColor Foreground;

		public ColorChanger(ConsoleColor? background = null, ConsoleColor? foreground = null)
		{
			Background = Console.BackgroundColor;
			Foreground = Console.ForegroundColor;

			if (background.HasValue)
				Console.BackgroundColor = background.Value;
			if (foreground.HasValue)
				Console.ForegroundColor = foreground.Value;
		}

		public void Dispose()
		{
			Console.BackgroundColor = Background;
			Console.ForegroundColor = Foreground;
		}
	}
	class CursorChanger : IDisposable
	{
		readonly Point Cursor;
		readonly bool Visible;

		public CursorChanger(Point? newPos = null, bool? visible = null)
		{
			Cursor = new Point(Console.CursorLeft, Console.CursorTop);
			Visible = Console.CursorVisible;

			if (newPos.HasValue)
				Console.SetCursorPosition(newPos.Value.X, newPos.Value.Y);
			if (visible.HasValue)
				Console.CursorVisible = visible.Value;
		}

		public void Dispose()
		{
			Console.SetCursorPosition(Cursor.X, Cursor.Y);
			Console.CursorVisible = Visible;
		}
	}

	public static class Graphics
	{
		static bool IsLinux
		{
			get {
				int p = (int)Environment.OSVersion.Platform;
				return (p == 4 || p == 6 || p == 128);
			}
		}

		public static Size AvailableSize
		{
			get
			{
				return new Size(Console.WindowWidth - 1, Console.WindowHeight);
			}
		}

		public static void WriteANSIString(string String, Point? p = null)
		{
			var data = String;

			var oldPos = new Point(Console.CursorLeft, Console.CursorTop);
			var oldBack = Console.BackgroundColor;
			var oldFore = Console.ForegroundColor;

			if (p.HasValue)
				Console.SetCursorPosition(p.Value.X, p.Value.Y);

			if (IsLinux)
				Console.Write(String);
			else
				do
				{
					var found = data.IndexOf('\x1b');
					if (found > 0)
					{
						Console.Write(data.Substring(0, found));
						data = data.Remove(0, found);
					}
					else if (found == 0)
					{
						var end = data.IndexOf('m');
						var ansi = data.Substring(1, end - 1);
						data = data.Remove(0, end + 1);

						if (ansi[0] != '[')
							continue;

						IEnumerable<int> cmd = ansi.Substring(1).Split(';').Select(c => int.Parse(c));
						int code = cmd.First();
						cmd = cmd.Skip(1);

						bool bold = cmd.FirstOrDefault() == 1;
						switch (code)
						{
							case 0:
								Console.ForegroundColor = oldFore;
								Console.BackgroundColor = oldBack;
								break;

							case 30: Console.ForegroundColor = bold ? ConsoleColor.Gray : ConsoleColor.Black; break;
							case 31: Console.ForegroundColor = bold ? ConsoleColor.Red : ConsoleColor.DarkRed; break;
							case 32: Console.ForegroundColor = bold ? ConsoleColor.Green : ConsoleColor.DarkGreen; break;
							case 33: Console.ForegroundColor = bold ? ConsoleColor.Yellow : ConsoleColor.DarkYellow; break;
							case 34: Console.ForegroundColor = bold ? ConsoleColor.Blue : ConsoleColor.DarkBlue; break;
							case 35: Console.ForegroundColor = bold ? ConsoleColor.Magenta : ConsoleColor.DarkMagenta; break;
							case 36: Console.ForegroundColor = bold ? ConsoleColor.Cyan : ConsoleColor.DarkCyan; break;
							case 37: Console.ForegroundColor = bold ? ConsoleColor.White : ConsoleColor.DarkGray; break;
							case 39: Console.ForegroundColor = oldFore; break;

							case 40: Console.BackgroundColor = bold ? ConsoleColor.Gray : ConsoleColor.Black; break;
							case 41: Console.BackgroundColor = bold ? ConsoleColor.Red : ConsoleColor.DarkRed; break;
							case 42: Console.BackgroundColor = bold ? ConsoleColor.Green : ConsoleColor.DarkGreen; break;
							case 43: Console.BackgroundColor = bold ? ConsoleColor.Yellow : ConsoleColor.DarkYellow; break;
							case 44: Console.BackgroundColor = bold ? ConsoleColor.Blue : ConsoleColor.DarkBlue; break;
							case 45: Console.BackgroundColor = bold ? ConsoleColor.Magenta : ConsoleColor.DarkMagenta; break;
							case 46: Console.BackgroundColor = bold ? ConsoleColor.Cyan : ConsoleColor.DarkCyan; break;
							case 47: Console.BackgroundColor = bold ? ConsoleColor.White : ConsoleColor.DarkGray; break;
							case 49: Console.BackgroundColor = oldBack; break;
						}
					}
					else
					{
						Console.Write(data);
						break;
					}
				} while (!string.IsNullOrEmpty(data));

			Console.BackgroundColor = oldBack;
			Console.ForegroundColor = oldFore;

			if (p.HasValue)
				Console.SetCursorPosition(oldPos.X, oldPos.Y);
		}

		public static void DrawChar(Point p, char c, ConsoleColor Background = ConsoleColor.Black, ConsoleColor Foreground = ConsoleColor.White)
		{
			using (new CursorChanger(p, false))
			using (new ColorChanger(Background, Foreground))
				Console.Write(c);
		}

		public static void DrawLine(Point a, Point b, char Character, ConsoleColor Background = ConsoleColor.Black, ConsoleColor Foreground = ConsoleColor.White)
		{
			using (new CursorChanger(null, false))
			using (new ColorChanger(Background, Foreground))
				Line(a.X, a.Y, b.X, b.Y, (x, y) =>
				{
					Console.SetCursorPosition(x, y);
					Console.Write(Character);
					return true;
				});
		}

		public static void DrawFilledBox(Point p1, Size sz, char c, ConsoleColor Background = ConsoleColor.Black, ConsoleColor Foreground = ConsoleColor.White)
		{
			DrawFilledBox(p1, p1 + sz, c, Background, Foreground);
		}

		public static void DrawFilledBox(Point p1, Point p2, char c, ConsoleColor Background = ConsoleColor.Black, ConsoleColor Foreground = ConsoleColor.White)
		{
			using (new CursorChanger(null, false))
			using (new ColorChanger(Background, Foreground))
			{
				var tmp = new string(c, p2.X - p1.X);

				for (int y = p1.Y; y != p2.Y; y += Math.Sign(p2.Y - p1.Y))
				{
					Console.SetCursorPosition(p1.X, y);
					Console.Write(tmp);
				}
			}
		}

		public static void DrawBox(Point pos, Point size, string BoxBrush, ConsoleColor Background = ConsoleColor.White, ConsoleColor Foreground = ConsoleColor.White)
		{
			if (BoxBrush == null)
				throw new ArgumentNullException(nameof(BoxBrush));
			if (BoxBrush.Length != 9)
				throw new ArgumentOutOfRangeException(nameof(BoxBrush));

			using (new CursorChanger(pos, false))
			using (new ColorChanger(Background, Foreground))
			{
				DrawChar(pos, BoxBrush[0]);
				DrawChar(pos + new Point(size.X, 0), BoxBrush[2]);
				DrawChar(pos + new Point(size.X, size.Y), BoxBrush[8]);
				DrawChar(pos + new Point(0, size.Y), BoxBrush[6]);

				DrawLine(pos + new Point(1, 0), pos + new Point(size.X - 1, 0), BoxBrush[1]);
				DrawLine(pos + new Point(0, 1), pos + new Point(0, size.Y - 1), BoxBrush[3]);
				DrawLine(pos + new Point(size.X, size.Y - 1), pos + new Point(size.X, 1), BoxBrush[5]);
				DrawLine(pos + new Point(size.X - 1, size.Y), pos + new Point(1, size.Y), BoxBrush[7]);
			}

			DrawFilledBox(pos + new Point(1, 1), pos + size - new Point(1, 1), BoxBrush[4]);
		}

		#region internal
		static void Swap<T>(ref T lhs, ref T rhs) { T temp; temp = lhs; lhs = rhs; rhs = temp; }

		public delegate bool PlotFunction(int x, int y);

		public static void Line(int x0, int y0, int x1, int y1, PlotFunction plot)
		{
			bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
			if (steep) { Swap<int>(ref x0, ref y0); Swap<int>(ref x1, ref y1); }
			if (x0 > x1) { Swap<int>(ref x0, ref x1); Swap<int>(ref y0, ref y1); }
			int dX = (x1 - x0), dY = Math.Abs(y1 - y0), err = (dX / 2), ystep = (y0 < y1 ? 1 : -1), y = y0;

			for (int x = x0; x <= x1; ++x)
			{
				if (!(steep ? plot(y, x) : plot(x, y))) return;
				err = err - dY;
				if (err < 0) { y += ystep; err += dX; }
			}
		}
		#endregion
	}
}

