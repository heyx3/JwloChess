using System;
using UnityEngine;


namespace MrsJMan
{
	public class Board
	{
		public Vector2i GhostHomeMin, GhostHomeMax;
		public Transform GameObjsParent = null;

		public event UnityEngine.Events.UnityAction<GameChar> OnGameWon;


		private CellContents[,] gameGrid = null;
		private CellObj[,] gameGridObjs;


		public CellContents this[Vector2i pos]
		{
			get { return gameGrid[pos.x, pos.y]; }
			set
			{
				//Update the count of the number of dots left on the board.
				if (gameGrid[pos.x, pos.y] != CellContents.Dot &&
					value == CellContents.Dot)
				{
					NumberOfDots += 1;
				}
				else if (gameGrid[pos.x, pos.y] == CellContents.Dot &&
						 value != CellContents.Dot)
				{
					NumberOfDots -= 1;
				}


				//If making a new wall, we have to figure out which sprite to use based on surrounding walls.
				if (value == CellContents.Wall)
				{
					gameGrid[pos.x, pos.y] = value;

					SetWallTile(pos);

					//We also must update surrounding tiles.
					if (IsValidPos(pos.LessX) && this[pos.LessX] == CellContents.Wall)
						SetWallTile(pos.LessX);
					if (IsValidPos(pos.LessY) && this[pos.LessY] == CellContents.Wall)
						SetWallTile(pos.LessY);
					if (IsValidPos(pos.MoreX) && this[pos.MoreX] == CellContents.Wall)
						SetWallTile(pos.MoreX);
					if (IsValidPos(pos.MoreY) && this[pos.MoreY] == CellContents.Wall)
						SetWallTile(pos.MoreY);
				}
				else
				{
					gameGrid[pos.x, pos.y] = value;

					//If removing a wall, update the surrounding wall tiles.
					if (gameGrid[pos.x, pos.y] == CellContents.Wall)
					{
						for (int x = -1; x <= 1; ++x)
						{
							for (int y = -1; y <= 1; ++y)
							{
								if (x != 0 || y != 0)
								{
									Vector2i delta = new Vector2i(x, y);
									if (IsValidPos(pos + delta) && this[pos + delta] == CellContents.Wall)
									{
										SetWallTile(pos + delta);
									}
								}
							}
						}
					}
					
					gameGridObjs[pos.x, pos.y].SetSpriteFor(value);
				}
			}
		}
		private void SetWallTile(Vector2i pos)
		{
			//Figure out whether the neighbors are solid.
			Vector2i lessX = pos.LessX,
					 lessY = pos.LessY,
					 moreX = pos.MoreX,
					 moreY = pos.MoreY;
			bool isLessX = !IsValidPos(lessX) || this[lessX] == CellContents.Wall,
				 isLessY = !IsValidPos(lessY) || this[lessY] == CellContents.Wall,
				 isMoreX = !IsValidPos(moreX) || this[moreX] == CellContents.Wall,
				 isMoreY = !IsValidPos(moreY) || this[moreY] == CellContents.Wall;

			gameGridObjs[pos.x, pos.y].SetSpriteForWall(isLessX, isMoreX, isLessY, isMoreY);
		}

		public CellObj GetCellObj(Vector2i pos) { return gameGridObjs[pos.x, pos.y]; }


		public int Width { get { return gameGrid.GetLength(0); } }
		public int Height { get { return gameGrid.GetLength(1); } }
		public Vector2i Size { get { return new Vector2i(Width, Height); } }

		public int NumberOfDots { get; private set; }


		public bool IsValidPos(Vector2i pos)
		{
			return pos.x >= 0 && pos.x < Width &&
				   pos.y >= 0 && pos.y < Height;
		}

		public bool IsInGhostHome(Vector2i pos)
		{
			return pos.x >= GhostHomeMin.x && pos.x <= GhostHomeMax.x &&
				   pos.y >= GhostHomeMin.y && pos.y <= GhostHomeMax.y;
		}

		public void Reset(Vector2i size, CellContents fillVal = CellContents.Nothing)
		{
			NumberOfDots = 0;

			if (gameGrid != null)
				for (int x = 0; x < Width; ++x)
					for (int y = 0; y < Height; ++y)
						GameObject.Destroy(gameGridObjs[x, y].gameObject);

			gameGrid = new CellContents[size.x, size.y];
			gameGridObjs = new CellObj[size.x, size.y];

			for (Vector2i v = new Vector2i(0, 0); v.x < Width; ++v.x)
			{
				for (v.y = 0; v.y < Height; ++v.y)
				{
					gameGridObjs[v.x, v.y] = CreateCell(v);
					this[v] = fillVal;

					gameGridObjs[v.x, v.y].transform.parent = GameObjsParent;
				}
			}
		}
		private CellObj CreateCell(Vector2i pos)
		{
			GameObject go = new GameObject("Cell " + pos);
			go.transform.position = new Vector3((float)pos.x + 0.5f, (float)pos.y + 0.5f, 0.0f);

			go.AddComponent<SpriteRenderer>();

			CellObj co = go.AddComponent<CellObj>();
			return co;
		}

		public void WinGame(GameChar winner)
		{
			if (OnGameWon != null)
			{
				OnGameWon(winner);
			}
		}
	}
}