// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using UnityEngine;
using System;


public class GoalControl : MonoBehaviour
{
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == Tag.PLAYER) {
			Global.stageControler.PlayerHitGoal();
		}
	}
}
