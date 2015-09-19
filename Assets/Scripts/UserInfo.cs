using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserInfo : SingletonMonoBehaviour<UserInfo> {

	// TODO セッター用意しなくてよくね？
	public string urlLogging { get; private set;}
	public string playerName { get; private set;}
	private Dictionary<string,object> userData;
	private Dictionary<string,object> roomState = null;

	public void SetLoggingURL(string url){
		urlLogging = url;
	}

	public void SetState(Dictionary<string, object> state){
		roomState = state;
	}

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

	public string GetUserRole(){
		return userData ["role"].ToString ();
	}

	public string GetState(){
		if (roomState == null) {
			return null;
		} else {
			return roomState ["state"].ToString ();
		}
	}

	public void LogoutFromServer(){
		if (UserInfo.Instance.IsUserDataNull ()) {
			return;
		}
		string logoutURL = UserInfo.Instance.urlLogging;
		string userID = UserInfo.Instance.GetUserID ().ToString();
		string playID = UserInfo.Instance.GetPlayID ().ToString();

		ShogiHTTP.Instance.Logout (logoutURL, playID, userID,
		                           (string str) => {
			if(str == "true"){
				Debug.Log ("Success Logout");
				UserInfo.Instance.InitUserData ();
				return;
			}
			Debug.LogError("Err: Failed Logout");
		});
	}

	public void OnApplicationQuit(){
		// 自動ログアウト処理


		Debug.Log ("Auto Logout");
	}

	void Awake(){
		if(this != Instance){
			Destroy(this);
			return;
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
