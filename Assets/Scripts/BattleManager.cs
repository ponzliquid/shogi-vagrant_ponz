using UnityEngine;
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

	// TODO 関数作れ：PieceAllocatorに1個だけ更新投げる関数

	void Start(){
	}
}
