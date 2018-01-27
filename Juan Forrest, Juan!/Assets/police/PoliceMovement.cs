using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(PoliceController))]
public class PoliceMovement : MonoBehaviour {

	[SerializeField] float stopDistance = 1;

	PoliceController policeController;
	AICharacterControl aiController;
	//public GameObject currentTarget;


	// Use this for initialization
	void Start () 
	{
		aiController = GetComponent<AICharacterControl> ();
		policeController = GetComponent<PoliceController> ();

	}

	void Update ()
	{
		CheckDistanceToTarget ();
	}


	//TODO
	//checks if it is too close to a player ->  attacks and eventually game over conditions?
	private void CheckDistanceToTarget()
	{
		if (TooCloseToTheTarget()) //distance between police and the currently target player
		{
			aiController.SetTarget (this.transform);
		}
		else
			aiController.SetTarget (policeController.currentTarget.transform);
	}

	private bool  TooCloseToTheTarget()
	{
		return Vector3.Distance (transform.position, policeController.currentTarget.transform.position) < stopDistance;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (transform.position, stopDistance);

		if(policeController.currentTarget != null)
			Gizmos.DrawLine (transform.position, policeController.currentTarget.transform.position);
	}
}
