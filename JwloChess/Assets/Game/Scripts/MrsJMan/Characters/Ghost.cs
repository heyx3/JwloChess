using System;
using System.Collections.Generic;
using UnityEngine;


namespace MrsJMan
{
	[RequireComponent(typeof(AnimateSprite))]
	public class Ghost : GameChar
	{
		public static List<Ghost> AllGhosts = new List<Ghost>();

		
		public SpriteRenderer Spr { get; private set; }
		public AnimateSprite Anim { get; private set; }

		public bool IsInHatMode { get; private set; }
		public bool IsDead { get; private set; }
		

		public float HatBlinkInterval = 0.15f;

		public Color NormalCol = Color.white,
					 HatModeCol = Color.blue;


		void Awake()
		{
			AllGhosts.Add(this);

			InputIndex = GameSettings.GhostInput;

			IsDead = false;
			IsInHatMode = false;

			Spr = GetComponent<SpriteRenderer>();
			Anim = GetComponent<AnimateSprite>();
		}
		void OnDestroy()
		{
			AllGhosts.Remove(this);
		}

		protected override void Update()
		{
			base.Update();

			//If this ghost is touching the player, either kill her or get eaten by her.
			Vector2 pos = (Vector2)MyTr.position;
			Vector2 playerPos = (Vector2)MrsJMan.Instance.MyTr.position;
			if ((pos - playerPos).sqrMagnitude <
				(Constants.Instance.GhostHitRadius * Constants.Instance.GhostHitRadius))
			{
				if (IsInHatMode)
				{
					IsDead = true;
					Anim.SpriteList = MaterialsAndArt.Instance.GhostEyeSprites;
					Anim.Reset();
					
				}
				else
				{
					GameBoard.WinGame(this);
				}
			}
		}

		protected override bool CanEnterCell(Vector2i cell)
		{
			if (GameBoard.IsInGhostHome(cell))
			{
				return IsDead;
			}
			else
			{
				return base.CanEnterCell(cell);
			}
		}
		protected override void OnEnteredCell(Vector2i cell)
		{
			if (GameBoard.IsInGhostHome(cell))
			{
				IsDead = false;
				Anim.SpriteList = MaterialsAndArt.Instance.GhostSprites;
				Anim.Reset();
			}
		}

		public override void StartHat(float time)
		{
			base.StartHat(time);

			IsInHatMode = true;
			Spr.color = HatModeCol;
		}
		protected override void OnNearEndHat(float timeLeft)
		{
			int currentIncrement = (int)(timeLeft / HatBlinkInterval);
			Spr.color = (currentIncrement % 2 == 0) ?
							NormalCol :
							HatModeCol;
		}
		protected override void OnEndHat()
		{
			Spr.color = NormalCol;
			IsInHatMode = false;
		}
	}
}