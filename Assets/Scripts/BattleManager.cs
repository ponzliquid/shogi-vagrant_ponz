﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {

	public IEnumerator LogoutFromServer(string playID, string userID){
		
		string logoutURL = UserInfo.Instance.urlLogging + "users/logout";
		
		userID = UserInfo.Instance.GetUserID ().ToString();
		playID = UserInfo.Instance.GetPlayID ().ToString();
		WWW www;
		www = ShogiHTTP.Instance.Logout (logoutURL, playID, userID);
		Debug.Log ("Logging out...");
		yield return www;
		UserInfo.Instance.InitUserData ();
	}

	public void SetPlayerInfo(Dictionary<string, object> dicPlayerInfo){
		// TODO まだShogiHTTPには完全なURLを送っている
		foreach(KeyValuePair<string, object> pair in dicPlayerInfo){
			Dictionary<string, object> dic = pair.Value as Dictionary<string, object>;
			if(pair.Key.ToString() == "first_player"){
				BattleInfo.Instance.SetFirstPlayerInfo(dic);
				Debug.Log ("seting battle info: first");
			}
			else{
				BattleInfo.Instance.SetLastPlayerInfo(dic);
				Debug.Log ("seting battle info: last");
			}
		}
	}

	// TODO 関数作れ：foreachで回してPieceAllocatorに40個全部投げる関数
	public void SetPieceInfo(Dictionary<string, object> dicPiece){
		foreach(KeyValuePair<string, object> pair in dicPiece){
			Dictionary<string, object> dic = pair.Value as Dictionary<string, object>;
			// 一気に
		}
	}

	// TODO 関数作れ：PieceAllocatorに1個だけ更新投げる関数

	void Awake(){
		
	}

	void Start(){
		if (BattleInfo.Instance.infoFirstPlayer ["user_id"].ToString ()
		    == UserInfo.Instance.GetUserID ().ToString ()) {
			// 先手なら
			Debug.Log("先手");
			RectTransform rectTrans = GameObject.Find("Board").GetComponent<RectTransform>();
			rectTrans.rotation = Quaternion.Euler (0, 0, 180);
		} else {
			Debug.Log("後手");
		}
	}
}
