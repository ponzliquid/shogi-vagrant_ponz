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

	public void FetchPlayerInfo(Dictionary<string, object> dicPlayerInfo){
		// TODO まだShogiHTTPには完全なURLを送っている
		Debug.Log("dicPlayerInfo[\"first_player\"] " + dicPlayerInfo["first_player"]);
//		BattleInfo.Instance.SetPlayerInfo(dicPlayerInfo);
		foreach(KeyValuePair<string, object> pair in dicPlayerInfo){
			Dictionary<string, object> dic = pair.Value as Dictionary<string, object>;
			if(dic["user_id"].ToString() == UserInfo.Instance.GetUserID().ToString()){
				BattleInfo.Instance.SetPlayerInfo(dic);
			}
			else{
				BattleInfo.Instance.SetOpponentInfo(dic);
			}
		}
	}

	// TODO 関数作れ：foreachで回してPieceAllocatorに40個全部投げる関数

	// TODO 関数作れ：PieceAllocatorに1個だけ更新投げる関数

	void Start(){
	}
}
