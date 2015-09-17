using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PieceSubject : MonoBehaviour {

	private const string PATH_SPRITE_PIECE_64 = "Koma";
	private const int POSX_DEF = 200, POSY_DEF = 200;

	// execute on instantiating
	public static PieceSubject Instantiate(PieceSubject prefab, Dictionary<string, object> dic){
		PieceSubject obj = Instantiate(prefab) as PieceSubject;

		Transform canvasObj = GameObject.Find("Canvas").GetComponent<Transform>();
		obj.transform.SetParent (canvasObj,false);

		Sprite spr = Resources.Load<Sprite>(PATH_SPRITE_PIECE_64 + "/" + dic["name"].ToString());
		obj.GetComponent<Image> ().overrideSprite = spr;

		obj.name = dic["name"].ToString();

		RectTransform rectTrans = obj.GetComponent<RectTransform> ();
		float buffX = 200f - 50f * (float.Parse(dic ["posx"].ToString()) - 1f);
		float buffY = 200f - 50f * (float.Parse(dic ["posy"].ToString()) - 1f);
		rectTrans.localPosition = new Vector3(buffX, buffY,0);

		if (dic ["owner"].ToString () != UserInfo.Instance.GetUserID ().ToString ()) {
			Debug.Log("P(name,x,y,own) =(" + dic["name"].ToString() + ", " + rectTrans.localPosition.x.ToString() + ", "
			          + rectTrans.localPosition.y + ", "
			          + dic["owner"].ToString() + ")" + "ひっくり返して");
		}

		return obj;
	}
}
