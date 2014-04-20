using UnityEngine;
using System.Collections;

public class UnityChanDemo1 : MonoBehaviour {
	private const string LADDER = "Ladder";
	private const string BAR = "Bar";
	private const string BLOCK = "Block";
	private const string BEAM = "Beam";

	private Animator animator;
	public GameObject beamPrefab;

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
			if (Input.GetKey(KeyCode.RightArrow)) {
				transform.position += new Vector3(0.05f, 0, 0);
				transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
				is_running = true;
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				transform.position += new Vector3(-0.05f, 0, 0);
				transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
				is_running = true;
			}
		}
		animator.SetBool("is_running", is_running);

		// if entering in any ladder collision, can move Up/Down
		if (Input.GetKey(KeyCode.UpArrow) && ladderEnter > 0) {
				transform.position += new Vector3(0, 0.05f, 0);
		} else if (Input.GetKey(KeyCode.DownArrow) && ladderEnter > 0) {
			transform.position += new Vector3(0, -0.05f, 0);
		} else if (Input.GetKey(KeyCode.DownArrow) && barEnter > 0) { // if entering in bar and KeyDown, bar are released.
			barEnter = 0;
		}

		// if entering in any ladder or bar, gravity is off
		if (ladderEnter > 0 || barEnter > 0) {
			rigidbody.useGravity = false;
			rigidbody.velocity = Vector3.zero;
		} else {
			rigidbody.useGravity = true;
		}

		// Gun
		int gun = Input.GetKey(KeyCode.Z) ? -1 : Input.GetKey(KeyCode.X) ? 1 : 0;
		if (gun != 0 && !isFalling) {
			RaycastHit hit;
			Vector3 fromPos = transform.position+Vector3.up;
			Vector3 direction = new Vector3(gun, -1, 0);
			float length = 1.8f;
			Debug.DrawRay(fromPos, direction.normalized * length, Color.green, 1, false);
			if (Physics.Raycast(fromPos, direction, out hit, length)) {
				if (canDestroyBlock(hit.transform)) {
					hit.transform.gameObject.SendMessage("HitGun");
				}
 			}
		}

		// if Velocity.y reach some degree, Falling is True
		if (rigidbody.velocity.y <  -0.05f) {
			isFalling = true;
		} else {
			isFalling = false;
		}
	}

	private bool canDestroyBlock(Transform block) {
		return 
			block.tag == BLOCK && 
			Mathf.Abs(block.position.x - transform.position.x) < 1.2f &&
			transform.position.y - block.position.y > 0.3;
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
