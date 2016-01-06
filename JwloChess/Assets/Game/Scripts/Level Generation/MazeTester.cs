using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;


namespace LevelGen
{
	public static class MazeTester
	{
		private static char ToChar(Maze m, Vector2i pos)
		{
			Cell c = m.Cells[pos.x, pos.y];

			bool freeLeft = !c.Wall_LessX,
				 freeTop = !c.Wall_LessY,
				 freeRight = !c.Wall_MoreX,
				 freeBottom = !c.Wall_MoreY;

			if (freeLeft && freeRight && freeTop && freeBottom)
				return ' ';

			else if (freeLeft && freeRight && freeTop && !freeBottom)
				return '_';
			else if (freeLeft && freeRight && !freeTop && freeBottom)
				return '`';
			else if (freeLeft && !freeRight && freeTop && freeBottom)
				return ')';
			else if (!freeLeft && freeRight && freeTop && freeBottom)
				return '(';

			else if (freeLeft && freeRight && !freeTop && !freeBottom)
				return '=';
			else if (freeLeft && !freeRight && !freeTop && freeBottom)
				return '\\';
			else if (!freeLeft && !freeRight && freeTop && freeBottom)
				return 'H';
			else if (freeLeft && !freeRight && freeTop && !freeBottom)
				return '/';
			else if (!freeLeft && freeRight && !freeTop && freeBottom)
				return '/';
			else if (!freeLeft && freeRight && freeTop && !freeBottom)
				return '\\';

			else if (freeLeft && !freeRight && !freeTop && !freeBottom)
				return '>';
			else if (!freeLeft && !freeRight && !freeTop && freeBottom)
				return 'n';
			else if (!freeLeft && !freeRight && freeTop && !freeBottom)
				return 'u';
			else if (!freeLeft && freeRight && !freeTop && !freeBottom)
				return '<';

			else if (!freeLeft && !freeRight && !freeTop && !freeBottom)
				return 'o';

			return 'X';
		}
		[MenuItem("MrsJMan/Test Maze Gen")]
		public static void TestMaze()
		{
			//Generate the maze.
			GrowingTreeGenerator gen = new GrowingTreeGenerator(2461);
			Maze m = gen.Generate(8, 8, new Vector2i(7, 0), new Vector2i(7, 0));


			string path = EditorUtility.SaveFilePanel("Choose save location for test file:",
													  Application.dataPath, "Maze", ".txt");

			StreamWriter sw = File.CreateText(path);
			for (int x = 0; x < m.Width; ++x)
			{
				for (int y = 0; y < m.Height; ++y)
				{
					sw.Write(ToChar(m, new Vector2i(x, y)));
				}
				sw.WriteLine();
			}
			sw.Close();
		}
	}
}