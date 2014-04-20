using UnityEngine;
using System.Collections;

public class BlockControl : MonoBehaviour {
	private const int DISAPPEAR_TIMER_COUNT = 60*3;
	private int disappearTimer = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (disappearTimer > 0) {
			disappearTimer--;
		}
	}

	void HitGun() {
		if (disappearTimer > 0) {
			return;
		}
		disappearTimer = DISAPPEAR_TIMER_COUNT;
		animation.Play("CubeBreaking");
	}

}
