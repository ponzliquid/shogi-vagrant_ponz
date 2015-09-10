using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserInfo : SingletonMonoBehaviour<UserInfo> {

	public string playerName { get; private set;}
	public string roomNumber { get; private set;}
	private Dictionary<string,object> userData;

	public void SetUserData(Dictionary<string,object> data){
		userData = data;
	}

	public void InitUserData(){
		userData = null;
	}

	public bool IsUserDataNull(){
		if(userData == null){
			return true;
		}else{
			return false;
		}
	}

	public int GetUserID(){
		return int.Parse(userData ["user_id"].ToString());
	}

	public int GetPlayID(){
		return int.Parse (userData ["play_id"].ToString ());
	}

	void Awake(){
		if(this != Instance){
			Destroy(this);
			return;
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
