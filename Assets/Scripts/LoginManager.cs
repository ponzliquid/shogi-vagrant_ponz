using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using MiniJSON;

public class LoginManager : MonoBehaviour {
	
	private string userID = null;
	private string playID = null;

	private const string LOCAL_HOST_URL = "http://192.168.33.11:3000/";
//	private const string REMOTE_HOST_URL = "http://***";
	private const string localloginURL = LOCAL_HOST_URL + "users/login";
	private const string locallogoutURL = LOCAL_HOST_URL + "users/logout";
	private Dictionary<string,object> download;

	public IEnumerator LoginToServer(string playerName, string roomNumber){

		Debug.Log ("Logging in...");
		string loginURL = localloginURL;

		WWW www = ShogiHTTP.Instance.Login (loginURL, playerName, roomNumber);
		yield return www;
		download = Json.Deserialize (www.text) as Dictionary<string,object>;
		yield return www.text;
		UserInfo.Instance.SetUserData (download);
	}

	// 退室デバッグ用、後で消して
	public IEnumerator LogoutFromServer(string playID, string userID){

		string logoutURL = locallogoutURL;
		
		userID = UserInfo.Instance.GetUserID ().ToString();
		playID = UserInfo.Instance.GetPlayID ().ToString();
		//		Debug.Log ("userID from infodata: " + userID);
		WWW www;
		www = ShogiHTTP.Instance.Logout (logoutURL, playID, userID);
		Debug.Log ("終了なう");
		yield return www;
		UserInfo.Instance.InitUserData ();
	}
	
//	public Dictionary<string,object> JsonParser(string wwwDownloaded){
//		download = Json.Deserialize (wwwDownloaded) as Dictionary<string,object>;
////		Debug.Log ("json[\"user_id\"]: " + download["user_id"]);
//		return download;
//	}
	
	public IEnumerator OnApplicationQuit(){
		while (!UserInfo.Instance.IsUserDataNull ()) {
			Debug.Log ("スタコル");
			yield return StartCoroutine (
				LogoutFromServer (UserInfo.Instance.GetPlayID().ToString(), UserInfo.Instance.GetUserID().ToString()));
		}
		Debug.Log ("Auto Logout");
	}

	public void Awake(){
	}
	
	void Start () {
	}

	void Update () {
//		Debug.Log ("json[user_id]: " + download["user_id"]);
	}
}
