using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour {

	private Text textPlayerUserID, textPlayerPlayID, textPlayerRole;

	// Use this for initialization
	void Start () {
		textPlayerUserID = GameObject.Find ("TextPlayerUserID").GetComponent<Text> ();
		textPlayerPlayID = GameObject.Find ("TextPlayerPlayID").GetComponent<Text> ();
		textPlayerRole = GameObject.Find ("TextPlayerRole").GetComponent<Text> ();

		textPlayerUserID.text = "YourID: " + UserInfo.Instance.GetUserID().ToString();
		textPlayerPlayID.text = "PlayID: " + UserInfo.Instance.GetPlayID().ToString();
		textPlayerRole.text = "YourRole: " + UserInfo.Instance.GetRole();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
