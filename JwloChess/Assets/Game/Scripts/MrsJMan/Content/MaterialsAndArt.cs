using System;
using UnityEngine;


public class MaterialsAndArt : Singleton<MaterialsAndArt>
{
	public Sprite DotSprite;

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
}