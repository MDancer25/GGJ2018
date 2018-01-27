using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    bool openDoor,closeDoor;
    public int id;
    private float maxDistance;
    private float minDistance;
    private float moveSpeed;
    private float timeToClose;
    private Vector3 initPosition;
    private Rigidbody body;

	// Use this for initialization
	void Start () {
        openDoor = false;
        closeDoor = false;
        maxDistance = 2.0f;
        minDistance = 0.2f;
        timeToClose = 0.5f;
        moveSpeed = 5.0f;
        initPosition = transform.position;
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (openDoor)
        {
            body.MovePosition(transform.position + transform.right * moveSpeed* Time.deltaTime);
        }
        else if (closeDoor)
        {
            body.MovePosition(transform.position - transform.right * moveSpeed * Time.deltaTime);
        }
        checkDoorPosition();
    }

    void checkDoorPosition()
    {
        if (openDoor)
        {
            if (Vector3.Distance(transform.position, initPosition) >= maxDistance)
            {
                openDoor = false;
                StartCoroutine(CloseDoor());
            }
        }
        else if (closeDoor)
        {
            //Debug.Log(Vector3.Distance(transform.position, initPosition));
            if(Vector3.Distance(transform.position,initPosition) <= minDistance)
            {
                closeDoor = false;
            }
        }
    }

    IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(timeToClose);
        closeDoor = true;
    }

    public bool OpenDoor(int keyId)
    {
        if(keyId == id)
            openDoor = true;
        return (keyId == id);
    }
}
