using System;
using UnityEngine;


namespace MrsJMan
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class CellObj : MonoBehaviour
	{
		public SpriteRenderer Spr { get; private set; }


		void Awake()
		{
			Spr = GetComponent<SpriteRenderer>();
		}


		/// <summary>
		/// Sets this cell to use the correct sprite for the given content.
		/// NOTE: If the content is "Wall", use "SetSpriteForWall" instead!
		/// </summary>
		public void SetSpriteFor(CellContents content)
		{
			UnityEngine.Assertions.Assert.IsFalse(content == CellContents.Wall);

			switch (content)
			{
				case CellContents.Nothing:
					gameObject.SetActive(false);
					break;
				case CellContents.Dot:
					gameObject.SetActive(true);
					Spr.sprite = MaterialsAndArt.Instance.DotSprite;
					break;
				case CellContents.Hat:
					gameObject.SetActive(true);
					Spr.sprite = MaterialsAndArt.Instance.RandomHatSprite;
					break;

				default: throw new NotImplementedException(content.ToString());
			}
		}
		/// <summary>
		/// Sets this cell to use the correct wall sprite,
		/// given information about whether the surrounding spaces are filled in.
		/// </summary>
		public void SetSpriteForWall(bool minX, bool maxX, bool minY, bool maxY)
		{
			gameObject.SetActive(true);

			if (minX && maxX && minY && maxY)
				Spr.sprite = MaterialsAndArt.Instance.WallSpriteFull;

			else if (minX && maxX && minY && !maxY)
				Spr.sprite = MaterialsAndArt.Instance.WallSpriteEndMaxY;
			else if (minX && maxX && !minY && maxY)
				Spr.sprite = MaterialsAndArt.Instance.WallSpriteEndMinY;
			else if (minX && !maxX && minY && maxY)
				Spr.sprite = MaterialsAndArt.Instance.WallSpriteEndMaxX;
			else if (!minX && maxX && minY && maxY)
				Spr.sprite = MaterialsAndArt.Instance.WallSpriteEndMinX;

			else if (minX && maxX && !minY && !maxY)
				Spr.sprite = MaterialsAndArt.Instance.WallSpriteEndMinYMaxY;
			else if (minX && !maxX && !minY && maxY)
				Spr.sprite = MaterialsAndArt.Instance.WallSpriteEndMaxXMinY;
			else if (!minX && !maxX && minY && maxY)
				Spr.sprite = MaterialsAndArt.Instance.WallSpriteEndMinXMaxX;
			else if (minX && !maxX && minY && !maxY)
				Spr.sprite = MaterialsAndArt.Instance.WallSpriteEndMaxXY;
			else if (!minX && maxX && !minY && maxY)
				Spr.sprite = MaterialsAndArt.Instance.WallSpriteEndMinXY;
			else if (!minX && maxX && minY && !maxY)
				Spr.sprite = MaterialsAndArt.Instance.WallSpriteEndMinXMaxY;

			else if (minX && !maxX && !minY && !maxY)
				Spr.sprite = MaterialsAndArt.Instance.WallSpriteEndMinYMaxXY;
			else if (!minX && !maxX && !minY && maxY)
				Spr.sprite = MaterialsAndArt.Instance.WallSpriteEndMinXYMaxX;
			else if (!minX && !maxX && minY && !maxY)
				Spr.sprite = MaterialsAndArt.Instance.WallSpriteEndMinXMaxXY;
			else if (!minX && maxX && !minY && !maxY)
				Spr.sprite = MaterialsAndArt.Instance.WallSpriteEndMinXYMaxY;

			else if (!minX && !maxX && !minY && !maxY)
				Spr.sprite = MaterialsAndArt.Instance.WallSpriteEndAll;
			else throw new InvalidOperationException(minX + ", " + minY + ", " + maxX + ", " + maxY);
		}
	}
}