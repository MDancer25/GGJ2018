using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 18;

	public float turnSpeed = 160;

	private Rigidbody rig;

	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        float hAxis = Input.GetAxis("LeftJoystickHorizontal");
        float vAxis = Input.GetAxis("LeftJoystickVertical");

        float hRightAxis = Input.GetAxis("RightJoystickHorizontal");

        Vector3 movement = transform.TransformDirection(new Vector3(hAxis, 0, -vAxis) * speed * Time.deltaTime);

        rig.MovePosition(transform.position + movement);

        transform.Rotate(new Vector3(0, hRightAxis, 0), turnSpeed * Time.deltaTime);

	}
}
