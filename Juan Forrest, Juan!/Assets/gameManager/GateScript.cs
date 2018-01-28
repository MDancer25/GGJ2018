using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour {

    // Use this for initialization
    GameObject[] players;
    bool[] terms = new bool[3];
    public bool levelpassed = false;

    void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        terms[0] = false;
        terms[1] = false;
        terms[2] = false;
    }

    void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        for (int i = 0; i < players.Length; i++) {
            if (Vector3.Distance(transform.position, players[i].transform.position) < 2f) {
                terms[i] = true;
                Debug.Log(i);
            }
        }
        if(terms[0] && terms[1] && terms[2])
        {
            levelpassed = true;
        }
	}

    public void resetGate()
    {
        terms[0] = false;
        terms[1] = false;
        terms[2] = false;
        levelpassed = false;
    }
}
