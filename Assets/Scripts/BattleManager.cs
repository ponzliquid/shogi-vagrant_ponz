using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour {

	public IEnumerator LogoutFromServer(string playID, string userID){
		
		string logoutURL = UserInfo.Instance.urlLogin + "users/logout";
		
		userID = UserInfo.Instance.GetUserID ().ToString();
		playID = UserInfo.Instance.GetPlayID ().ToString();
		WWW www;
		www = ShogiHTTP.Instance.Logout (logoutURL, playID, userID);
		Debug.Log ("Logging out...");
		yield return www;
		UserInfo.Instance.InitUserData ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
