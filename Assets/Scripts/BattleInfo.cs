using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleInfo : SingletonMonoBehaviour<BattleInfo> {

	public Dictionary<string, object> infoPlayer{ get; private set;}
	public Dictionary<string, object> infoOpponent { get; private set;}

	public bool IsPlayerInfoNull(){
		if(infoPlayer == null && infoOpponent == null){
			return true;
		}else{
			return false;
		}
	}

	public void SetPlayerInfo(Dictionary<string, object> info){
		infoPlayer = null;
		infoPlayer = info;
	}
	
	public void SetOpponentInfo(Dictionary<string, object> info){
		infoOpponent = null;
		infoOpponent = info;
	}

	void Awake(){
		if(this != Instance){
			Destroy(this);
			return;
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
