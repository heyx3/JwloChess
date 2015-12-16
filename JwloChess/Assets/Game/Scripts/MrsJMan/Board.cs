using System;
using UnityEngine;


namespace MrsJMan
{
	public class Board : MonoBehaviour
	{
		private CellContents[,] gameGrid;
		private CellObj[,] gameGridObjs;


		public CellContents this[Vector2i pos]
		{
			get { return gameGrid[pos.x, pos.y]; }
			set
			{
				if (value == CellContents.Wall)
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
				else
				{
					gameGridObjs[pos.x, pos.y].SetSpriteFor(value);
				}
				
				gameGrid[pos.x, pos.y] = value;
			}
		}


		public int Width { get { return gameGrid.GetLength(0); } }
		public int Height { get { return gameGrid.GetLength(1); } }
		public Vector2i Size { get { return new Vector2i(Width, Height); } }


		public bool IsValidPos(Vector2i pos)
		{
			return pos.x >= 0 && pos.x < Width &&
				   pos.y >= 0 && pos.y < Height;
		}

		public void Reset(Vector2i size, CellContents fillVal = CellContents.Nothing)
		{
			for (int x = 0; x < Width; ++x)
				for (int y = 0; y < Height; ++y)
					Destroy(gameGridObjs[x, y].gameObject);

			gameGrid = new CellContents[size.x, size.y];
			gameGridObjs = new CellObj[size.x, size.y];

			for (Vector2i v = new Vector2i(0, 0); v.x < Width; ++v.x)
			{
				for (v.y = 0; v.y < Height; ++v.y)
				{
					gameGridObjs[v.x, v.y] = CreateCell(v);
					this[v] = fillVal;
				}
			}
		}
		private CellObj CreateCell(Vector2i pos)
		{
			GameObject go = new GameObject("Cell " + pos);
			go.transform.position = new Vector3((float)pos.x + 0.5f, (float)pos.y + 0.5f, 0.0f);

			SpriteRenderer spr = go.AddComponent<SpriteRenderer>();


			CellObj co = go.AddComponent<CellObj>();
			return co;
		}
	}
}