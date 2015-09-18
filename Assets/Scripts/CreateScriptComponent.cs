using UnityEngine;
using System;
using System.Collections;

public class CreateScriptComponent : MonoBehaviour {

	public static GameObject Create(string className){
		GameObject obj;
//		var type = Type.GetType (className);
		obj = GameObject.Find(className);
		obj.AddComponent(System.Type.GetType(className));
		return obj;
	}
}
