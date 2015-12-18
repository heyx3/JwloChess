using System;
using System.Collections.Generic;
using UnityEngine;


namespace MrsJMan
{
	public class Ghost : GameChar
	{
		public static List<Ghost> AllGhosts = new List<Ghost>();

		
		public SpriteRenderer Spr { get; private set; }
		

		public float HatBlinkInterval = 0.15f;

		public Color NormalCol = Color.white,
					 HatModeCol = Color.blue;


		void Awake()
		{
			AllGhosts.Add(this);

			InputIndex = GameSettings.GhostInput;

			Spr = GetComponent<SpriteRenderer>();
		}
		void OnDestroy()
		{
			AllGhosts.Remove(this);
		}

		protected override void Update()
		{
			base.Update();

			//If this ghost is touching the player, kill her.
			Vector2 pos = (Vector2)MyTr.position;
			Vector2 playerPos = (Vector2)MrsJMan.Instance.MyTr.position;
			if ((pos - playerPos).sqrMagnitude <
				(Constants.Instance.GhostHitRadius * Constants.Instance.GhostHitRadius))
			{
				//TODO: Kill her.
			}
		}


		public override void StartHat(float time)
		{
			base.StartHat(time);

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
		}
	}
}