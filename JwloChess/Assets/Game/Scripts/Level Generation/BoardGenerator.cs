using System;
using System.Collections.Generic;
using UnityEngine;
using MrsJMan;


namespace LevelGen
{
	public class LevelData
	{
		public CellContents[,] GameBoard;
		public Vector2i GhostHomeMin, GhostHomeMax;
		public Vector2i MrsJManStart;
		public List<Vector2i> GhostStarts;
		public List<Vector2i> ChocolateSpawns;
	}


	public enum MazeTypes
	{
		Normal,
		Choppy,
		Straight,
	}

	
	/// <summary>
	/// Generates a level for a match.
	/// </summary>
	public class BoardGenerator
	{
		public static LevelData GenerateLevel(bool mirrorX, bool mirrorY,
											  int fullWidth, int fullHeight,
											  float reflectionHolesChance,
											  MazeTypes type, int seed)
		{
			//Initialize return data.

			LevelData dat = new LevelData();
			dat.GameBoard = null;
			dat.MrsJManStart = new Vector2i(-1, -1);
			dat.GhostStarts = new List<Vector2i>(4);
			dat.ChocolateSpawns = new List<Vector2i>(1);


			//Do the basic generation of a section of maze.

			GrowingTreeGenerator gen = new GrowingTreeGenerator(seed);
			switch (type)
			{
				case MazeTypes.Normal:
					gen.CellChooser = GrowingTreeGenerator.Chooser_Default;
					break;
				case MazeTypes.Straight:
					gen.CellChooser = GrowingTreeGenerator.Chooser_Straight;
					break;
				case MazeTypes.Choppy:
					gen.CellChooser = GrowingTreeGenerator.Chooser_Choppy;
					break;
				default: throw new NotImplementedException(type.ToString());
			}

			Vector2i ghostHomeMin = new Vector2i((fullWidth / 2) - 1,
												 (fullHeight / 2) - 1);

			//Generate the unique section of the maze.
			//This will later be mirrored across the X or Y if necessary.
			Maze maze = gen.Generate(mirrorX ? (fullWidth / 2) : fullWidth,
									 mirrorY ? (fullHeight / 2) : fullHeight,
									 ghostHomeMin,
									 ghostHomeMin + new Vector2i(mirrorX ? 0 : 1,
																 mirrorY ? 0 : 1));


			//Mirror the board across the X and Y if necessary.
			
			Maze fullMaze = new Maze(fullWidth, fullHeight);
			
			dat.GhostHomeMin = (ghostHomeMin * 2) + new Vector2i(1, 1);
			dat.GhostHomeMax = dat.GhostHomeMin + new Vector2i(2, 2);

			for (int x = 0; x < maze.Width; ++x)
			{
				for (int y = 0; y < maze.Height; ++y)
				{
					fullMaze.Cells[x, y] = maze.Cells[x, y];
					Cell c = maze.Cells[x, y];

					if (mirrorX)
					{
						fullMaze.Cells[fullMaze.Width - 1 - x, y] = new Cell(c.Wall_MoreX, c.Wall_LessY,
																			 c.Wall_LessX, c.Wall_MoreY);
						if (mirrorY)
						{
							fullMaze.Cells[fullMaze.Width - 1 - x,
										   fullMaze.Height - 1 - y] = new Cell(c.Wall_MoreX, c.Wall_MoreY,
																			   c.Wall_LessX, c.Wall_LessY);
						}
					}
					if (mirrorY)
					{
						fullMaze.Cells[x, fullMaze.Height - 1 - y] = new Cell(c.Wall_LessX, c.Wall_MoreY,
																			  c.Wall_MoreX, c.Wall_LessY);
					}
				}
			}


			//Convert to game board representation.
			dat.GameBoard = new CellContents[(fullMaze.Width * 2) + 1,
											 (fullMaze.Height * 2) + 1];
			for (int x = 0; x < fullMaze.Width; ++x)
			{
				for (int y = 0; y < fullMaze.Height; ++y)
				{
					int wallX = x * 2,
						wallY = y * 2;
					int cellX = wallX + 1,
						cellY = wallY + 1;
					
					UnityEngine.Assertions.Assert.IsTrue(cellY < dat.GameBoard.GetLength(1),
														 "cellY is " + cellY.ToString() +
															", game board is " + dat.GameBoard.GetLength(1) +
															" tall, maze is " + fullMaze.Height + " tall");

					Cell cell = fullMaze.Cells[x, y];

					//Fill in the walls right behind this cell.
					dat.GameBoard[wallX, wallY] = CellContents.Wall;
					dat.GameBoard[wallX, cellY] = (cell.Wall_LessX ?
													   CellContents.Wall :
													   CellContents.Dot);
					dat.GameBoard[cellX, wallY] = (cell.Wall_LessY ?
													   CellContents.Wall :
													   CellContents.Dot);

					//Now set the cell.
					dat.GameBoard[cellX, cellY] = CellContents.Dot;

					//If this is the max border along the X or Y, add the walls after the cell as well.
					if (x == fullMaze.Width - 1)
					{
						int wallX2 = cellX + 1;
						dat.GameBoard[wallX2, cellY] = (cell.Wall_MoreX ?
															CellContents.Wall :
															CellContents.Dot);
						dat.GameBoard[wallX2, wallY] = CellContents.Wall;

						if (y == fullMaze.Height - 1)
						{
							dat.GameBoard[wallX2, cellY + 1] = CellContents.Wall;
						}
					}
					if (y == fullMaze.Height - 1)
					{
						int wallY2 = cellY + 1;
						dat.GameBoard[cellX, wallY2] = (cell.Wall_MoreY ?
															CellContents.Wall :
															CellContents.Dot);
						dat.GameBoard[wallX, wallY2] = CellContents.Wall;
					}
				}
			}
			

			//Add gaps between the original part of the board and the reflections.
			UnityEngine.Random.seed = seed;
			if (mirrorX)
			{
				int midX = (dat.GameBoard.GetLength(0) / 2);
				for (int y = 0; y < dat.GameBoard.GetLength(1); ++y)
				{
					if (dat.GameBoard[midX - 1, y] != CellContents.Wall &&
						dat.GameBoard[midX + 1, y] != CellContents.Wall &&
						UnityEngine.Random.value < reflectionHolesChance)
					{
						dat.GameBoard[midX, y] = CellContents.Dot;
						
						//If mirroring along the Y axis, we also need to do this
						//    on the other half of the board.
						if (mirrorY)
						{
							dat.GameBoard[midX, dat.GameBoard.GetLength(1) - y - 1] = CellContents.Dot;
						}
					}
				}
			}
			if (mirrorY)
			{
				int midY = (dat.GameBoard.GetLength(1) / 2);
				for (int x = 0; x < maze.Width; ++x)
				{
					if (dat.GameBoard[x, midY - 1] != CellContents.Wall &&
						dat.GameBoard[x, midY + 1] != CellContents.Wall &&
						UnityEngine.Random.value < reflectionHolesChance)
					{
						dat.GameBoard[x, midY] = CellContents.Dot;

						//If mirroring along the X axis, we also need to do this
						//    on the other half of the board.
						if (mirrorX)
						{
							dat.GameBoard[dat.GameBoard.GetLength(0) - x - 1, midY] = CellContents.Dot;
						}
					}
				}
			}

			//Go back and clear out the ghost home.
			for (int x = dat.GhostHomeMin.x; x <= dat.GhostHomeMax.x; ++x)
				for (int y = dat.GhostHomeMin.y; y <= dat.GhostHomeMax.y; ++y)
					dat.GameBoard[x, y] = CellContents.Nothing;

			dat.GhostHomeMin += new Vector2i(1, 1);
			dat.GhostHomeMax -= new Vector2i(1, 1);
			
			//Randomly search around for an open area near the ghost home to spawn chocolate in.
			Vector2i searchPos = dat.GhostHomeMin +
								 new Vector2i(UnityEngine.Random.Range(0, 3),
											  UnityEngine.Random.Range(0, 3));
			dat.ChocolateSpawns.Add(FindEmptySpaceNear(searchPos, dat.GameBoard, v => true));


			//Randomly search around for an open area near the corners to spawn hats and players in.

			Predicate<Vector2i> isValidSpawn = v =>
				{
					return dat.GameBoard[v.x, v.y] != CellContents.Hat &&
						   !dat.ChocolateSpawns.Contains(v) &&
						   !dat.GhostStarts.Contains(v) &&
						   (v.x < dat.GhostHomeMin.x || v.x > dat.GhostHomeMax.x ||
							v.y < dat.GhostHomeMin.y || v.y > dat.GhostHomeMax.y);
				};

			searchPos = new Vector2i(1, 1);
			Vector2i hatPos1 = FindEmptySpaceNear(searchPos, dat.GameBoard, isValidSpawn);
			dat.GameBoard[hatPos1.x, hatPos1.y] = CellContents.Hat;
			dat.GhostStarts.Add(FindEmptySpaceNear(searchPos, dat.GameBoard, isValidSpawn));

			if (mirrorX)
			{
				dat.GameBoard[dat.GameBoard.GetLength(0) - 1 - hatPos1.x, hatPos1.y] = CellContents.Hat;
				Vector2i ghostPos = dat.GhostStarts[0];
				dat.GhostStarts.Add(new Vector2i(dat.GameBoard.GetLength(0) - 1 - ghostPos.x,
												 ghostPos.y));
			}
			else
			{
				searchPos = new Vector2i(dat.GameBoard.GetLength(0) - 2, 1);
				Vector2i hatPos2 = FindEmptySpaceNear(searchPos, dat.GameBoard, isValidSpawn);
				dat.GameBoard[hatPos2.x, hatPos2.y] = CellContents.Hat;
				dat.GhostStarts.Add(FindEmptySpaceNear(searchPos, dat.GameBoard, isValidSpawn));
			}

			if (mirrorY)
			{
				dat.GameBoard[hatPos1.x, dat.GameBoard.GetLength(1) - 1 - hatPos1.y] = CellContents.Hat;
				Vector2i ghostPos = dat.GhostStarts[0];
				dat.GhostStarts.Add(new Vector2i(ghostPos.x,
												 dat.GameBoard.GetLength(1) - 1 - ghostPos.y));
			}
			else
			{
				searchPos = new Vector2i(1, dat.GameBoard.GetLength(1) - 2);
				Vector2i hatPos2 = FindEmptySpaceNear(searchPos, dat.GameBoard, isValidSpawn);
				dat.GameBoard[hatPos2.x, hatPos2.y] = CellContents.Hat;
				dat.GhostStarts.Add(FindEmptySpaceNear(searchPos, dat.GameBoard, isValidSpawn));
			}


			if (mirrorX && mirrorY)
			{
				dat.GameBoard[dat.GameBoard.GetLength(0) - 1 - hatPos1.x,
							  dat.GameBoard.GetLength(1) - 1 - hatPos1.y] = CellContents.Hat;
				Vector2i ghostPos = dat.GhostStarts[0];
				dat.GhostStarts.Add(new Vector2i(dat.GameBoard.GetLength(0) - 1 - ghostPos.x,
												 dat.GameBoard.GetLength(1) - 1 - ghostPos.y));
			}
			else
			{
				searchPos = new Vector2i(dat.GameBoard.GetLength(0) - 2,
											dat.GameBoard.GetLength(1) - 2);
				Vector2i hatPos2 = FindEmptySpaceNear(searchPos, dat.GameBoard, isValidSpawn);
				dat.GameBoard[hatPos2.x, hatPos2.y] = CellContents.Hat;
				dat.GhostStarts.Add(FindEmptySpaceNear(searchPos, dat.GameBoard, isValidSpawn));
			}

			dat.MrsJManStart = FindEmptySpaceNear(dat.ChocolateSpawns[0], dat.GameBoard, isValidSpawn);
			
			return dat;
		}

