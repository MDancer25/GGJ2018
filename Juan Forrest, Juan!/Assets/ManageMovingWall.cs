using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageMovingWall : MonoBehaviour {

    private bool allReached;
	// Use this for initialization
	void Start () {
        allReached = false;
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < transform.childCount; i++)
        {
            string[] name = transform.GetChild(i).name.Split(' ');
            if (name[0] == "Fence")
            {
                if (!transform.GetChild(i).GetComponent<MovingWall>().reached) {
                    allReached = false;
                    return;
                }
            }
        }
        allReached = true;

        if (allReached)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                string[] name = transform.GetChild(i).name.Split(' ');
                if (name[0] == "Fence")
                {
                    transform.GetChild(i).GetComponent<MovingWall>().Switch();
                }
            }
        }
	}
}
