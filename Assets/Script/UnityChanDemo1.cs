using UnityEngine;
using System.Collections;


public class UnityChanDemo1 : MonoBehaviour {
	private Animator animator;
	private PlayerState state;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		state = new PlayerState(this);
	}
	
	// Update is called once per frame
	void Update () {
		// Moving
		state.KeyLeftRight(Direction(Input.GetKey (KeyCode.LeftArrow), Input.GetKey (KeyCode.RightArrow)));
		state.KeyUpDown(Direction(Input.GetKey(KeyCode.DownArrow), Input.GetKey(KeyCode.UpArrow)));

		// Gun
		state.KeyGun(Direction(Input.GetKey(KeyCode.Z), Input.GetKey(KeyCode.X)));

		// Update Animation State
		animator.SetBool("is_running", state.IsMovingLeftRight());
		animator.SetBool("on_bar", state.IsOnBar());
		animator.SetBool("on_ladder", state.IsOnLadder());
		animator.SetBool("is_falling", state.IsFalling());

		state.Tick();
	}

	/**
	 * <param name="direction">-1:left or 1:right</param>
	 */
	public void MoveLeftRight(int direction) { 
		transform.position += new Vector3(0.05f * direction, 0, 0);
		transform.rotation = Quaternion.Euler(new Vector3(0, 90*direction, 0));
	}

	/**
	 * <param name="direction">-1:left or 1:right</param>
	 */
	public void MoveUpDown(int direction) {
		transform.position += new Vector3(0, 0.05f * direction, 0);
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
	}

	public void Shoot(int direct) {
		RaycastHit hit;
		Vector3 fromPos = transform.position+Vector3.up;
		Vector3 direction = new Vector3(direct, -1, 0);
		float length = 1.8f;
		Debug.DrawRay(fromPos, direction.normalized * length, Color.green, 1, false);
		if (Physics.Raycast(fromPos, direction, out hit, length)) {
			if (canDestroyBlock(hit.transform)) {
				hit.transform.gameObject.SendMessage("HitGun");
			}
		}
	}

	public void Grap() {
		rigidbody.useGravity = false;
		rigidbody.velocity = Vector3.zero;
	}

	public void Freefall() {
		rigidbody.useGravity = true;
	}

	public void GrapBar() {
		transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
	}

	public void GrapLadder() {
	}

	private bool canDestroyBlock(Transform block) {
		return 
			block.tag == Tag.BLOCK && 
			Mathf.Abs(block.position.x - transform.position.x) < 1.2f &&
			transform.position.y - block.position.y > 0.3;
	}

	void OnTriggerEnter(Collider other) {
		state.OnTriggerEnter(other.gameObject);
	}

	void OnTriggerExit(Collider other) {
		state.OnTriggerExit(other.gameObject);
	}

	private int Direction(bool low, bool high) {
		return low ? -1 : high ? 1 : 0;
	}
}

/*
 * Role:
 *	- Manage UnityChan's Status, Not Behavior.
 * 	- Process Events.
 *  - Call player actions when triggered by events.
 */
class PlayerState {
	private int ladderEnter = 0;
	private int barEnter = 0;
	private bool isFalling = false;
	private bool isMovingLeftRight = false;
	private bool isMovingUpDown = false;
	private UnityChanDemo1 player;

	public PlayerState(UnityChanDemo1 player) {
		this.player = player;
	}

	// Status
	public bool IsOnBar() {
		return barEnter > 0;
	}

	public bool IsOnLadder() {
		return ladderEnter > 0;
	}

	public bool IsGripping() {
		return IsOnBar() || IsOnLadder();
	}

	public bool IsFalling() {
		return isFalling;
	}

	public bool IsGround() {
		return !IsOnBar() && !IsOnLadder() && !IsFalling();
	}

	public bool IsMovingLeftRight() {
		return isMovingLeftRight;
	}

	public bool IsMovingUpDown() {
		return isMovingUpDown;
	}


	//// Event 
	public void Tick() {
		if (IsOnBar()) {
			player.Grap();
			player.GrapBar();
		} else if (IsOnLadder()) {
			player.Grap();
			player.GrapLadder();
		} else {
			player.Freefall();
		}

		// if Velocity.y reach some degree, Falling is True
		if (player.rigidbody.velocity.y <  -0.05f) {
			isFalling = true;
		} else {
			isFalling = false;
		}
	}

	// Key Event
	public void KeyLeftRight(int direction) {
		isMovingLeftRight = false;
		if (direction != 0 && !IsFalling()) {
			player.MoveLeftRight(direction);
			isMovingLeftRight = true;
		}
	}

	public void KeyUpDown(int direction) {
		isMovingUpDown = false;
		if (IsOnLadder() && direction != 0) {
			player.MoveUpDown(direction);
			isMovingUpDown = true;
		} else if (IsOnBar() && direction == -1) {
			ReleaseBar();
		}
	}

	public void KeyGun(int direction) {
		if (!IsFalling() && direction != 0) {
			player.Shoot(direction);
		}
	}
	
	public void ReleaseBar() {
		barEnter = 0;
	}

	public void OnTriggerEnter(GameObject go) {
		switch(go.tag) {
		case Tag.LADDER:
			ladderEnter += 1;
			break;
		case Tag.BAR:
			barEnter += 1;
			break;
		}
	}

	public void OnTriggerExit(GameObject go) {
		switch(go.tag) {
		case Tag.LADDER:
			ladderEnter -= 1;
			break;
		case Tag.BAR:
			if (barEnter > 0) {
				barEnter -= 1;
			}
			break;
		}
	}

}
