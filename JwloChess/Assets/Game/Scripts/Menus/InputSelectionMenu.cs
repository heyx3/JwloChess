using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Menu
{
	public class InputSelectionMenu : MonoBehaviour
	{
		public Text TitleText;

		private bool findingMrsJ;


		void Start()
		{
			TitleText.text = "Press a control key for: Mrs. J-MAN";
		}
		void Update()
		{
			int input = InputManager.Instance.GetFirstUsedInput();
			if (input != -1)
			{
				if (findingMrsJ)
				{
					MrsJMan.GameSettings.MrsJManInput = input;
					findingMrsJ = false;
					TitleText.text = "Press a control key for: Ghost";
				}
				else
				{
					MrsJMan.GameSettings.GhostInput = input;
					UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelectionMenu");
				}
			}
		}
	}
}