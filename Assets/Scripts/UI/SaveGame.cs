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
        data = LoadGame();
    }

    [Serializable]
    public class SaveData{
        // volum de so de les opcions
        internal float soundValue;
        // volum de efectes de so de les opcions
        internal float chunkValue;
        // mon a carregar en pas de mainMenu a mon del joc, per default a 0 (primer mon)
        internal int levelCheckPointToRespawn = 0;
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

    private SaveData LoadGame()
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
            Debug.Log("Game data loaded!");
            return data;
        }
        else
            Debug.LogError("There is no save data!");
        return new SaveData();
    }
}