using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class BoardBlock : MonoBehaviour, IRecieveMessage,
							IPointerClickHandler{

	private Vector3 vec3;

	private Vector3 clickedPosOnPiece;
	private BattleManager battleManager;

	public void RecvUpdatePiecePos(int a, Dictionary<string, object> d){}

	public void RecvRememberSelectedPiece(Vector3 v){
		Debug.Log ("remember: " + v);
		clickedPosOnPiece = v;
	}

	public void RecvShowDestination(Vector3 v){

	}

	public void RecvUnshowDestination(){
		
	}

	public void OnPointerClick(PointerEventData ev){
		Debug.Log ("Block: clicked on pos; " + vec3);

		ExecuteEvents.Execute<IRecieveMessage>(
			gameObject,
			null,
			(recieveTarget,y)=>recieveTarget.RecvMoveToDestination(vec3));

		SendMessage("RecvMoveToDestination", vec3);

		battleManager.MovePieceToThisPos (vec3);
	}

	public void RecvMoveToDestination (Vector3 v){

	}

	void Start(){
		vec3 = this.GetComponent<RectTransform> ().localPosition;

		battleManager = GameObject.Find ("BattleManager").GetComponent<BattleManager> ();
	}
}
