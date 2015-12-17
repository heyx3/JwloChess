using System;
using System.Collections.Generic;
using UnityEngine;


namespace MrsJMan
{
	public class GameplayTester : MonoBehaviour
	{
		private Board board;

		void Start()
		{
			const int SIZE = 16;

			UnityEngine.Random.seed = 3462312;

			board = new Board();

			board.Reset(new Vector2i(SIZE, SIZE), CellContents.Nothing);
			for (int x = 0; x < SIZE; ++x)
			{
				for (int y = 0; y < SIZE; ++y)
				{
					if (UnityEngine.Random.value > 0.8f)
						board[new Vector2i(x, y)] = CellContents.Wall;
					else if (UnityEngine.Random.value > 0.95f)
						board[new Vector2i(x, y)] = CellContents.Hat;
					else
						board[new Vector2i(x, y)] = CellContents.Dot;
				}
			}

			//Spawn one MrsJMan and then some ghosts.
			int spawned = 0;
			for (int x = 0; x < SIZE && spawned < 2; ++x)
			{
				for (int y = 0; y < SIZE && spawned < 2; ++y)
				{
					if (board[new Vector2i(x, y)] != CellContents.Wall)
					{
						if (spawned == 0)
						{
							Prefabs.Instance.CreateMrsJMan(board, 0, new Vector2i(x, y));
						}
						else
						{
							Prefabs.Instance.CreateGhost(board, 2, new Vector2i(x, y));
						}

						spawned += 1;
					}
				}
			}
		}
	}
}