using UnityEngine;
using System.Collections;

public class UnityChanDemo1 : MonoBehaviour {
	private const string LADDER = "Ladder";
	private const string BAR = "Bar";
	private const string BLOCK = "Block";

	private Animator animator;

	public int ladderEnter = 0;
	public int barEnter = 0;
	public bool isFalling = false;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		bool is_running = false;
		if (!isFalling) { // If Not Falling, can move to Left/Right
			if (Input.GetKey("right")) {
				transform.position += new Vector3(0.05f, 0, 0);
				transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
				is_running = true;
			} else if (Input.GetKey ("left")) {
				transform.position += new Vector3(-0.05f, 0, 0);
				transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
				is_running = true;
			}
		}
		animator.SetBool("is_running", is_running);

		// if entering in any ladder collision, can move Up/Down
		if (Input.GetKey("up") && ladderEnter > 0) {
				transform.position += new Vector3(0, 0.05f, 0);
		} else if (Input.GetKey("down") && ladderEnter > 0) {
			transform.position += new Vector3(0, -0.05f, 0);
		} else if (Input.GetKey("down") && barEnter > 0) { // if entering in bar and KeyDown, bar are released.
			barEnter = 0;
		}

		// if entering in any ladder or bar, gravity is off
		if (ladderEnter > 0 || barEnter > 0) {
			rigidbody.useGravity = false;
			rigidbody.velocity = Vector3.zero;
		} else {
			rigidbody.useGravity = true;
		}

		// if Velocity.y reach some degree, Falling is True
		if (rigidbody.velocity.y <  -0.05f) {
			isFalling = true;
		} else {
			isFalling = false;
		}
	}

	void OnTriggerEnter(Collider other) {
		switch(other.gameObject.tag) {
		case LADDER:
			ladderEnter += 1;
			break;
		case BAR:
			barEnter += 1;
			break;
		}
	}

	void OnTriggerExit(Collider other) {
		switch(other.gameObject.tag) {
		case LADDER:
			ladderEnter -= 1;
			break;
		case BAR:
			if (barEnter > 0) {
				barEnter -= 1;
			}
			break;
		}
	}
}
