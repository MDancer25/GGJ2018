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
    static int level = 1;
    GateScript gate;
    public GameObject[] traps;
    GameObject SlowTrap;
    GameObject StuckTrap;
    GameObject Bomb;
    GameObject WalkFaster;

    void Awake()
    {
        traps = new GameObject[4];
        traps[0] = Resources.Load("Slow Trap") as GameObject;
        traps[1] = Resources.Load("Stuck Trap") as GameObject;
        traps[2] = Resources.Load("Bomb") as GameObject;
        traps[3] = Resources.Load("WalkFasterPU") as GameObject;

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
            policeMovement[i].aiController.agent.velocity *= (level / 10 + 1);
		}
        for (int i = 0; i < Random.Range(level - 1, level + 1); i++){
            SpawnTrap();
        }
		Assert.IsNotNull (policeMovement);
        gate = Gate.GetComponent<GateScript>();
        if (Random.Range(0, level) > 2)
        {
            TrapWall.SetActive(true);
            if (Random.Range(0, level) > 4)
                TrapWall2.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (gate != null && gate.levelpassed)
        {
            Debug.Log("GM IF");
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
        TrapWall.SetActive(false);
        TrapWall2.SetActive(false);
    }

    void SpawnTrap()
    {
        Instantiate(traps[Random.Range(0, traps.Length)], new Vector3(Random.Range(-24, 24), 0, Random.Range(-24, 7)), Quaternion.identity);
    }
}
