using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

//[RequireComponent(typeof(PoliceMovement))]
public class GManager : MonoBehaviour {

	PoliceMovement[] policeMovement;
    GameObject TrapWall;
    GameObject TrapWall2;
    GameObject Gate;
    int level = 0;
    GateScript gate;

    void Awake()
    {
        TrapWall = GameObject.Find("TrapWall");
        TrapWall2 = GameObject.Find("TrapWall2");
        Gate = GameObject.Find("GateCollision");
        TrapWall.SetActive(false);
        TrapWall2.SetActive(false);
    }

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
        gate = gameObject.GetComponent<GateScript>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (gate != null && gate.levelpassed)
        {
            level++;
            gate.resetGate();
            levelWon();
        }
	}

	//what happens when the police is close to the player
	void policeCloseToPlayer(GameObject closePlayer)
	{
		Scene currentScene = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (currentScene.name);
		Debug.Log ("policeCloseToPlayer");
	}


    void levelWon()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        Debug.Log("Victory");
    }
}
