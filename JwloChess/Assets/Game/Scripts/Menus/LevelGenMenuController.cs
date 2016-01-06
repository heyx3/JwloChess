using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using LevelGen;


namespace Menu
{
	public class LevelGenMenuController : Singleton<LevelGenMenuController>
	{
		public RawImage LevelPreview;
		public Camera LevelPreviewCamera;
		public int PixelsPerPreviewBlock = 64;

		public float HoleChance = 0.65f;

		private MrsJMan.Board levelPreviewBoard;
		private RenderTexture levelPreviewTex;
		private Transform levelPreviewContainer = null;

		private float levelPreviewScale;



		public static bool MirrorX = true,
						   MirrorY = false;
		public Toggle MirrorXToggle, MirrorYToggle;

		public static MazeTypes MazeType = MazeTypes.Normal;
		public Text MazeTypeLabel;
		private static string MazeTypeStr
		{
			get
			{
				switch (MazeType)
				{
					case MazeTypes.Normal: return "Normal";
					case MazeTypes.Choppy: return "Choppy";
					case MazeTypes.Straight: return "Straight";
					default: throw new NotImplementedException(MazeType.ToString());
				}
			}
		}

		public static int MazeSizeX = 8,
						  MazeSizeY = 8;
		private static readonly int MinMazeSize = 4;
		public Text MazeSizeXLabel, MazeSizeYLabel;


		void Start()
		{
			levelPreviewScale = LevelPreview.transform.localScale.x;
			UnityEngine.Assertions.Assert.AreApproximatelyEqual(levelPreviewScale,
																LevelPreview.transform.localScale.y,
																0.00001f);

			MirrorXToggle.isOn = MirrorX;
			MirrorYToggle.isOn = MirrorY;

			MazeTypeLabel.text = MazeTypeStr;
			
			MazeSizeXLabel.text = MazeSizeX.ToString();
			MazeSizeYLabel.text = MazeSizeY.ToString();


			//Set up the level preview.
			OnButton_RegenLevel();
		}


		public void OnButton_Play()
		{
			SceneManager.LoadScene("GameScene");
		}
		public void OnButton_Back()
		{
			SceneManager.LoadScene("ControlSelectionMenu");
		}
		public void OnButton_RegenLevel()
		{
			//Destroy the current level preview if it exists.
			if (levelPreviewContainer != null)
			{
				Destroy(levelPreviewContainer.gameObject);
			}
			if (levelPreviewTex != null && levelPreviewTex.IsCreated())
			{
				LevelPreviewCamera.targetTexture = null;
				levelPreviewTex.Release();
			}

			//Generate.
			MrsJMan.GameSettings.GeneratedLevel = BoardGenerator.GenerateLevel(MirrorX, MirrorY,
																			   MazeSizeX, MazeSizeY,
																			   HoleChance, MazeType,
																			   UnityEngine.Random.seed);
			levelPreviewContainer = MrsJMan.GameGenerator.GenerateGame(out levelPreviewBoard,
																	   LevelPreviewCamera, true);

			//Generate the level preview texture.
			levelPreviewTex = new RenderTexture(PixelsPerPreviewBlock * levelPreviewBoard.Width,
												PixelsPerPreviewBlock * levelPreviewBoard.Height,
												16, RenderTextureFormat.Default);
			LevelPreview.texture = levelPreviewTex;
			LevelPreviewCamera.targetTexture = levelPreviewTex;

			//Scale the level preview UI item to have the right width/height.
			RectTransform rtr = LevelPreview.rectTransform;
			rtr.sizeDelta = new Vector2(levelPreviewTex.width, levelPreviewTex.height);
		}

		public void OnValueChanged_MirrorX(bool b)
		{
			MirrorX = MirrorXToggle.isOn;
		}
		public void OnValueChanged_MirrorY(bool b)
		{
			MirrorY = MirrorYToggle.isOn;
		}

		public void OnButton_NextMazeType()
		{
			switch (MazeType)
			{
				case MazeTypes.Normal:
					MazeType = MazeTypes.Choppy;
					break;
				case MazeTypes.Choppy:
					MazeType = MazeTypes.Straight;
					break;
				case MazeTypes.Straight:
					MazeType = MazeTypes.Normal;
					break;
				default: throw new NotImplementedException(MazeType.ToString());
			}

			MazeTypeLabel.text = MazeTypeStr;
		}
		public void OnButton_PreviousMazeType()
		{
			switch (MazeType)
			{
				case MazeTypes.Normal:
					MazeType = MazeTypes.Straight;
					break;
				case MazeTypes.Choppy:
					MazeType = MazeTypes.Normal;
					break;
				case MazeTypes.Straight:
					MazeType = MazeTypes.Choppy;
					break;
				default: throw new NotImplementedException(MazeType.ToString());
			}

			MazeTypeLabel.text = MazeTypeStr;
		}
		
		public void OnButton_NextMazeSizeX()
		{
			MazeSizeX += 2;
			MazeSizeXLabel.text = MazeSizeX.ToString();
		}
		public void OnButton_PreviousMazeSizeX()
		{
			MazeSizeX = Mathf.Max(MinMazeSize, MazeSizeX - 2);
			MazeSizeXLabel.text = MazeSizeX.ToString();
		}
		public void OnButton_NextMazeSizeY()
		{
			MazeSizeY += 2;
			MazeSizeYLabel.text = MazeSizeY.ToString();
		}
		public void OnButton_PreviousMazeSizeY()
		{
			MazeSizeY = Mathf.Max(MinMazeSize, MazeSizeY - 2);
			MazeSizeYLabel.text = MazeSizeY.ToString();
		}
	}
}