using UnityEngine;
using System.Collections;


public class BaseCharacterControl : MonoBehaviour, ICharacterControl  {
	protected Animator animator;
	protected CharacterState state;

	protected float moveSpeed = 0.05f;
	
	// Use this for initialization
	protected void StartBase () {
		animator = GetComponent<Animator>();
		state = new CharacterState(this);
	}
	
	/**
	 * <param name="direction">-1:left or 1:right</param>
	 */
	virtual public void MoveLeftRight(int direction) { 
		transform.position += new Vector3(moveSpeed * direction, 0, 0);
		transform.rotation = Quaternion.Euler(new Vector3(0, 90*direction, 0));
	}
	
	/**
	 * <param name="direction">-1:left or 1:right</param>
	 */
	virtual public void MoveUpDown(int direction) {
		transform.position += new Vector3(0, moveSpeed * direction, 0);
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
	}
	
	virtual public void Shoot(int direct) {
	}
	
	virtual public void Grap() {
		rigidbody.useGravity = false;
		rigidbody.velocity = Vector3.zero;
	}
	
	virtual public void Freefall() {
		rigidbody.useGravity = true;
	}
	
	virtual public void GrapBar() {
		transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
	}
	
	virtual public void GrapLadder() {
	}
	
	virtual public Rigidbody getRigidbody() {
		return rigidbody;
	}
	
	void OnTriggerEnter(Collider other) {
		state.OnTriggerEnter(other.gameObject);
	}
	
	void OnTriggerExit(Collider other) {
		state.OnTriggerExit(other.gameObject);
	}
}

