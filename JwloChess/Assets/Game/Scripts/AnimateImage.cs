using System;
using UnityEngine;
using Image = UnityEngine.UI.Image;


[RequireComponent(typeof(Image))]
public class AnimateImage: MonoBehaviour
{
	public float FrameLength = 0.2f;
	public Sprite[] SpriteList;


	private Image img;
	private float elapsedTime;


	public int CurrentFrame { get; private set; }


	void Awake()
	{
		CurrentFrame = 0;
		elapsedTime = 0.0f;

		img = GetComponent<Image>();
		img.sprite = SpriteList[CurrentFrame];
	}
	void Update()
	{
		elapsedTime += Time.deltaTime;
		if (elapsedTime > FrameLength)
		{
			elapsedTime -= FrameLength;
			CurrentFrame = (CurrentFrame + 1) % SpriteList.Length;

			img.sprite = SpriteList[CurrentFrame];
		}
	}
}