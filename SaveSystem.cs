using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; 

public static class SaveSystem
{
    public static void SaveGame(SaveManager sm){
		BinaryFormatter formatter = new BinaryFormatter();

		string path = Application.persistentDataPath + "/savefile.save";
		FileStream stream = new FileStream(path, FileMode.Create);

		SaveData data = new SaveData(sm);

		formatter.Serialize(stream, data);
		stream.Close();
	}

	public static SaveData LoadGame(){
		string path = Application.persistentDataPath + "/savefile.save";
		if(File.Exists(path)){
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			SaveData data = formatter.Deserialize(stream) as SaveData;
			stream.Close(); 

			return data;

		}
		else{
			Debug.Log("save file not found in " + path);
			return null;
		}
	}
	public static bool checkForFile(){
		string path = Application.persistentDataPath + "/savefile.save";
		if(File.Exists(path)){
			return true;

		}
		else{
			return false;
		}
	}
}
