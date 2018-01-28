using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(PoliceController))]
public class PoliceMovement : MonoBehaviour {

	[SerializeField] float stopDistance = 1;

	public delegate  void OnCLoseToPlayer (GameObject closePlayer);	// new delegate type
	public event OnCLoseToPlayer notifyCloseToPlayer;				// instantiate an observer set

	PoliceController policeController;
	AICharacterControl aiController;

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
		if (TooCloseToTheTarget ()) { 	//distance between police and the currently target player
			notifyCloseToPlayer (policeController.currentTarget);		//notify all observing classes that the player is close
			aiController.SetTarget (this.transform);
		} else 
		{
			aiController.SetTarget (policeController.currentTarget.transform);
		}
	}

	private bool  TooCloseToTheTarget()
	{
		return Vector3.Distance (transform.position, policeController.currentTarget.transform.position) < stopDistance;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Player") || other.gameObject.CompareTag("Passport"))
		{
			notifyCloseToPlayer(other.gameObject);
		}

	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (transform.position, stopDistance);

		if(policeController.currentTarget != null)
			Gizmos.DrawLine (transform.position, policeController.currentTarget.transform.position);
	}
}
