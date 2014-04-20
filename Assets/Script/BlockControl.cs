using UnityEngine;
using System.Collections;

public class BlockControl : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	void HitGun() {
		if (animation.IsPlaying("CubeBreaking")) {
			return;
		}
		animation.Play("CubeBreaking");
	}
}