		private static Vector2i FindEmptySpaceNear(Vector2i searchStart, CellContents[,] board,
												   Predicate<Vector2i> isPosAcceptable)
		{
			List<Vector2i> toTry = new List<Vector2i>();
			toTry.Add(searchStart);

			while (toTry.Count > 0)
			{
				Vector2i next = toTry[UnityEngine.Random.Range(0, toTry.Count)];
				if (board[next.x, next.y] != CellContents.Wall && isPosAcceptable(next))
				{
					return next;
				}
				else
				{
					Vector2i temp = next.LessX;
					if (temp.x >= 0 && !toTry.Contains(temp) &&
						board[temp.x, temp.y] != CellContents.Wall && isPosAcceptable(temp))
					{
						toTry.Add(temp);
					}

					temp = next.MoreX;
					if (temp.x < board.GetLength(0) && !toTry.Contains(temp) &&
						board[temp.x, temp.y] != CellContents.Wall && isPosAcceptable(temp))
					{
						toTry.Add(temp);
					}

					temp = next.LessY;
					if (temp.y >= 0 && !toTry.Contains(temp) &&
						board[temp.x, temp.y] != CellContents.Wall && isPosAcceptable(temp))
					{
						toTry.Add(temp);
					}

					temp = next.MoreY;
					if (temp.y < board.GetLength(1) && !toTry.Contains(temp) &&
						board[temp.x, temp.y] != CellContents.Wall && isPosAcceptable(temp))
					{
						toTry.Add(temp);
					}
				}
			}

			return new Vector2i(-1, -1);
		}
	}
}