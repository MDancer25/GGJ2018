﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private GameObject passport;
	private LineRenderer lineRenderer;
    static Animator animator;
	private Rigidbody passportRB;
	private TrailRenderer passportTR;

	public bool holdingPassport = false;
    private bool stuckTrapped, slowTrapped, knockedBack;
    private float knockBackSpeed;
	public float moveSpeed = 5.0f;
	public bool hasAcceleration = false;
	public float firingElevationAngle = 70.0f;
	public float timeBetweenPickUp = 1.0f;
	public float minThrowVelocity = 5.0f;
	public float maxThrowVelocity = 40.0f;
    public int playerNum;
	public bool canThrow = false;


	private Vector3 inputMovement;
	private Vector3 forwardVector;
	private float angle;
	private float throwForce;
	private float pressTime;
	private bool canPickUp;
	private float pickUpTime;


	private string S_BUTTON;
	private string THROW_BUTTON;
	private string O_BUTTON ;
	private string T_BUTTON;

	private float barrierDuration;

	public delegate void OnPlayerPickPassport (GameObject player);	// new delegate
	public static event OnPlayerPickPassport notifyPlayerPickedPassport;	// observer

	// Use this for initialization
	void Start () {
		passport = GameObject.Find ("/Passport");
		passportRB = passport.GetComponent<Rigidbody> ();
		passportTR = passport.GetComponent<TrailRenderer> ();
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

		S_BUTTON = "joystick " + playerNum + " button 0";
		THROW_BUTTON = "joystick " + playerNum + " button 1";
		O_BUTTON = "joystick " + playerNum + " button 2";
		T_BUTTON = "joystick " + playerNum + " button 3";

		barrierDuration = 1f;
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		var horizontalAxis = Input.GetAxis ("Horizontal"+playerNum);
		var verticalAxis = Input.GetAxis("Vertical"+playerNum);

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


		if (new Vector3 (verticalAxis, 0, horizontalAxis).sqrMagnitude > 0.2 && !stuckTrapped && !slowTrapped && !knockedBack) {
			angle = Mathf.Atan2 (horizontalAxis, verticalAxis) * Mathf.Rad2Deg;
            animator.SetBool("isRunning", true);
            Debug.Log(animator.GetBool("isRunning"));	
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

		transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));

		float rHor = Input.GetAxis("RightHorizontal");
		float rVer = Input.GetAxis("RightVertical");

		//print (rHor + ", " + rVer);

		if (pressTime != 0.0f && holdingPassport) {
			lineRenderer.SetPosition(1, new Vector3(0,0, 3 * (Time.time - pressTime)));
		}

		if (Input.GetKeyDown (THROW_BUTTON)) {
			if (holdingPassport) {
				lineRenderer.SetPosition (1, Vector3.zero);
				lineRenderer.enabled = true;
				canThrow = true;
				pressTime = Time.time;
			}
		}

		if (Input.GetKeyUp (THROW_BUTTON)) {
			float timeHeld = Time.time - pressTime;
            
			if (holdingPassport && passport != null && canThrow)
            {
                    animator.SetTrigger("isThrowing");

					notifyPlayerPickedPassport(passport);
                    //target = GameObject.Find ("/Floor/Target");\
                    Vector3 target = this.transform.position + forwardVector;
				//passportRB.constraints = RigidbodyConstraints.None;

				passportRB.isKinematic = false;



				passport.GetComponent<Rigidbody> ().useGravity = true;
				ThrowObject (target, Mathf.Min(Mathf.Max(minThrowVelocity, throwForce * timeHeld), maxThrowVelocity), passport);   

				passport.transform.parent = null;

				holdingPassport = false;
				lineRenderer.SetPosition (1, Vector3.zero);

				lineRenderer.enabled = false;
				canThrow = false;


			}

		}

		//if (holdingPassport)
		//	passport.transform.position = this.transform.position + new Vector3 (2, 0, 0);

			
		
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
			Debug.Log (this.gameObject.name);
			if (!canPickUp)
				return;
			canPickUp = false;
			holdingPassport = true;

			lineRenderer.SetPosition (1, Vector3.zero);


			passportRB.useGravity = false;
			passportRB.velocity = Vector3.zero;
			passportRB.angularVelocity = Vector3.zero;

			passport.transform.parent = this.transform;
			//passport.transform.position = this.transform.position + new Vector3 (-1.0f, 2.5f, 1.5f);
			passport.transform.position = this.transform.position + 2f*this.transform.forward + new Vector3(0,2.5f,0);
			//passportRB.constraints = RigidbodyConstraints.FreezeAll;
			passportRB.isKinematic = true;

			pickUpTime = Time.time;

			notifyPlayerPickedPassport(this.gameObject);
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
            other.transform.Find("Particles").transform.GetComponent<ParticleSystem>().Play();
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
    }

}
