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

    // Use this for initialization
    void Start ()
    {
        left = true;
        canSwitch = true;
        startTime = Time.time;
        initPosition = transform.position;
        endPosition = transform.parent.Find("EndMark").position;
        journeyLength = Vector3.Distance(initPosition, endPosition);
    }
	
	// Update is called once per frame
	void Update () {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        if (left)
        {
            transform.position = Vector3.Lerp(initPosition, endPosition, fracJourney);
        }else
        {
            transform.position = Vector3.Lerp(endPosition, initPosition, fracJourney);
        }

        checkSideMoving();
    }

    private void checkSideMoving()
    {
        if (canSwitch)
        {
            if (transform.position == endPosition && left)
            {
                startTime = Time.time;
                left = false;
            }
            else if (transform.position == initPosition && !left)
            {
                startTime = Time.time;
                left = true;
            }
            Invoke("ResetSwitch", 0.5f);
            canSwitch = false;
        }
    }
    
    void ResetSwitch()
    {
        canSwitch = true;
    }

}

