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

//	public void GetPlayerInfoFromServer(){
//		// TODO まだShogiHTTPには完全なURLを送っている
//		ShogiHTTP.Instance.Player (UserInfo.Instance.urlLogging,
//		                                           (Dictionary<string, object> dicPlayerInfo) => {
//			Debug.Log("dicPlayerInfo[\"first_player\"] " + dicPlayerInfo["first_player"]);
////			BattleInfo.Instance.dataPlayerInfo = dicPlayerInfo;
//			BattleInfo.Instance.SetPlayerInfo(dicPlayerInfo);
//		});
//		Debug.Log ("completed get playerinfo");
//	}

	void Start(){
	}
}
