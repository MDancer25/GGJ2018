using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour {

    // Use this for initialization
    GameObject[] players;
    bool[] finished = new bool[3];
    public bool levelpassed = false;

    void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        finished[0] = false;
        finished[1] = false;
        finished[2] = false;
    }

    void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if(finished[0] && finished[1] && finished[2])
        {
            levelpassed = true;
            Debug.Log("leveldone");
        }
	}

    public void resetGate()
    {
        finished[0] = false;
        finished[1] = false;
        finished[2] = false;
        levelpassed = false;
    }

    void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (other.gameObject.name == "Player " + (i+1))
            {
                finished[i] = true;
                Debug.Log(i);
            }
        }
    }
}
