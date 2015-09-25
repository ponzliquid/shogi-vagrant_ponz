using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShogiHTTP : SingletonMonoBehaviour<ShogiHTTP> {

//	private WWW GET(string url){
//		WWW www = new WWW (url);
//		StartCoroutine (WaitForRequest (www));
//		return www;
//	}

	// 「この引数が返ってくるから」
	public delegate void ParsedJSON (Dictionary<string, object> dic);
	public delegate void OnlyString (string str);
	
	public void Login(string url, string name, string room_no, ParsedJSON callback){
		WWWForm form = new WWWForm();
		form.AddField("name", name);
		form.AddField("room_no", room_no);
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, callback));
	}
	
	public void Logout(OnlyString callback){
		string url = UserInfo.Instance.urlLogging + "users/logout";
		string play_id = UserInfo.Instance.GetPlayID ().ToString();
		string user_id = UserInfo.Instance.GetUserID ().ToString ();
		WWWForm form = new WWWForm();
		form.AddField("play_id", play_id);
		form.AddField("user_id", user_id);
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, callback));
	}
	
	public void UpdatePieces(string url, int pID, Dictionary<string, object> dic, ParsedJSON callback){
		string play_id = UserInfo.Instance.GetPlayID().ToString ();
		string user_id = UserInfo.Instance.GetUserID ().ToString ();
		string move_id = pID.ToString();
		string posx = dic ["posx"].ToString ();
		string posy = dic ["posy"].ToString ();
		string promote = dic ["promote"].ToString ();

		WWWForm form = new WWWForm();
		form.AddField("play_id", play_id);
		form.AddField("user_id", user_id);
		form.AddField("move_id", move_id);
		form.AddField("posx", posx);
		form.AddField("posy", posy);
		form.AddField("promote", promote);
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www, callback));
	}
	
	public void State(ParsedJSON callback){
		string url = UserInfo.Instance.urlLogging + "plays/"
					+ UserInfo.Instance.GetPlayID ().ToString () + "/state";
		WWW www = new WWW (url);
		StartCoroutine (WaitForRequest (www, callback));
	}

	public void Player(string url, ParsedJSON callback){
		url = url + "plays/" + UserInfo.Instance.GetPlayID ().ToString () + "/users";
		StartCoroutine (WaitForRequest (new WWW(url), callback));
	}

	public void Play(string url, ParsedJSON callback){
		url = url + "plays/" + UserInfo.Instance.GetPlayID ().ToString ();
		StartCoroutine (WaitForRequest(new WWW(url), callback));
	}

	public void Pieces(string url, ParsedJSON callback){
		url = url + "plays/" + UserInfo.Instance.GetPlayID ().ToString () + "/pieces";
		StartCoroutine (WaitForRequest(new WWW(url), callback));
	}


	private IEnumerator WaitForRequest(WWW www) {
		yield return www;
		Debug.Log ("www: " + www.text.ToString());
		if (www.error == null) {
			Debug.Log("WWW Ok!: " + www.text);
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}
	} 

	private IEnumerator WaitForRequest(WWW www, ParsedJSON callback) {
		yield return www;
		if (www.error == null) {
			Debug.Log("WWW Ok!: " + www.text);
			Dictionary<string, object> wwwParsed = JsonParser.ParseJson(www.text);
			callback(wwwParsed);
		} else {
			Debug.Log("WWW Error: "+ www.error);
			callback(null);
		}
	}

	private IEnumerator WaitForRequest(WWW www, OnlyString callback) {
		yield return www;
		if (www.error == null) {
			Debug.Log("WWW Ok!: " + www.text);
			callback(www.text);
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}
	}

	public void Awake()
	{
		if(this != Instance){
			Destroy(this);
			return;
		}
		DontDestroyOnLoad(this.gameObject);


	}
}
