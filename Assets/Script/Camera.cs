﻿using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {
	public Transform target;

	// Use this for initialization
	void Start () {
		//target = GameObject.FindWithTag("Player").transform;
		foreach(GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
			Debug.Log(go.name);
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = target.position + new Vector3(0, 1, -3);
	}
}