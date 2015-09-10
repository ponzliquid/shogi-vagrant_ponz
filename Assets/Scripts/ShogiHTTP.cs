using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShogiHTTP : SingletonMonoBehaviour<ShogiHTTP> {
	
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

	private IEnumerator WaitForRequest(WWW www) {
		yield return www;
		// check for errors
		if (www.error == null) {
			Debug.Log("WWW Ok!: " + www.text);
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
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log ("accessor test: " + LoginManager.Instance.playerName);
	}
}
