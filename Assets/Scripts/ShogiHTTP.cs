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

	// TODO wwwじゃなくてDicで返して
	public WWW Login(string url, string name, string room_no){
		WWWForm form = new WWWForm();
		form.AddField("name", name);
		form.AddField("room_no", room_no);
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
		return www;
	}
	
	public WWW Logout(string url, string play_id, string user_id){
		WWWForm form = new WWWForm();
		form.AddField("play_id", play_id);
		form.AddField("user_id", user_id);
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
		return www;
	}
	
	public WWW State(string url){
		WWW www = new WWW (url);
		StartCoroutine (WaitForRequest (www));
		return www;
	}

	public void Player(string url, ParsedJSON callback){
		url = url + "plays/" + UserInfo.Instance.GetPlayID ().ToString () + "/users";
		//return JsonParser.ParseNonNestedJson (GET (url).text);
		StartCoroutine (WaitForRequest (new WWW(url), callback));
	}

	public void Pieces(string url, ParsedJSON callback){
		url = url + "plays/" + UserInfo.Instance.GetPlayID ().ToString () + "/pieces";
		StartCoroutine (WaitForRequest(new WWW(url), callback));
	}

	private IEnumerator WaitForRequest(WWW www) {
		yield return www;
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
			Dictionary<string, object> wwwParsed = JsonParser.ParseNonNestedJson(www.text);
			callback(wwwParsed);
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
