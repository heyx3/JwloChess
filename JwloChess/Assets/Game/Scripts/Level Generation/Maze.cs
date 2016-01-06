using System;
using System.Collections.Generic;
using Assert = UnityEngine.Assertions.Assert;


namespace LevelGen
{
	public struct Cell
	{
		public bool Wall_LessX,
					Wall_LessY,
					Wall_MoreX,
					Wall_MoreY;


		public int NumberOfWalls
		{
			get
			{
				return (Wall_LessX ? 1 : 0) +
					   (Wall_LessY ? 1 : 0) +
					   (Wall_MoreX ? 1 : 0) +
					   (Wall_MoreY ? 1 : 0);
			}
		}


		public Cell(bool wall_LessX, bool wall_LessY, bool wall_MoreX, bool wall_MoreY)
		{
			Wall_LessX = wall_LessX;
			Wall_LessY = wall_LessY;
			Wall_MoreX = wall_MoreX;
			Wall_MoreY = wall_MoreY;
		}


		public void SetAllWalls(bool hasWalls)
		{
			Wall_LessX = hasWalls;
			Wall_LessY = hasWalls;
			Wall_MoreX = hasWalls;
			Wall_MoreY = hasWalls;
		}
	}


	public class Maze
	{
		public Cell[,] Cells;


		public int Width { get { return Cells.GetLength(0); } }
		public int Height { get { return Cells.GetLength(1); } }


		public Maze(int width, int height)
		{
			Cells = new Cell[width, height];
			for (int x = 0; x < width; ++x)
			{
				for (int y = 0; y < height; ++y)
				{
					Cells[x, y].SetAllWalls(true);
				}
			}
		}
		

		public void CarvePath(Vector2i from, Vector2i dir)
		{
			//Make sure they're adjacent.
			Assert.IsTrue((dir.x == 0 ^ dir.y == 0) &&
						  ((dir.x == 1 || dir.x == -1) ^ (dir.y == 1 || dir.y == -1)));

			if (dir == new Vector2i(-1, 0))
			{
				Assert.IsTrue(from.x > 0, "From is " + from + " and maze width is " + Width);

				Cells[from.x, from.y].Wall_LessX = false;
				Cells[from.x - 1, from.y].Wall_MoreX = false;
			}
			else if (dir == new Vector2i(1, 0))
			{
				Assert.IsTrue(from.x < (Width - 1), "From is " + from + " and maze width is " + Width);

				Cells[from.x, from.y].Wall_MoreX = false;
				Cells[from.x + 1, from.y].Wall_LessX = false;
			}
			else if (dir == new Vector2i(0, -1))
			{
				Assert.IsTrue(from.y > 0, "From is " + from + " and maze height is " + Height);
				Cells[from.x, from.y].Wall_LessY = false;
				Cells[from.x, from.y - 1].Wall_MoreY = false;
			}
			else
			{
				Assert.IsTrue(dir == new Vector2i(0, 1));
				Assert.IsTrue(from.y < (Height - 1), "From is " + from + " and maze height is " + Height);

				Cells[from.x, from.y].Wall_MoreY = false;
				Cells[from.x, from.y + 1].Wall_LessY = false;
			}
		}

		public bool IsValidPos(Vector2i pos)
		{
			return (pos.x >= 0 && pos.x < Width &&
					pos.y >= 0 && pos.y < Height);
		}
	}
}