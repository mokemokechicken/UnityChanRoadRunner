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
	public GameObject goalPrefab;
	public GameObject cameraPrefab;
	public GameObject enemyPrefab;

	const char BLOCK = '#';
	const char LADDER = 'H';
	const char HIDDEN_LADDER = 'h';
	const char BAR = '-';
	const char PLAYER = 'P';
	const char TREASURE = '$';
	const char GOAL = 'G';
	const char ENEMY = 'E';

	private GameObject playerObject;
	private GameObject cameraObject;
	private int numTreasure;
	private float loadNextAt;
	private Dictionary<char, GameObject> prefabMap;
	private List<GameObject> hiddenObjects;
	private List<GameObject> enemyObjects;

	// Use this for initialization
	void Start () {
		prefabMap = new Dictionary<char, GameObject> {
			{BLOCK, blockPrefab},
			{LADDER, ladderPrefab},
			{HIDDEN_LADDER, ladderPrefab},
			{BAR, barPrefab},
			{PLAYER, playerPrefab},
			{TREASURE, treasurePrefab},
			{GOAL, goalPrefab},
			{ENEMY, enemyPrefab},
		};

		BuildStage(STAGE_DATA);

		if (playerObject) {
			// Camera
			cameraObject = (GameObject)Instantiate(cameraPrefab);
			var cameraScript = cameraObject.GetComponent<Camera>();
			cameraScript.target = playerObject.transform;

			// set target to EnemyControl
			foreach (var enemyObject in enemyObjects) {
				var enemyControl = enemyObject.GetComponent<EnemyControl>();
				enemyControl.target = playerObject.transform;
			}
		}

		Global.stageControler = this;
		Global.paused = false;
	}

	void Update() {
		if (Global.paused && loadNextAt < Time.time) {
			Application.LoadLevel("Stage");
		}
	}

	public void PlayerGotTreasure() {
		numTreasure--;
		Global.score += 100;

		if (numTreasure == 0) {
			showGoal();
		}
	}

	public void PlayerHitGoal() {
		cameraObject.camera.fieldOfView = 40;
		Global.paused = true;
		loadNextAt = Time.time + 3;
	}

	private void showGoal() {
		foreach (var obj in hiddenObjects) {
			obj.SetActive(true);
		}
	}

	void BuildStage(string stageData) {
		numTreasure = 0;
		hiddenObjects = new List<GameObject>();
		enemyObjects = new List<GameObject>();
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
					switch (ch) {
					case PLAYER:
						playerObject = instance;
						break;
					case TREASURE:
						numTreasure++;
						break;
					case HIDDEN_LADDER:
					case GOAL:
						instance.SetActive(false);
						hiddenObjects.Add(instance);
						break;
					case ENEMY:
						enemyObjects.Add (instance);
						break;
					}
				}
				x++;
			}
		}
	}

	const string STAGE_DATA = @"


#        EhhhhhG 
#         H    #
# P       H    #
#         H    #
# $#HHHHHHH    #
#         H    #
#---------H    #
#        $H    #
####H###########
#   H--------- #
#   H          #
#   H ####     #
#   H----      #
#   H    ##### #
#   H          #
#   H        E #
################";
}

