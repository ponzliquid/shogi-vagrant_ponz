using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using MiniJSON;

public class LoginManager : SingletonMonoBehaviour<LoginManager> {

	public string playerName{get; private set;}
	public string roomNumber{get; private set;}
	public int user_id{get; private set;}
	public int play_id{ get; private set;}

	private int buffRoomNumber;

	private string loginURL, logoutURL;
	private const string LOCAL_HOST_URL = "http://192.168.33.11:3000/";
	private const string localloginURL = LOCAL_HOST_URL + "users/login";
	private const string locallogoutURL = LOCAL_HOST_URL + "users/logout";

	public void SetPlayerName(){
		Text name = GameObject.Find ("TxtPlayerName").GetComponent<Text>();
		playerName = name.text;
	}

	public void SetRoomNumber(){
		Text roomnum = GameObject.Find ("TxtRoomNumber").GetComponent<Text>();

	}

	public void SetIPAddress(){
		Text addr = GameObject.Find ("TxtIPAddr").GetComponent<Text>();
//		loginURL = addr.text;
	}

	public IEnumerator LoginToServer(){
		WWW www = ShogiHTTP.Instance.Login (loginURL, playerName, roomNumber);
		if (www.error == null) {
			Debug.Log("WWW Ok!");
		} else {
			Debug.Log("WWW Error:");
		}
		yield return www;
		JsonParser (www.text);
	}

	// 退室デバッグ用、後で消して
	public void LogoutFromServer(){
		WWW www;
		www = ShogiHTTP.Instance.Logout (logoutURL, playerName, roomNumber);
		if (www.error == null) {
			Debug.Log("WWW Ok!");
		} else {
			Debug.Log("WWW Error:");
		}
	}

	public void JsonParser(string wwwJson){
		var json = Json.Deserialize (wwwJson) as IDictionary;
		Debug.Log ("json[name]: " + json["user_id"]);
	}

	// デバッグ用、後で消して
	private void TestInit(){
		loginURL = localloginURL;
		logoutURL = locallogoutURL;
		roomNumber = "1";
		playerName = "tester";
	}

	public void Awake()
	{
		if(this != Instance){
			Destroy(this);
			return;
		}
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Use this for initialization
	void Start () {
		TestInit ();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
