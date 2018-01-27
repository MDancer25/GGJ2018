using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {
    
    public int id;
    Rigidbody body;
    public float speed;
    bool pickedUp;

    private float timeToGoThrough;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
        pickedUp = true;
        timeToGoThrough = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        body.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision col)
    {
        string[] nameArray = col.transform.name.Split('_');
        /*if (nameArray[0] == "Door" && pickedUp)
        {
            col.transform.gameObject.GetComponent<Door>().OpenDoor(id);
        }*/
        if(nameArray[0] == "Crate")
        {
            GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(turnOffKinematic());
        }
    }

    IEnumerator turnOffKinematic()
    {
        yield return new WaitForSeconds(timeToGoThrough);
        GetComponent<Rigidbody>().isKinematic = false;
    }


}
