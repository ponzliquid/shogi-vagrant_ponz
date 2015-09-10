using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginUI : MonoBehaviour {

	public void LoginToServerUI(){
		GameObject.Find ("BtnLogin").SetActive (false);
		LoginManager loginManager = this.GetComponent<LoginManager> ();
		loginManager.LoginToServer ();
	}

	public void LoggedInToServerUI(){

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
