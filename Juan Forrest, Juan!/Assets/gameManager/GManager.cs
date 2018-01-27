using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PoliceMovement))]
public class GManager : MonoBehaviour {

	PoliceMovement[] policeMovement;

	//TODO only notifications of one cop will be sent
	void Start ()
	{
		GameObject[] policeOfficers = GameObject.FindGameObjectsWithTag ("Police");
		policeMovement = new PoliceMovement[policeOfficers.Length];

		for (int i = 0; i < policeOfficers.Length; i++) 
		{
			policeMovement[i] = policeOfficers [i].GetComponent<PoliceMovement> ();
			policeMovement[i].notifyCloseToPlayer += policeCloseToPlayer; 
		}
		Assert.IsNotNull (policeMovement); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//what happens when the police is close to the player
	void policeCloseToPlayer(GameObject closePlayer)
	{
		Scene currentScene = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (currentScene.name);
		Debug.Log ("policeCloseToPlayer");
	}
}
