using System;
using System.Collections.Generic;
using UnityEngine;


namespace MrsJMan
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class Ghost : GameChar
	{
		public static List<Ghost> AllGhosts = new List<Ghost>();

		
		public SpriteRenderer Spr { get; private set; }
		

		public float AnimFrameLength = 0.15f;
		private float elapsed = 0.0f;
		private int currentFrame = 0;


		void Awake()
		{
			AllGhosts.Add(this);

			Spr = GetComponent<SpriteRenderer>();
		}
		void OnDestroy()
		{
			AllGhosts.Remove(this);
		}

		protected override void Start()
		{
			base.Start();
			
			currentFrame = 0;
			elapsed = 0;
			Spr.sprite = MaterialsAndArt.Instance.GhostSprites[currentFrame];
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

			//Update animation frame.
			elapsed += Time.deltaTime;
			if (elapsed >= AnimFrameLength)
			{
				elapsed -= AnimFrameLength;

				currentFrame = (currentFrame + 1) % MaterialsAndArt.Instance.GhostSprites.Length;
				Spr.sprite = MaterialsAndArt.Instance.GhostSprites[currentFrame];
			}
		}
	}
}