using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Menu
{
	public class InputSelectionMenu : MonoBehaviour
	{
		public Text TitleText;
		public float WaitTimeBetweenPlayers = 1.0f;

		private bool findingMrsJ = true;
		private float timeTillAllowed = -1.0f;


		void Start()
		{
			TitleText.text = "Press a control key for: Mrs. J-MAN";
		}
		void Update()
		{
			if (timeTillAllowed <= 0.0f)
			{
				int input = InputManager.Instance.GetFirstUsedInput();
				if (input != -1)
				{
					if (findingMrsJ)
					{
						MrsJMan.GameSettings.MrsJManInput = input;
						findingMrsJ = false;
						TitleText.text = "Press a control key for: Ghost";
						timeTillAllowed = WaitTimeBetweenPlayers;
					}
					else
					{
						MrsJMan.GameSettings.GhostInput = input;
						UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelectionMenu");
					}
				}
			}
			else
			{
				timeTillAllowed -= Time.deltaTime;
			}
		}
	}
}