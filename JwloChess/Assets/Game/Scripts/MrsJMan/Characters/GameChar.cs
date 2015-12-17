using System;
using System.Collections.Generic;
using UnityEngine;


namespace MrsJMan
{
	public class GameChar : MonoBehaviour
	{
		public Board GameBoard;

		public int InputIndex;

		[NonSerialized]
		public float Speed;

		private Vector2 oldPos, newPos;
		private float posLerp = 0.0f;
		private float timeTillHatDone = -1.0f;


		public Transform MyTr { get; private set; }

		public Vector2i CurrentInput { get { return InputManager.Instance.InputValues[InputIndex]; } }
		public Vector2i CurrentMoveDir
		{
			get
			{
				return new Vector2i(Mathf.RoundToInt(newPos.x - oldPos.x),
									Mathf.RoundToInt(newPos.y - oldPos.y));
			}
		}


		public virtual void StartHat(float time)
		{
			timeTillHatDone = time;
		}
		protected virtual void OnNearEndHat(float timeLeft) { }
		protected virtual void OnEndHat() { }

		protected virtual void Start()
		{
			MyTr = transform;

			newPos = (Vector2)MyTr.position;
			ChangeDirRandomly();
		}
		protected virtual void Update()
		{
			Vector2i input = CurrentInput;
			Vector2 moveDelta = (newPos - oldPos);
			Vector2i moveDeltaI = new Vector2i(Mathf.RoundToInt(moveDelta.x),
											   Mathf.RoundToInt(moveDelta.y));

			//If the player is pushing the input in the opposite direction, immediately reverse.
			if ((moveDeltaI.x != 0 && input.x == -moveDeltaI.x) ||
				(moveDeltaI.y != 0 && input.y == -moveDeltaI.y))
			{
				Vector2 temp = oldPos;
				oldPos = newPos;
				newPos = temp;

				posLerp = 1.0f - posLerp;
			}
			//Otherwise, move forward like normal.
			else
			{
				posLerp += Time.deltaTime * Speed;

				//If we hit the next cell, see if we should change directions.
				if (posLerp >= 1.0f)
				{
					posLerp -= 1.0f;

					Vector2i newPosI = new Vector2i((int)newPos.x,
													(int)newPos.y);


					//If the player is pushing towards an open direction, use that.
					if (input.x < 0 && CanMoveTo(newPosI.LessX))
					{
						oldPos = newPos;
						newPos = oldPos - new Vector2(1.0f, 0.0f);
					}
					else if (input.x > 0 && CanMoveTo(newPosI.MoreX))
					{
						oldPos = newPos;
						newPos = oldPos + new Vector2(1.0f, 0.0f);
					}
					else if (input.y < 0 && CanMoveTo(newPosI.LessY))
					{
						oldPos = newPos;
						newPos = oldPos - new Vector2(0.0f, 1.0f);
					}
					else if (input.y > 0 && CanMoveTo(newPosI.MoreY))
					{
						oldPos = newPos;
						newPos = oldPos + new Vector2(0.0f, 1.0f);
					}
					//Otherwise, if the current direction is now blocked, change randomly.
					else if (!CanMoveTo(newPosI + moveDeltaI))
					{
						ChangeDirRandomly();
					}
					//Otherwise, keep going.
					else
					{
						oldPos = newPos;
						newPos += moveDelta;
					}

					OnEnteredCell(new Vector2i((int)oldPos.x,
											   (int)oldPos.y));
				}

				//Update the position of this character.
				MyTr.position = Vector3.LerpUnclamped(oldPos, newPos, posLerp);
			}

			//Update the time until the hat effect is done.
			if (timeTillHatDone > 0.0f)
			{
				timeTillHatDone -= Time.deltaTime;
				if (timeTillHatDone <= 0.0f)
				{
					OnEndHat();
				}
				else if (timeTillHatDone <= Constants.Instance.HatNearEndTime)
				{
					OnNearEndHat(timeTillHatDone);
				}
			}
		}
		
		
		private bool CanMoveTo(Vector2i pos)
		{
			return GameBoard.IsValidPos(pos) && GameBoard[pos] != CellContents.Wall;
		}
	
		private List<Vector2i> moveOptions = new List<Vector2i>(4);
		private void ChangeDirRandomly()
		{
			oldPos = newPos;
			Vector2i posI = new Vector2i((int)oldPos.x, (int)oldPos.y);
			
			//Find all the available open spaces.
			moveOptions.Clear();
			if (CanMoveTo(posI.LessX))
			{
				moveOptions.Add(posI.LessX);
			}
			if (CanMoveTo(posI.LessY))
			{
				moveOptions.Add(posI.LessY);
			}
			if (CanMoveTo(posI.MoreX))
			{
				moveOptions.Add(posI.MoreX);
			}
			if (CanMoveTo(posI.MoreY))
			{
				moveOptions.Add(posI.MoreY);
			}
			
			UnityEngine.Assertions.Assert.IsTrue(moveOptions.Count > 0);

			Vector2i tempPos = moveOptions[UnityEngine.Random.Range(0, moveOptions.Count)];
			newPos = new Vector2((float)tempPos.x + 0.5f, (float)tempPos.y + 0.5f);
			posLerp = 0.0f;
		}


		protected virtual void OnEnteredCell(Vector2i cell) { }
	}
}