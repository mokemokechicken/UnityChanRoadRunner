using UnityEngine;
using System.Collections;


public class UnityChanControl : BaseCharacterControl  {

	// Use this for initialization
	void Start () {
		StartBase();
	}
	
	// Update is called once per frame
	void Update () {
		if (Global.paused) {
			animator.SetBool("is_falling", true);
			transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
			return;
		}

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

	override public void Shoot(int direct) {
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

	private bool canDestroyBlock(Transform block) {
		return 
			block.tag == Tag.BLOCK && 
			Mathf.Abs(block.position.x - transform.position.x) < 1.2f &&
			transform.position.y - block.position.y > 0.3;
	}

	private int Direction(bool low, bool high) {
		return low ? -1 : high ? 1 : 0;
	}
}

