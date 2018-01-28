using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PoliceController : MonoBehaviour {

	public bool followPassport;
	public GameObject currentTarget;

	GameObject[] listOfTargets;
	int numberOfJuans;

	// Use this for initialization
	void Start () {
		listOfTargets = GameObject.FindGameObjectsWithTag ("Player");

		PlayerController.notifyPlayerPickedPassport += ChangeTarget;	//subscribe to the notification of ticket changing hands

		Assert.IsNotNull (listOfTargets);
		Assert.IsTrue (listOfTargets.Length != 0);

		numberOfJuans = listOfTargets.Length;
		ChooseTarget ();
	}

	// Update is called once per frame
	void Update () {

	}

	void ChangeTarget(GameObject passportHolder)
	{
		if (passportHolder.gameObject.CompareTag ("Passport")) //if the passport is being thrown, go after if
			currentTarget = passportHolder;
		else {
			listOfTargets = GameObject.FindGameObjectsWithTag ("Player");
			int indexOfPassportHolder = -1; // used  to prevent this from being the next target of the police

			for (int i = 0; i < listOfTargets.Length; i++)
				if (listOfTargets [i].name.Equals (passportHolder.name))	//if the new holder has the same name of the guy in this position, he should be excluded from the pool of possible targets
				indexOfPassportHolder = i;


			int randIndex = Random.Range (0, numberOfJuans);
			while (randIndex == indexOfPassportHolder)
				randIndex = Random.Range (1, numberOfJuans);	
			currentTarget = listOfTargets [randIndex];
		}
	}

	//TODO
	//chooses who to follow
	private void ChooseTarget()
	{
		int randIndex = Random.Range (0, numberOfJuans);
		currentTarget = listOfTargets [randIndex];
	}
}
