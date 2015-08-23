using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Patrol : MonoBehaviour 
{

	public float moveSpeed;

	public bool looping = true;
	public bool bounce = true;
	public bool closedCircuit = false;

	public List<Transform> startingTargets = new List<Transform> ();

	private int currentTargetIndex = 1;
	private bool patrolling;


	private IEnumerator PatrolPoints(List<Transform> currentTargets)
	{
		for(int i = 0; i < currentTargets.Count - 1; i++)
		{
			float desiredTravelTime = Vector3.Distance(currentTargets[i].position,
			                                           currentTargets[i+1].position) / moveSpeed;
			float elapsedTime = 0f;
			
			while(elapsedTime < desiredTravelTime)
			{
				transform.position = Vector3.Lerp(currentTargets[i].position,
				                                  currentTargets[i+1].position,
				                                  elapsedTime/desiredTravelTime);
				elapsedTime += Time.deltaTime;
				yield return null;
			}
		}

		ManageMovement(currentTargets);
	}


	private void ManageMovement(List<Transform> currentTargets)
	{
		if(looping)
		{
			if(bounce)
			{
				if(closedCircuit)
				{
					//need to loop back to the beginning
					transform.position = currentTargets[currentTargets.Count - 1].position;
					
					List<Transform> newList = new List<Transform> ();
					newList.Add(currentTargets[currentTargets.Count - 1]);
					currentTargets.RemoveAt (currentTargets.Count - 1);
					newList.AddRange (currentTargets);
					
					StartCoroutine(PatrolPoints(newList));
				}
				else
				{
					//need to go back the way we came
					transform.position = currentTargets[currentTargets.Count - 1].position;
					currentTargets.Reverse ();
					StartCoroutine(PatrolPoints(currentTargets));
				}
			}
			else
			{
				transform.position = currentTargets[0].position;
				StartCoroutine(PatrolPoints(currentTargets));
			}

		}
		else
		{
			//dont need to do anything i dont think?
		}
	}


	private void Awake()
	{
		StartCoroutine(PatrolPoints(startingTargets));
	}
}
