using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MrsJMan
{
	public static class GameGenerator
	{
		private static LevelGen.LevelData LvlDat { get { return GameSettings.GeneratedLevel; } }


		/// <summary>
		/// Creates a game board (or possibly a game board preview).
		/// Returns the container for all created GameObjets.
		/// </summary>
		public static Transform GenerateGame(out Board board, Camera gameCam, bool isPreview)
		{
			Transform containerTr = new GameObject("Game Board").transform;


			board = new Board();
			board.ChocolateSpawns = LvlDat.ChocolateSpawns;
			board.GameObjsParent = containerTr;
			board.Reset(new Vector2i(LvlDat.GameBoard.GetLength(0),
									 LvlDat.GameBoard.GetLength(1)));

			//Set the board's tiles.
			for (int x = 0; x < LvlDat.GameBoard.GetLength(0); ++x)
				for (int y = 0; y < LvlDat.GameBoard.GetLength(1); ++y)
					board[new Vector2i(x, y)] = LvlDat.GameBoard[x, y];


			//Set up the ghost home.
			board.GhostHomeMin = LvlDat.GhostHomeMin;
			board.GhostHomeMax = LvlDat.GhostHomeMax;
			Transform ghTr = Prefabs.Instance.CreateGhostHomeSprite(board.GhostHomeMin,
																	board.GhostHomeMax);
			ghTr.parent = containerTr;
			for (int x = board.GhostHomeMin.x; x <= board.GhostHomeMax.x; ++x)
				for (int y = board.GhostHomeMin.y; y <= board.GhostHomeMax.y; ++y)
					board[new Vector2i(x, y)] = CellContents.Nothing;


			//Spawn characters.
			//If in "preview" mode, remove the actual behavor from each character.

			MrsJMan mjm = Prefabs.Instance.CreateMrsJMan(board, LvlDat.MrsJManStart);
			mjm.transform.parent = containerTr;
			if (isPreview)
			{
				GameObject.Destroy(mjm);
			}

			for (int i = 0; i < LvlDat.GhostStarts.Count; ++i)
			{
				Ghost g = Prefabs.Instance.CreateGhost(board, LvlDat.GhostStarts[i]);
				g.transform.parent = containerTr;
				if (isPreview)
				{
					GameObject.Destroy(g);
				}
			}


			//Set up the camera.
			gameCam.transform.position = new Vector3(board.Width * 0.5f, board.Height * 0.5f, 0.0f);
			gameCam.orthographicSize = (board.Height / 2.0f);


			return containerTr;
		}
	}
}