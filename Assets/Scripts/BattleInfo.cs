using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleInfo : SingletonMonoBehaviour<BattleInfo> {

	public Dictionary<string, object> infoFirstPlayer{ get; private set;}
	public Dictionary<string, object> infoLastPlayer { get; private set;}

	public bool IsPlayerInfoNull(){
		if(infoFirstPlayer == null || infoLastPlayer == null){
			return true;
		}else{
			return false;
		}
	}

	public void SetFirstPlayerInfo(Dictionary<string, object> info){
		infoFirstPlayer = null;
		infoFirstPlayer = info;
	}
	
	public void SetLastPlayerInfo(Dictionary<string, object> info){
		infoLastPlayer = null;
		infoLastPlayer = info;
	}

	void Awake(){
		if(this != Instance){
			Destroy(this);
			return;
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
