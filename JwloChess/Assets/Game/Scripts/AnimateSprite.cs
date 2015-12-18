using System;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class AnimateSprite : MonoBehaviour
{
	public float FrameLength = 0.2f;
	public Sprite[] SpriteList;


	private SpriteRenderer spr;
	private float elapsedTime;


	public int CurrentFrame { get; private set; }


	void Awake()
	{
		CurrentFrame = 0;
		elapsedTime = 0.0f;

		spr = GetComponent<SpriteRenderer>();
		spr.sprite = SpriteList[CurrentFrame];
	}
	void Update()
	{
		elapsedTime += Time.deltaTime;
		if (elapsedTime > FrameLength)
		{
			elapsedTime -= FrameLength;
			CurrentFrame = (CurrentFrame + 1) % SpriteList.Length;

			spr.sprite = SpriteList[CurrentFrame];
		}
	}
}