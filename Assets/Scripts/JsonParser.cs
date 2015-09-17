using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class JsonParser {

	public static Dictionary<string, object> ParseNonNestedJson(string rawtext){
		Dictionary<string, object> dicParsed;
		dicParsed= Json.Deserialize(rawtext) as Dictionary<string, object>;
		return dicParsed;
	}
}