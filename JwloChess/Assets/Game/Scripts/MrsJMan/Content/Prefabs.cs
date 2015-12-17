using System;
using System.Collections.Generic;
using UnityEngine;


namespace MrsJMan
{
	public class Prefabs : Singleton<Prefabs>
	{
		public GameObject MrsJManPrefab, GhostPrefab;


		public MrsJMan CreateMrsJMan(Board board, int inputIndex, Vector2i boardPos)
		{
			GameObject go = Instantiate(MrsJManPrefab);
			go.transform.position = new Vector3((float)boardPos.x + 0.5f,
												(float)boardPos.y + 0.5f,
												0.0f);

			MrsJMan mjm = go.GetComponent<MrsJMan>();
			mjm.GameBoard = board;
			mjm.InputIndex = inputIndex;
			mjm.Speed = Constants.Instance.MrsJManSpeed;

			return mjm;
		}
		public Ghost CreateGhost(Board board, int inputIndex, Vector2i boardPos)
		{
			GameObject go = Instantiate(GhostPrefab);
			go.transform.position = new Vector3((float)boardPos.x + 0.5f,
												(float)boardPos.y + 0.5f,
												0.0f);

			Ghost g = go.GetComponent<Ghost>();
			g.GameBoard = board;
			g.InputIndex = inputIndex;
			g.Speed = Constants.Instance.GhostSpeed;

			return g;
		}
	}
}