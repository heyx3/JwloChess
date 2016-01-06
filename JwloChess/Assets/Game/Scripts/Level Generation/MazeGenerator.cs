using System;
using System.Collections.Generic;
using Assert = UnityEngine.Assertions.Assert;


namespace LevelGen
{
	public static class ListExtensions
	{
		/// <summary>
		/// Shuffles the given list using the given RNG.
		/// Useful for various generation algorithms.
		/// </summary>
		public static void Shuffle<T>(this IList<T> toShuffle, Random rnd)
		{
			int n = toShuffle.Count;
			while (n > 1)
			{
				n -= 1;
				
				int i = rnd.Next(n + 1);
				T value = toShuffle[i];
				toShuffle[i] = toShuffle[n];
				toShuffle[n] = value;
			}
		}
	}

	public abstract class MazeGenerator
	{
		protected Random Rand { get; private set; }


		public MazeGenerator() { Rand = new Random(); }
		public MazeGenerator(int seed) { Rand = new Random(seed); }


		public Maze Generate(int width, int height, Vector2i ghostHomeMin, Vector2i ghostHomeMax)
		{
			Maze maze = new Maze(width, height);

			RunAlgo(maze, ghostHomeMin, ghostHomeMax);

			//Remove any dead-ends; Pac-man boards can't have dead ends.
			for (int x = 0; x < width; ++x)
			{
				for (int y = 0; y < height; ++y)
				{
					Vector2i pos = new Vector2i(x, y);

					Cell c = maze.Cells[x, y];
					if (c.NumberOfWalls == 3)
					{
						//Remove a wall so that two opposite sides are open.
						if (c.Wall_LessX && c.Wall_MoreX)
						{
							//If on the edge of the maze, don't open it up.
							if ((c.Wall_LessY && y == 0) ||
								(c.Wall_MoreY && y == height - 1))
							{
								if ((x == (width - 1) || (x > 0 && Rand.NextDouble() < 0.5)))
									maze.CarvePath(pos, new Vector2i(-1, 0));
								else
									maze.CarvePath(pos, new Vector2i(1, 0));
							}
							else
							{
								if (c.Wall_LessY)
									maze.CarvePath(pos, new Vector2i(0, -1));
								else
									maze.CarvePath(pos, new Vector2i(0, 1));
							}
						}
						else
						{
							//If on the edge of the maze, don't open it up.
							if ((c.Wall_LessX && x == 0) ||
								(c.Wall_MoreX && x == width - 1))
							{
								if (y == (height - 1) || (y > 0 && Rand.NextDouble() < 0.5))
									maze.CarvePath(pos, new Vector2i(0, -1));
								else
									maze.CarvePath(pos, new Vector2i(0, 1));
							}
							else
							{
								if (c.Wall_LessX)
									maze.CarvePath(pos, new Vector2i(-1, 0));
								else
									maze.CarvePath(pos, new Vector2i(1, 0));
							}
						}
					}
				}
			}

			return maze;
		}
		protected abstract void RunAlgo(Maze maze, Vector2i ghostHomeMin, Vector2i ghostHomeMax);
	}
	

	/// <summary>
	/// Uses the "Growing Tree" algorithm to generate the maze.
	/// Taken from http://weblog.jamisbuck.org/2011/1/27/maze-generation-growing-tree-algorithm
	/// </summary>
	public class GrowingTreeGenerator : MazeGenerator
	{
		public delegate int ElementChooser(Random r, int nElements);

		public static int Chooser_Default(Random r, int nElements)
		{
			return nElements - 1;
			if (r.NextDouble() > 0.25)
			{
				return nElements - 1;
			}
			else
			{
				return r.Next(nElements);
			}
		}
		public static int Chooser_Straight(Random r, int nElements)
		{
			return 0;
		}
		public static int Chooser_Choppy(Random r, int nElements)
		{
			return r.Next(nElements);
		}


		public ElementChooser CellChooser = Chooser_Default;


		public GrowingTreeGenerator() : base() { }
		public GrowingTreeGenerator(int seed) : base(seed) { }


		protected override void RunAlgo(Maze maze, Vector2i ghostHomeMin, Vector2i ghostHomeMax)
		{
			bool[,] visitedCells = new bool[maze.Width, maze.Height];
			for (int x = 0; x < maze.Width; ++x)
				for (int y = 0; y < maze.Height; ++y)
					visitedCells[x, y] = false;

			List<Vector2i> cells = new List<Vector2i>();

			//Populate the cells initially with everything around the ghost home,
			//    which itself must stay open.
			for (int x = ghostHomeMin.x; x <= ghostHomeMax.x; ++x)
			{
				for (int y = ghostHomeMin.y; y <= ghostHomeMax.y; ++y)
				{
					Vector2i v = new Vector2i(x, y);
					if (x == ghostHomeMin.x && maze.IsValidPos(v.LessX) && !visitedCells[x - 1, y])
					{
						cells.Add(v.LessX);
					}
					if (y == ghostHomeMin.y && maze.IsValidPos(v.LessY) && !visitedCells[x, y - 1])
					{
						cells.Add(v.LessY);
					}
					if (x == ghostHomeMax.x && maze.IsValidPos(v.MoreX) && !visitedCells[x + 1, y])
					{
						cells.Add(v.MoreX);
					}
					if (y == ghostHomeMax.y && maze.IsValidPos(v.MoreY) && !visitedCells[x, y + 1])
					{
						cells.Add(v.MoreY);
					}
				}
			}
			cells.Shuffle(Rand);


			//Run the algorithm as long as there are cells left to examine.
			List<Vector2i> neighbors = new List<Vector2i>(4);
			while (cells.Count > 0)
			{
				//Choose the next cell to iterate over.
				int index = CellChooser(Rand, cells.Count);
				Vector2i pos = cells[index];
				
				//Get all unvisited neighbors.
				neighbors.Clear();
				if (maze.IsValidPos(pos.LessX) && !visitedCells[pos.x - 1, pos.y])
					neighbors.Add(pos.LessX);
				if (maze.IsValidPos(pos.LessY) && !visitedCells[pos.x, pos.y - 1])
					neighbors.Add(pos.LessY);
				if (maze.IsValidPos(pos.MoreX) && !visitedCells[pos.x + 1, pos.y])
					neighbors.Add(pos.MoreX);
				if (maze.IsValidPos(pos.MoreY) && !visitedCells[pos.x, pos.y + 1])
					neighbors.Add(pos.MoreY);

				//If there is at least one, choose a neighbor and carve from this cell into that one.
				if (neighbors.Count > 0)
				{
					Vector2i neighbor = neighbors[Rand.Next(neighbors.Count)];

					visitedCells[neighbor.x, neighbor.y] = true;
					maze.CarvePath(pos, (neighbor - pos));
					cells.Add(neighbor);
				}
				//Otherwise, remove this cell from the list.
				else
				{
					cells.Remove(pos);
				}
			}
		}
	}
}