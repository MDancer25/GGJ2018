using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckTrap : MonoBehaviour {

    public bool activated;
    public float timeTrapped, timeToReset;
    public bool reseted;

    private Vector3 shieldPosition;
	// Use this for initialization
	void Start () {
        activated = false;
        timeTrapped = 2f;
        timeToReset = 3f;
        reseted = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (activated && reseted)
        {
            GameObject.Find("Shield").GetComponent<MeshRenderer>().enabled = true;
            Invoke("Deactivate", timeTrapped);
            reseted = false;
        }
	}

    public void setBubblePosition(Vector3 pos)
    {
        transform.DetachChildren();
        GameObject.Find("Shield").transform.position = pos;
        activated = true;
    }

    void Deactivate()
    {
        GameObject.Find("Shield").GetComponent<MeshRenderer>().enabled = false;
        GameObject.Find("Shield").transform.parent = transform;
        activated = false;
        Invoke("Reset", timeToReset);
    }

    void Reset()
    {
        reseted = true;
    }

}
