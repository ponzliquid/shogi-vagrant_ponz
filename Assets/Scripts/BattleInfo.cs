using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleInfo : SingletonMonoBehaviour<BattleInfo> {

	public Dictionary<string,Dictionary<string, object>> dataPlayerInfo{ get; private set;}

	public bool IsPlayerInfoNull(){
		if(dataPlayerInfo == null){
			return true;
		}else{
			return false;
		}
	}

	public void SetPlayerInfo(Dictionary<string, Dictionary<string, object>> playerInfo){
		dataPlayerInfo = null;
		dataPlayerInfo = playerInfo;
	}

	void Awake(){
		if(this != Instance){
			Destroy(this);
			return;
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
