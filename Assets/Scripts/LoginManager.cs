using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using MiniJSON;

public class LoginManager : MonoBehaviour {

	private string playerName = null;
	private string roomNumber = null;
	private string userID = null;
	private string playID = null;
	private string loginURL, logoutURL;
	private const string LOCAL_HOST_URL = "http://192.168.33.11:3000/";
//	private const string REMOTE_HOST_URL = "http://***";
	private const string localloginURL = LOCAL_HOST_URL + "users/login";
	private const string locallogoutURL = LOCAL_HOST_URL + "users/logout";
	private Dictionary<string,object> download;

	public void SetPlayerName(){
		Text name = GameObject.Find ("TxtPlayerName").GetComponent<Text>();
		playerName = name.text;
	}

	public void SetRoomNumber(){
		Text roomnum = GameObject.Find ("TxtRoomNumber").GetComponent<Text>();
		roomNumber = roomnum.text;
	}

	public void SetIPAddress(){
		Text addr = GameObject.Find ("TxtIPAddr").GetComponent<Text>();
//		loginURL = addr.text;
	}

	public IEnumerator LoginToServer(){
		if(!UserInfo.Instance.IsUserDataNull()){
			Debug.Log("CAUTION: Already Logged in.");
			yield break;
		}
		else if(playerName == null){
			Debug.Log("CAUTION: Null Name.");
			yield break;
		}
		else if(roomNumber == null){
			Debug.Log("CAUTION: Null RoomNumber.");
			yield break;
		}
		WWW www = ShogiHTTP.Instance.Login (loginURL, playerName, roomNumber);
		yield return www;
		download = Json.Deserialize (www.text) as Dictionary<string,object>;
		yield return www.text;
		UserInfo.Instance.SetUserData (download);
	}

	// 退室デバッグ用、後で消して
	public IEnumerator LogoutFromServer(){
		if(UserInfo.Instance.IsUserDataNull()){
			Debug.Log("CAUTION: Already Logged out.");
			yield break;
		}
		userID = UserInfo.Instance.GetUserID ().ToString();
		Debug.Log ("userID from infodata: " + userID);
		playID = UserInfo.Instance.GetPlayID ().ToString();
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
		Debug.Log ("終了判定");
		while (!UserInfo.Instance.IsUserDataNull ()) {
			Debug.Log ("スタコル");
			yield return StartCoroutine (LogoutFromServer ());
		}
		Debug.Log ("Auto Logout");
	}

	public void Awake(){
	}
	
	void Start () {
		loginURL = localloginURL;
	}

	void Update () {
//		Debug.Log ("json[user_id]: " + download["user_id"]);
	}
}
