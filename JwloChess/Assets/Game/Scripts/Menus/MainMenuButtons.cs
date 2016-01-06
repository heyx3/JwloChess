using UnityEngine;
using UnityEngine.SceneManagement;


namespace Menu
{
	public class MainMenuButtons : MonoBehaviour
	{
		public void OnPlay()
		{
			SceneManager.LoadScene("ControlSelectionMenu");
		}
		public void OnQuit()
		{
			Application.Quit();
		}
	}
}