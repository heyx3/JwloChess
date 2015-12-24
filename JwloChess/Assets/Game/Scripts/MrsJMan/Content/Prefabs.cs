using System;
using System.Collections.Generic;
using UnityEngine;


namespace MrsJMan
{
	public class Prefabs : Singleton<Prefabs>
	{
		public GameObject MrsJManPrefab, GhostPrefab;


		public MrsJMan CreateMrsJMan(Board board, Vector2i boardPos)
		{
			GameObject go = Instantiate(MrsJManPrefab);
			go.transform.position = new Vector3((float)boardPos.x + 0.5f,
												(float)boardPos.y + 0.5f,
												0.0f);

			MrsJMan mjm = go.GetComponent<MrsJMan>();
			mjm.GameBoard = board;
			mjm.Speed = Constants.Instance.MrsJManSpeed;

			return mjm;
		}
		public Ghost CreateGhost(Board board, Vector2i boardPos)
		{
			GameObject go = Instantiate(GhostPrefab);
			go.transform.position = new Vector3((float)boardPos.x + 0.5f,
												(float)boardPos.y + 0.5f,
												0.0f);

			Ghost g = go.GetComponent<Ghost>();
			g.GameBoard = board;
			g.Speed = Constants.Instance.GhostSpeed;

			return g;
		}

		public Transform CreateGhostHomeSprite(Vector2i min, Vector2i max)
		{
			GameObject go = new GameObject("Ghost Home Sprite");
			Transform tr = go.transform;

			SpriteRenderer spr = go.AddComponent<SpriteRenderer>();
			spr.sprite = MaterialsAndArt.Instance.GhostHome;

			Vector2 minF = new Vector2(min.x + 0.5f, min.y + 0.5f),
					maxF = new Vector2(max.x + 0.5f, max.y + 0.5f);
			tr.position = new Vector3((maxF.x + minF.x) * 0.5f,
									  (maxF.y + minF.y) * 0.5f,
									  0.0f);
			
			Vector2 scale = maxF - minF + new Vector2(1.0f, 1.0f);
			scale = new Vector2(scale.x / (MaterialsAndArt.Instance.GhostHome.bounds.extents.x * 2.0f),
								scale.y / (MaterialsAndArt.Instance.GhostHome.bounds.extents.y * 2.0f));
			tr.localScale = new Vector3(scale.x, scale.y, 1.0f);

			return tr;
		}
	}
}