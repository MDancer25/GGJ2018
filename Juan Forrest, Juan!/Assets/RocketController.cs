using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RocketController : MonoBehaviour {

	public float maxRocketSpeed;

	Vector3 force;
	Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		force = CalculateRandomForceVector ();	
	}

	void Update () 
	{
		rb.AddForce (force * maxRocketSpeed);	
		transform.localRotation = Quaternion.Euler (new Vector3(transform.localRotation.x, transform.localRotation.y ,0));
	}

	Vector3 CalculateRandomForceVector()
	{
		float x, y, z;
		x = Random.Range (-1.0f, 1.0f);
		y = Random.Range (1, maxRocketSpeed);
		z = Random.Range (0.5f, 1.0f);

		return new Vector3 (x, y, z);
	}
}
