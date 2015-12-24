using System;
using UnityEngine;


public class MaterialsAndArt : Singleton<MaterialsAndArt>
{
	public Sprite DotSprite, ChocolateSprite;

	public Sprite GhostHome;

	public Sprite[] HatSprites;
	public Sprite RandomHatSprite
	{
		get
		{
			return HatSprites[UnityEngine.Random.Range(0, HatSprites.Length)];
		}
	}

	public Sprite WallSpriteFull,

				  WallSpriteEndMinX,
				  WallSpriteEndMinY,
				  WallSpriteEndMaxX,
				  WallSpriteEndMaxY,

				  WallSpriteEndMinXY,
				  WallSpriteEndMinXMaxY,
				  WallSpriteEndMaxXMinY,
				  WallSpriteEndMaxXY,
				  WallSpriteEndMinXMaxX,
				  WallSpriteEndMinYMaxY,

				  WallSpriteEndMinXYMaxX,
				  WallSpriteEndMinXYMaxY,
				  WallSpriteEndMinXMaxXY,
				  WallSpriteEndMinYMaxXY,

				  WallSpriteEndAll;


	public Sprite[] MrsJManSprites, GhostSprites, GhostEyeSprites;
}