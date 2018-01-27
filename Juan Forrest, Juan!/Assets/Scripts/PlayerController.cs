using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private GameObject passport;
	private LineRenderer lineRenderer;

	public bool holdingPassport = false;
    private bool stuckTrapped, slowTrapped, knockedBack;
    private float knockBackSpeed;
	public float moveSpeed = 5.0f;
	public bool hasAcceleration = false;
	public float firingElevationAngle = 70.0f;
	public float timeBetweenPickUp = 1.0f;
	public float minThrowVelocity = 5.0f;
	public float maxThrowVelocity = 40.0f;


	private Vector3 inputMovement;
	private Vector3 forwardVector;
	private float angle;
	private float throwForce;
	private float pressTime;
	private bool canPickUp;
	private float pickUpTime;
    private GameObject bomb;

	private string S_BUTTON = "joystick button 0";
	private string THROW_BUTTON = "joystick button 1";
	private string O_BUTTON = "joystick button 2";
	private string T_BUTTON = "joystick button 3";

	private float barrierDuration;

	// Use this for initialization
	void Start () {
		passport = GameObject.Find ("/Passport");
        stuckTrapped = false;
        slowTrapped = false;
        knockedBack = false;
        knockBackSpeed = 5;
		lineRenderer = GetComponent<LineRenderer> ();
		inputMovement = new Vector3 (0, 0, 0);
		angle = 0.0f;
		throwForce = 10.0f;
		pressTime = 0.0f;
		canPickUp = true;
		pickUpTime = 0.0f;
		lineRenderer.enabled = false;
		lineRenderer.startColor = Color.blue;
		lineRenderer.endColor = Color.red;
		barrierDuration = 1f;
	}
	
	// Update is called once per frame
	void Update () {

		var horizontalAxis = Input.GetAxis ("Horizontal");
		var verticalAxis = Input.GetAxis("Vertical");

        if (knockedBack)
        {
            transform.position -= transform.forward * Time.deltaTime*knockBackSpeed;
        }
        else if (slowTrapped)
			transform.Translate (inputMovement * Time.deltaTime * moveSpeed * 0.5f, Space.World);

        

     
		if (!canPickUp && Time.time - pickUpTime > timeBetweenPickUp)
			canPickUp = true;


		inputMovement.Set (horizontalAxis, 0, verticalAxis);

		if (hasAcceleration) {
			print ("not yet implemented");         
		} else {
			if (!slowTrapped && !stuckTrapped && !knockedBack)
				transform.Translate (inputMovement * Time.deltaTime * moveSpeed, Space.World);
		}

		forwardVector = this.transform.forward;


		if (new Vector3 (verticalAxis, 0, horizontalAxis).sqrMagnitude > 0.2) {
			angle = Mathf.Atan2 (horizontalAxis, verticalAxis) * Mathf.Rad2Deg;
		}
		transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));

		float rHor = Input.GetAxis("RightHorizontal");
		float rVer = Input.GetAxis("RightVertical");

		//print (rHor + ", " + rVer);

		if (pressTime != 0.0f && holdingPassport) {
			lineRenderer.enabled = true;
			lineRenderer.SetPosition(1, new Vector3(0,0,  3 * (Time.time - pressTime)));
		}




		if (Input.GetKeyDown (THROW_BUTTON)) {
			pressTime = Time.time;
		}

		if (Input.GetKeyUp (THROW_BUTTON)) {
			float timeHeld = Time.time - pressTime;

			if (holdingPassport && passport != null) {
				//target = GameObject.Find ("/Floor/Target");
				Vector3 target = this.transform.position + forwardVector;

				passport.GetComponent<Rigidbody> ().useGravity = true;
				ThrowObject (target, Mathf.Min(Mathf.Max(minThrowVelocity, throwForce * timeHeld), maxThrowVelocity), passport);   

				passport.transform.parent = null;

				holdingPassport = false;
				lineRenderer.SetPosition (1, Vector3.zero);
				lineRenderer.enabled = false;
				pressTime = 0.0f;

			}

		}

			
		
	}

	void ThrowObject(Vector3 targetLocation, float initialVelocity, GameObject ball) {
		Vector3 direction = (targetLocation - transform.position).normalized;
		float distance = Vector3.Distance(targetLocation, transform.position);

		Vector3 elevation = Quaternion.AngleAxis(firingElevationAngle, transform.right) * transform.up;
		float directionAngle = AngleBetweenAboutAxis(transform.forward, direction, transform.up);
		Vector3 velocity = Quaternion.AngleAxis(directionAngle, transform.up) * elevation * initialVelocity;

		// ball is the object to be thrown
		ball.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
	}


	// Helper method to find angle between two points (v1 & v2) with respect to axis n
	public static float AngleBetweenAboutAxis(Vector3 v1, Vector3 v2, Vector3 n) {
		return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Passport")) {
            
			if (!canPickUp)
				return;
			canPickUp = false;
			holdingPassport = true;

			lineRenderer.SetPosition (1, Vector3.zero);


			Rigidbody passportRB = passport.GetComponent<Rigidbody> ();
			passportRB.useGravity = false;
			passportRB.velocity = Vector3.zero;
			passportRB.angularVelocity = Vector3.zero;

			passport.transform.parent = this.transform;
			passport.transform.position = this.transform.position + new Vector3 (0, 2, 0);


			pickUpTime = Time.time;
		}
        else if (other.gameObject.CompareTag("StuckTrap"))
        {
            if (other.GetComponent<StuckTrap>().reseted)
            {
                other.GetComponent<StuckTrap>().setBubblePosition(transform.position);
                stuckTrapped = true;
                Invoke("ResetTrapped", other.GetComponent<StuckTrap>().timeTrapped);
            }
        }
        else if (other.gameObject.CompareTag("SlowTrap"))
        {
            slowTrapped = true;
            Invoke("ResetTrapped", 3f);
        }
        else if (other.gameObject.CompareTag("Bomb"))
        {
            knockedBack = true;
            for (int i = 0; i < other.transform.childCount; i++)
            {
                string[] nameArray = other.transform.GetChild(i).name.Split(' ');
                Debug.Log(nameArray[0] + " " + i);
                if(nameArray[0] == "Particles")
                    other.transform.GetChild(i).transform.GetComponent<ParticleSystem>().Play();
                other.transform.GetComponent<MeshRenderer>().enabled = false;
            }
            bomb = other.gameObject;
            Invoke("ResetKnocked", 1f);
        }

		/*string[] nameArray = other.transform.name.Split('_');
		if (nameArray[0] == "Door" && holdingPassport)
		{
			int keyId = transform.Find ("Passport").GetComponent<Key> ().id;
			if(other.transform.gameObject.GetComponent<Door>().OpenDoor(keyId))
				transform.Find ("Passport").GetComponent<Rigidbody>().isKinematic = true;
		}*/
		
	}

	void OnCollisionEnter(Collision col)
	{
		string[] nameArray = col.transform.name.Split('_');
		if (nameArray[0] == "Door" && holdingPassport)
        {
			int keyId = transform.Find ("Passport").transform.GetComponent<Key> ().id;
			if (col.transform.gameObject.GetComponent<Door> ().OpenDoor (keyId)) {
				transform.Find ("Passport").transform.GetComponent<Rigidbody> ().isKinematic = true;
				GetComponent<Rigidbody> ().isKinematic = true;
				Invoke ("ResetKinematic", 1f);
			}
        }
	}

	void ResetKinematic(){
		GetComponent<Rigidbody> ().isKinematic = false;
		transform.Find ("Passport").transform.GetComponent<Rigidbody> ().isKinematic = false;
	}

    void ResetTrapped()
    {
        stuckTrapped = false;
        slowTrapped = false;
    }

    void ResetKnocked()
    {
        knockedBack = false;
        bomb.GetComponent<MeshRenderer>().enabled = true;
    }

}
