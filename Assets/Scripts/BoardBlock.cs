using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class BoardBlock : MonoBehaviour, IRecieveMessage,
							IPointerClickHandler{

	private Vector3 vec3;

	public void RecvUpdatePiecePos(int a, Dictionary<string, object> d){}

	public void RecvShowDestination(Vector3 v){

	}

	public void RecvUnshowDestination(){
		
	}

	public void OnPointerClick(PointerEventData ev){
		Debug.Log ("Block: clicked on pos; " + vec3);
	}

	void Start(){
		vec3 = this.GetComponent<RectTransform> ().localPosition;
	}
}
