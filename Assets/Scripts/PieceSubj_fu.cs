using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

abstract class PieceMove {

	public Vector3 CalcDestination (){
		Vector3 vec3 = Vector3.zero;

		return vec3;
	}
	
	void ReqShowDestination(){

	}

	void ReqUnshowDestination(){

	}
	
	void ReqRememberSelectedPiece(){
		
	}
}
