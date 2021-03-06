// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using System.Collections;
using UnityEngine;

public class StageModel
{
	//// Singleton
	private static StageModel instance = new StageModel(Global.stageUrl);

	private StageModel(string baseUrl) {
		this.baseUrl = baseUrl;
	}

	static public StageModel GetInstance() {
		return instance;
	}
	////

	private string baseUrl;

	public IEnumerator LoadStageData(int stageNum, Action<string> callback) {
		var url = String.Format("{0}/{1:D4}.txt", baseUrl, stageNum);
		var www = new WWW(url);
		Debug.Log(url);
		yield return www;
		callback(www.text);
	}

}

