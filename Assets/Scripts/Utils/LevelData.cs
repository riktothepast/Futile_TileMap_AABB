using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelData {

	TextAsset jsonFile;
	List<PlatformData> platforms;

	public LevelData(string pathToJson){
		jsonFile = Resources.Load(pathToJson) as TextAsset;

		platforms = new List<PlatformData> ();
		IDictionary dictionary = (IDictionary) Json.Deserialize (jsonFile.ToString());
		RXDebug.Log ("this is the json file: "+jsonFile.ToString());
		IList platform_list = (IList) dictionary ["Platforms"]; 
		setPlatformData (platform_list);
	}

	private void setPlatformData(IList platform_list){
		foreach (object platform in platform_list) {
			IDictionary temp_platform = (IDictionary)platform;
			double temp_x = (double) temp_platform ["x"] ;
			float x = (float)temp_x;
			double temp_y = (double) temp_platform ["y"] ;
			float y = (float)temp_y;
			string image = (string)temp_platform ["image"];
			platforms.Add (new PlatformData(x,y,image));
		}
	}

	public List<PlatformData> getPlatformData(){
		return platforms;
	}

}