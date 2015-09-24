using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public interface IRecieveMessage : IEventSystemHandler {

	// 以下の名前の関数は、どのスクリプトのものであれ全て呼ばれることになる

	void RecvUpdatePiecePos(int a, Dictionary<string, object> d);

	void RecvShowDestination(Vector3 v);

	void RecvUnshowDestination();
}
