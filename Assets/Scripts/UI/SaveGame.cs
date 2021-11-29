using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class SaveGame : MonoBehaviour
{

    public SaveData data;
    private void Start() {
        data = LoadGameData();
    }

    [Serializable]
    public class SaveData{
        // volum de so de les opcions
        internal float soundValue = 75;
        // volum de efectes de so de les opcions
        internal float chunkValue = 75;
        // Llunyanyia de camera
        internal int farCamera = 90;
        // FOV Horitzontal
        internal int farClipPlane = 75;
        // mon a carregar en pas de mainMenu a mon del joc, per default a 0 (primer mon)
        internal int levelCheckPointToRespawn = 0;
        // llenguatje
        // 0 = english
        // 1 = spanish
        internal string language = "EN";
        bool savedBool;
    }

    internal void UpdateSaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter(); 
        FileStream file = File.Create(Application.persistentDataPath 
                    + "/data.dat"); 
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
    }

    private SaveData LoadGameData()
    {
        if (File.Exists(Application.persistentDataPath 
                    + "/data.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = 
                    File.Open(Application.persistentDataPath 
                    + "/data.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            //Debug.Log("Game data loaded!");
            return data;
        }
        else
            Debug.LogError("There is no save data!");
        return new SaveData();
    }
}
