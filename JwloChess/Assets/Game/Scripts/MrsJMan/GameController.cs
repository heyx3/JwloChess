using System;
using System.Collections.Generic;
using UnityEngine;


namespace MrsJMan
{
	public class GameController : MonoBehaviour
	{
		public Board GameBoard;

		void Start()
		{
			GameGenerator.GenerateGame(out GameBoard, Camera.main, false);
		}
	}
}
