using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour 
{
	public float rotationSpeed;
	public Transform pivot;
	public bool clockwise;

	private void FixedUpdate()
	{
		float t = Time.fixedDeltaTime;
		if(clockwise) 
		{
			gameObject.transform.RotateAround(pivot.position, Vector3.forward, -t * rotationSpeed);
		}
		else
		{
			gameObject.transform.RotateAround(pivot.position, Vector3.forward, t * rotationSpeed);
		}
	}
}
