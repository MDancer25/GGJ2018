using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private GameObject passport;
	private GameObject target;
	public bool holdingPassport = false;
	private Rigidbody rb;

	private float barrierDuration;

	// Use this for initialization
	void Start () {
		passport = GameObject.Find ("/Passport");
		rb = GetComponent<Rigidbody> ();
		barrierDuration = 1f;

	}
	
	// Update is called once per frame
	void Update () {
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);

		if (Input.GetKeyDown ("space")) {
			if (holdingPassport && passport != null) {
				target = GameObject.Find ("/Floor/Target");
				passport.GetComponent<Rigidbody> ().useGravity = true;
				ThrowObject (target.transform.position, 10f, passport);   
				passport.transform.parent = null;
			}
		}

		/*
		if (holdingPassport) {
			passport.transform.position = transform.position + new Vector3(0,2,0);
		}
		*/
		
	}

	void ThrowObject(Vector3 targetLocation, float initialVelocity, GameObject ball) {
		Vector3 direction = (targetLocation - transform.position).normalized;
		float distance = Vector3.Distance(targetLocation, transform.position);

		float firingElevationAngle = FiringElevationAngle(Physics.gravity.magnitude, distance, initialVelocity);
		Vector3 elevation = Quaternion.AngleAxis(firingElevationAngle, transform.right) * transform.up;
		float directionAngle = AngleBetweenAboutAxis(transform.forward, direction, transform.up);
		Vector3 velocity = Quaternion.AngleAxis(directionAngle, transform.up) * elevation * initialVelocity;

		// ballGameObject is object to be thrown
		ball.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
	}


	// Helper method to find angle between two points (v1 & v2) with respect to axis n
	public static float AngleBetweenAboutAxis(Vector3 v1, Vector3 v2, Vector3 n) {
		return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
	}

	// Helper method to find angle of elevation (ballistic trajectory) required to reach distance with initialVelocity
	// Does not take wind resistance into consideration.
	private float FiringElevationAngle(float gravity, float distance, float initialVelocity) {
		float angle = 0.5f * Mathf.Asin ((gravity * distance) / (initialVelocity * initialVelocity)) * Mathf.Rad2Deg;
		return angle;
	}
		
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Passport")) {
			//other.gameObject.SetActive (false);
			holdingPassport = true;
			passport.GetComponent<Rigidbody> ().useGravity = false;
			passport.transform.parent = this.transform;
			passport.transform.position = this.transform.position + new Vector3(0,2,0);
		}
		
	}

	void OnCollisionEnter(Collision col){
		string[] nameArray = col.transform.name.Split('_');
		if (nameArray[0] == "Door" && holdingPassport)
		{
			int keyId = transform.Find ("Passport").GetComponent<Key> ().id;
			if (col.transform.gameObject.GetComponent<Door> ().OpenDoor (keyId)) {
				transform.GetComponent<Rigidbody> ().isKinematic = true;
				StartCoroutine (turnOffKinematic ());
			}
		}
	}

	IEnumerator turnOffKinematic(){
		yield return new WaitForSeconds (barrierDuration);
		transform.GetComponent<Rigidbody> ().isKinematic = false;
	}

}
