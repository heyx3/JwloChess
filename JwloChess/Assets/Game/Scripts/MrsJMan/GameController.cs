using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MrsJMan
{
	public class GameController : Singleton<GameController>
	{
		public Board GameBoard;

		private Coroutine chocoSpawnCoroutine = null;
		private bool runningEndgame = false;


		void Start()
		{
			GameGenerator.GenerateGame(out GameBoard, Camera.main, false);

			chocoSpawnCoroutine = StartCoroutine(ChocolateSpawnCoroutine());
		}
		void Update()
		{
			if (!runningEndgame && GameBoard.NumberOfDots == 0)
			{
				StartCoroutine(EndGameCoroutine(true));
			}

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				SceneManager.LoadScene("MainMenu");
			}
		}


		public void KillMrsJMan()
		{
			if (!runningEndgame)
			{
				StartCoroutine(EndGameCoroutine(false));
			}
		}
		private IEnumerator EndGameCoroutine(bool mrsJManWon)
		{
			runningEndgame = true;
			Transform boardContainer = MrsJMan.Instance.MyTr.parent;

			//Remove animation and movement from game characters.
			GameObject go = MrsJMan.Instance.gameObject;
			Destroy(MrsJMan.Instance.GetComponent<MrsJMan>());
			Destroy(MrsJMan.Instance.GetComponent<AnimateSprite>());
			foreach (Ghost g in Ghost.AllGhosts)
			{
				go = g.gameObject;
				Destroy(go.GetComponent<Ghost>());
				Destroy(go.GetComponent<AnimateSprite>());
			}

			//Get the objects that should blink to indicate the winner.
			List<GameObject> toHide = new List<GameObject>();
			for (int i = 0; i < boardContainer.childCount; ++i)
			{
				Transform tr = boardContainer.GetChild(i);
				if ((!mrsJManWon && tr == MrsJMan.Instance.MyTr) ||
					(mrsJManWon && tr != MrsJMan.Instance.MyTr))
				{
					toHide.Add(tr.gameObject);
				}
			}

			//Make them blink.
			bool visible = true; 
			for (int i = 0; i < 8; ++i)
			{
				yield return new WaitForSeconds(0.6f);

				foreach (GameObject g in toHide)
					g.SetActive(visible);

				visible = !visible;
			}

			SceneManager.LoadScene("LevelSelectionMenu");
		}

		public void CollectChocolate()
		{
			StopCoroutine(chocoSpawnCoroutine);
			chocoSpawnCoroutine = StartCoroutine(ChocolateSpawnCoroutine());

			StartCoroutine(FreezeGhostCoroutine());
		}
		private IEnumerator ChocolateSpawnCoroutine()
		{
			while (true)
			{
				GameBoard.DespawnChocolate();

				yield return new WaitForSeconds(Constants.Instance.ChocolateSpawnInterval);

				GameBoard.SpawnChocolate();

				yield return new WaitForSeconds(Constants.Instance.ChocolateLife -
												Constants.Instance.ChocolateBlinkTimeLeft);

				const int nBlinks = 10;
				float interval = Constants.Instance.ChocolateBlinkTimeLeft / (float)nBlinks;

				for (int i = 0; i < nBlinks; ++i)
				{
					GameBoard.Chocolate.gameObject.SetActive(i % 2 == 1);
					yield return new WaitForSeconds(interval);
				}
			}
		}
		private IEnumerator FreezeGhostCoroutine()
		{
			List<float> ghostSpeeds = new List<float>();
			for (int i = 0; i < Ghost.AllGhosts.Count; ++i)
			{
				ghostSpeeds.Add(Ghost.AllGhosts[i].Speed);
				Ghost.AllGhosts[i].Speed = 0.0f;
			}

			yield return new WaitForSeconds(Constants.Instance.GhostFreezeTime);

			for (int i = 0; i < Ghost.AllGhosts.Count; ++i)
				Ghost.AllGhosts[i].Speed = ghostSpeeds[i];
		}
	}
}