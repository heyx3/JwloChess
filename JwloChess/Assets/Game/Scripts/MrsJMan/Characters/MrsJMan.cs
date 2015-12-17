using System;
using UnityEngine;


namespace MrsJMan
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class MrsJMan : GameChar
	{
		public static MrsJMan Instance { get; private set; }

		
		public SpriteRenderer Spr { get; private set; }
		

		public float AnimFrameLength = 0.25f;
		private float elapsed = 0.0f;
		private int currentFrame = 0;


		void Awake()
		{
			UnityEngine.Assertions.Assert.IsNull(Instance);
			Instance = this;

			Spr = GetComponent<SpriteRenderer>();
		}
		protected override void Start()
		{
			base.Start();
			
			currentFrame = 0;
			elapsed = 0;
			Spr.sprite = MaterialsAndArt.Instance.MrsJManSprites[currentFrame];
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
					//TODO: Put hat on char.
					break;

				case CellContents.Nothing:
					break;

				default: throw new NotImplementedException(inCell.ToString());
			}
		}
		protected override void Update()
		{
			base.Update();

			//Update animation frame.
			elapsed += Time.deltaTime;
			if (elapsed >= AnimFrameLength)
			{
				elapsed -= AnimFrameLength;

				currentFrame = (currentFrame + 1) % MaterialsAndArt.Instance.MrsJManSprites.Length;
				Spr.sprite = MaterialsAndArt.Instance.MrsJManSprites[currentFrame];
			}

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
	}
}