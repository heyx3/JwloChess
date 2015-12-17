using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
public class InputManager : Singleton<InputManager>
{
	/// <summary>
	/// The number of unique means of providing input.
	/// </summary>
	public const int N_CONTROLS = 7;


	public Vector2i[] InputValues = new Vector2i[N_CONTROLS];


	/// <summary>
	/// Returns the index of the first item in "InputValues" that has a non-zero input.
	/// Returns -1 if all inputs are zero.
	/// </summary>
	public int GetFirstUsedInput()
	{
		for (int i = 0; i < N_CONTROLS; ++i)
		{
			if (InputValues[i].x != 0 || InputValues[i].y != 0)
			{
				return i;
			}
		}

		return -1;
	}


	void Update()
	{
		InputValues[0] = Vector2i.Zero;
		InputValues[0].x = (Input.GetKey(KeyCode.A) ? -1 : 0) +
						   (Input.GetKey(KeyCode.D) ? 1 : 0);
		InputValues[0].y = (Input.GetKey(KeyCode.S) ? -1 : 0) +
						   (Input.GetKey(KeyCode.W) ? 1 : 0);
		
		InputValues[1] = Vector2i.Zero;
		InputValues[1].x = (Input.GetKey(KeyCode.J) ? -1 : 0) +
						   (Input.GetKey(KeyCode.L) ? 1 : 0);
		InputValues[1].y = (Input.GetKey(KeyCode.K) ? -1 : 0) +
						   (Input.GetKey(KeyCode.I) ? 1 : 0);
		
		InputValues[2] = Vector2i.Zero;
		InputValues[2].x = (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0) +
						   (Input.GetKey(KeyCode.RightArrow) ? 1 : 0);
		InputValues[2].y = (Input.GetKey(KeyCode.DownArrow) ? -1 : 0) +
						   (Input.GetKey(KeyCode.UpArrow) ? 1 : 0);

		for (int i = 0; i < 4; ++i)
		{
			InputValues[i + 3] = Vector2i.Zero;
			InputValues[i + 3].x = Mathf.RoundToInt(Input.GetAxis("Gamepad " + (i + 1) + " X"));
			InputValues[i + 3].y = Mathf.RoundToInt(Input.GetAxis("Gamepad " + (i + 1) + " Y"));
		}
	}
}