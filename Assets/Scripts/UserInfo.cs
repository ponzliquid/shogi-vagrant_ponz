using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserInfo : SingletonMonoBehaviour<UserInfo> {

	public string urlLogging { get; private set;}
	public string playerName { get; private set;}
	private Dictionary<string,object> userData = null;
	private Dictionary<string,object> roomState = null;

	public void SetLoggingURL(string url){
		urlLogging = url;
	}

	public void SetState(Dictionary<string, object> state){
		Debug.Log ("set state");
		roomState = state;
	}

	public void SetUserData(Dictionary<string,object> data){
		userData = data;
		Dictionary<string, object> dic = new Dictionary<string, object>();
		dic.Add("state",userData["state"]);
		SetState (dic);
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

	public bool IsStateDataNULL(){
		if(roomState == null){
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
		return roomState ["state"].ToString ();
	}

	private IEnumerator LogoutOnQuit(){
		if (UserInfo.Instance.IsUserDataNull ()) {
			yield break;
		}
		
		while (true) {
			ShogiHTTP.Instance.Logout ((string www) => {
				if(www == "[\"true\"]"){
					Debug.Log ("Success Logout");
					UserInfo.Instance.InitUserData ();
					return;
				}
			});
			if(!UserInfo.Instance.IsUserDataNull ()){
				yield return new WaitForSeconds(1);
				continue;
			}
		}
	}

	public void OnApplicationQuit(){
		// 自動ログアウト処理
		StartCoroutine (LogoutOnQuit());

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
