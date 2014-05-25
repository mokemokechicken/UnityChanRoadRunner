using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StageController : MonoBehaviour {
	public GameObject blockPrefab;
	public GameObject ladderPrefab;
	public GameObject barPrefab;
	public GameObject playerPrefab;
	public GameObject treasurePrefab;
	public GameObject cameraPrefab;

	const char BLOCK = '#';
	const char LADDER = 'H';
	const char BAR = '-';
	const char PLAYER = 'P';
	const char TREASURE = '$';

	private GameObject playerObject;
	private int numTreasure;

	private Dictionary<char, GameObject> prefabMap;

	// Use this for initialization
	void Start () {
		prefabMap = new Dictionary<char, GameObject> {
			{BLOCK, blockPrefab},
			{LADDER, ladderPrefab},
			{BAR, barPrefab},
			{PLAYER, playerPrefab},
			{TREASURE, treasurePrefab},
		};

		BuildStage(STAGE_DATA);
		// Camera
		if (playerObject) {
			var camera = (GameObject)Instantiate(cameraPrefab);
			var cameraScript = camera.GetComponent<Camera>();
			cameraScript.target = playerObject.transform;
		}

		Global.stageControler = this;
	}

	public void PlayerGotTreasure() {
		numTreasure--;
		Global.score += 100;
	}

	void BuildStage(string stageData) {
		numTreasure = 0;
		// Left Botton's position is (0,0)
		var lines = stageData.Split('\n');
		int y = lines.Count();
		foreach (var line in lines) {
			y--;
			int x = 0;
			foreach (var ch in line) {
				GameObject obj;
				prefabMap.TryGetValue(ch, out obj);
				if (obj) {
					var instance = (GameObject)Instantiate(obj, new Vector3(x, y, 0), Quaternion.identity);
					if (ch == PLAYER) {
						playerObject = instance;
					} else if (ch == TREASURE) {
						numTreasure++;
					}
				}
				x++;
			}
		}
	}

	const string STAGE_DATA = @"
#         H    #
# P       H    #
#      $  H    #
# $#HHHHHHH    #
#         H    #
#---------H    #
# $     $ H    #
####H###########
#   H--------- #
#   H   $      #
#   H ####     #
#   H----      #
#   H    ##### #
#   H          #
#   H     $    #
################";
}

