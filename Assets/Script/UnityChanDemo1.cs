using UnityEngine;
using System.Collections;

public class UnityChanDemo1 : MonoBehaviour {
	private float maxFallingVelocity = -1f;
	private Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("right")) {
			transform.position += new Vector3(0.05f, 0, 0);
			transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
			animator.SetBool("is_running", true);
		} else if (Input.GetKey ("left")) {
			transform.position += new Vector3(-0.05f, 0, 0);
			transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
			animator.SetBool("is_running", true);
		} else {
			animator.SetBool("is_running", false);
		}

//		if (rigidbody.velocity.y < maxFallingVelocity) {
//			rigidbody.velocity = new Vector3(rigidbody.velocity.x, maxFallingVelocity, rigidbody.velocity.z);
//		}
	}
}
