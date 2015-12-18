﻿using System;
using UnityEngine;


namespace MrsJMan
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class MrsJMan : GameChar
	{
		public static MrsJMan Instance { get; private set; }

		
		public SpriteRenderer Spr { get; private set; }
		

		public float HatBlinkInterval = 0.15f;
		public SpriteRenderer HatChildSprite;


		void Awake()
		{
			UnityEngine.Assertions.Assert.IsNull(Instance);
			Instance = this;

			InputIndex = GameSettings.MrsJManInput;

			Spr = GetComponent<SpriteRenderer>();
		}
		
		protected override void OnEnteredCell(Vector2i cell)
		{
			CellContents inCell = GameBoard[cell];

			switch (inCell)
			{
				case CellContents.Dot:
					GameBoard[cell] = CellContents.Nothing;
					if (GameBoard.NumberOfDots <= 0)
					{
						//TODO: MrsJMan won.
					}
					break;

				case CellContents.Hat:

					HatChildSprite.sprite = GameBoard.GetCellObj(cell).Spr.sprite;
					GameBoard[cell] = CellContents.Nothing;

					StartHat(Constants.Instance.HatUseTime);
					for (int i = 0; i < Ghost.AllGhosts.Count; ++i)
						Ghost.AllGhosts[i].StartHat(Constants.Instance.HatUseTime);
				break;

				case CellContents.Nothing:
					break;

				default: throw new NotImplementedException(inCell.ToString());
			}
		}
		protected override void Update()
		{
			base.Update();

			//Scale/Rotate animation to match direction.
			Vector3 scale = MyTr.localScale;
			Vector3 rot = Vector3.zero;
			Vector2i moveDir = CurrentMoveDir;
			if (moveDir == new Vector2i(-1, 0))
			{
				scale.x = -1.0f;
			}
			else if (moveDir == new Vector2i(1, 0))
			{
				scale.x = 1.0f;
			}
			else if (moveDir == new Vector2i(0, -1))
			{
				rot.z = -90.0f * scale.x;
			}
			else if (moveDir == new Vector2i(0, 1))
			{
				rot.z = 90.0f * scale.x;
			}
			MyTr.localScale = scale;
			MyTr.eulerAngles = rot;
		}

		public override void StartHat(float time)
		{
			base.StartHat(time);

			HatChildSprite.enabled = true;
		}
		protected override void OnNearEndHat(float timeLeft)
		{
			int currentIncrement = (int)(timeLeft / HatBlinkInterval);
			HatChildSprite.enabled = (currentIncrement % 2 == 0);
		}
		protected override void OnEndHat()
		{
			HatChildSprite.enabled = false;
		}
	}
}