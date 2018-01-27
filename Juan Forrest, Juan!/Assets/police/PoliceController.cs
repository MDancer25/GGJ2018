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
		Assert.IsNotNull (listOfTargets);

		ChooseTarget ();
		numberOfJuans = listOfTargets.Length;
	}

	// Update is called once per frame
	void Update () {

	}

	//TODO
	//chooses who to follow
	private void ChooseTarget()
	{
		int randIndex = Random.Range (0, numberOfJuans);
		currentTarget = listOfTargets [randIndex];
	}
}
