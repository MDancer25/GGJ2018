using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour {

    private Vector3 initPosition;
    private Vector3 endPosition;

    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;
    private bool left, canSwitch;

    public int fenceId;
    public bool reached;

    // Use this for initialization
    void Start ()
    {
        left = true;
        canSwitch = true;
        reached = false;
        startTime = Time.time;
        initPosition = transform.position;
        endPosition = transform.parent.Find("EndMark").position;
        journeyLength = Vector3.Distance(initPosition, endPosition);
    }
	
	// Update is called once per frame
	void Update () {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        if (left && !reached)
        {
            transform.position = Vector3.Lerp(initPosition, endPosition, fracJourney);
        }else if(!left && !reached)
        {
            transform.position = Vector3.Lerp(endPosition, initPosition, fracJourney);
        }
        checkSideMoving();
    }

    private void checkSideMoving()
    {

        if (transform.position == endPosition && left)
        {
            left = false;
            reached = true;
        }
        else if (transform.position == initPosition && !left)
        {
            left = true;
            reached = true;
        }
    }

    public void Switch()
    {
        startTime = Time.time;
        reached = false;
    }

}

