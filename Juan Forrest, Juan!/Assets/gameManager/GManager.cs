using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(PoliceMovement))]
public class GManager : MonoBehaviour {

	PoliceMovement policeMovement;

	//TODO only notifications of one cop will be sent
	void Start ()
	{
		GameObject[] policeOfficers = GameObject.FindGameObjectsWithTag ("Police");
		policeMovement = policeOfficers [0].GetComponent<PoliceMovement> (); 

		Assert.IsNotNull (policeMovement);

		policeMovement.notifyCloseToPlayer += policeCloseToPlayer; 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void policeCloseToPlayer(GameObject closePlayer)
	{
		Debug.Log (closePlayer.name);
	}
}
