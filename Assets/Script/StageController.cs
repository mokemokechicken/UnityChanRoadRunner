using UnityEngine;
using System.Collections;

public class StageController : MonoBehaviour {

	private int[,] blocks;
	// Use this for initialization
	void Start () {
		blocks = new int[100,100];
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Block")) {
			int x = Mathf.RoundToInt(go.transform.position.x);
			int y = Mathf.FloorToInt(go.transform.position.y);
			go.transform.position = new Vector3(x, y+0.5f, 0);
			blocks[x,y] = 1;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool isFalling(Vector3 pos) {
		int x = Mathf.RoundToInt(pos.x);
		int y = Mathf.FloorToInt(pos.y-0.02f);
		if (y > 0 && blocks[x, y] == 0) {
			return true;
		}
		return false;
	}
}
