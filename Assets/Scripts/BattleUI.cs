using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleUI : MonoBehaviour {

	private Text textPlayerUserID, textPlayerPlayID, textPlayerRole;
	private BattleManager battleManager;

	public void LogoutFromServerUI(){
		GameObject.Find ("BtnLogout").SetActive (false);
		StartCoroutine(battleManager.LogoutFromServer (
					UserInfo.Instance.GetPlayID().ToString(), UserInfo.Instance.GetUserID().ToString()));
		StartCoroutine(LoggedOutFromServerUI());
	}

	private IEnumerator LoggedOutFromServerUI(){
		while (!UserInfo.Instance.IsUserDataNull()) {
			yield return new WaitForEndOfFrame();
		}
		Application.LoadLevel ("Lobby");
	}
	
	// Use this for initialization
	void Start () {
		textPlayerUserID = GameObject.Find ("TextPlayerUserID").GetComponent<Text> ();
		textPlayerPlayID = GameObject.Find ("TextPlayerPlayID").GetComponent<Text> ();
		textPlayerRole = GameObject.Find ("TextPlayerRole").GetComponent<Text> ();

		battleManager = GameObject.Find ("BattleManager").GetComponent<BattleManager> ();

		textPlayerUserID.text = "YourID: " + UserInfo.Instance.GetUserID().ToString();
		textPlayerPlayID.text = "PlayID: " + UserInfo.Instance.GetPlayID().ToString();
		textPlayerRole.text = "YourRole: " + UserInfo.Instance.GetRole();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
