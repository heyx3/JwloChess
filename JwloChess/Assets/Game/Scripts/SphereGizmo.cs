using UnityEngine;


public class SphereGizmo : MonoBehaviour
{
	public Color Col = Color.white;
	public float Radius = 0.5f;

	public bool OnlyDrawWhenSelected = false;


	void OnDrawGizmos()
	{
		if (!OnlyDrawWhenSelected)
		{
			Gizmos.color = Col;
			Gizmos.DrawSphere(transform.position, Radius);
		}
	}
	void OnDrawGizmosSelected()
	{
		if (OnlyDrawWhenSelected)
		{
			Gizmos.color = Col;
			Gizmos.DrawSphere(transform.position, Radius);
		}
	}
}