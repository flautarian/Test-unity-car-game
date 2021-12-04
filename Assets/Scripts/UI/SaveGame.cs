using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class SaveGame : MonoBehaviour
{

    public SaveData data;

    public KeyCode[] keyCodeBindings = new KeyCode[6];
    private void Start() {
        data = LoadGameData();
        RefreshKeyCodeBindings();
        GlobalVariables.Instance.prepareSceneWithSaveGameParametters();
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
        // Idioma
        internal string language = "EN";
        // Sensibilitat de gir
        internal float hSensibility = 0.05f;

        // keyCodes
        // 0 - UP
        // 1 - DOWN
        // 2 - LEFT
        // 3 - RIGHT
        // 4 - ACCELERATE
        // 5 - STUNT MODE
        internal string[] keyBindings = {"W", "S", "A", "D", "Mouse0", "Mouse1"};
        bool savedBool;
    }

    internal void RefreshKeyCodeBindings(){
        for(int i = 0; i < data.keyBindings.Length; i++){
            keyCodeBindings[i] = (KeyCode) System.Enum.Parse(typeof(KeyCode), data.keyBindings[i]);
        }
    }

    internal KeyCode GetKeyCodeBinded(int key){
        return keyCodeBindings[key];
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
            Debug.LogError("There is no save data!, generating new dataFile");
        return new SaveData();
    }
}
