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

    private void Awake() {
        data = LoadGameData();
        if(GlobalVariables.Instance != null)
            GlobalVariables.Instance.prepareSceneWithSaveGameParametters();
    }

    private void Start() {
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
        // Qualitat de video
        internal int quality = 0;
        // Indicador de pantalla completa
        internal bool fullscreen = false;
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

        internal Scroll[] scrolls = new Scroll[20];

        internal Level[] levels = new Level[20];
        internal float[] challenges = new float[10]{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f};
        internal int[] wheels = new int[20]{1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
        internal int[] hats = new int[20]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
        internal int[] cars = new int[10]{1,0,0,0,0,0,0,0,0,0};

        internal int[] relics = new int[20]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

        internal int[] equippedScrolls = new int[4];

        internal int equippedCar = 0;

        internal int equippedWheel = 0;

        internal int equippedHat = 0;

        internal int totalCoins = 0;
        bool savedBool;
    }

    internal void RefreshKeyCodeBindings(){
        for(int i = 0; i < data.keyBindings.Length; i++){
            keyCodeBindings[i] = (KeyCode) System.Enum.Parse(typeof(KeyCode), data.keyBindings[i]);
        }
    }

    private Scroll[] GenerateDefaultScrollList(){
        Scroll[] result = new Scroll[20];
        result[0] = new Scroll("Barrel Roll Left", false, false, 0, "^stunt_description_0", new int[]{3,3,-1,-1,-1});
        result[1] = new Scroll("Barrel Roll Right", false, false, 0, "^stunt_description_1", new int[]{2,2,-1,-1,-1});
        result[2] = new Scroll("Gainer", false, true, 1, "^stunt_description_2", new int[]{1,0,0,-1,-1});
        result[3] = new Scroll("Reversal gainer", false, true, 1, "^stunt_description_3", new int[]{0,1,1,-1,-1});
        result[4] = new Scroll("Wind Strike", false, true, 2, "^stunt_description_4", new int[]{2,1,3,-1,-1});
        result[5] = new Scroll("Air Savior", false, false, 1, "^stunt_description_5", new int[]{3,0,2,-1,-1});
        result[6] = new Scroll("Air Savior", false, false, 1, "^stunt_description_5", new int[]{3,0,2,-1,-1});
        result[7] = new Scroll("Air Savior", false, false, 1, "^stunt_description_5", new int[]{3,0,2,-1,-1});
        result[8] = new Scroll("Air Savior", false, false, 1, "^stunt_description_5", new int[]{3,0,2,-1,-1});
        result[9] = new Scroll("Air Savior", false, false, 1, "^stunt_description_5", new int[]{3,0,2,-1,-1});
        result[10] = new Scroll("Air Savior", false, false, 1, "^stunt_description_5", new int[]{3,0,2,-1,-1});
        return result;
    }

    private Level[] GenerateDefaultLevelsList(){
        Level[] result = new Level[20];
        result[0] = new Level(-1);
        result[1] = new Level(0);
        result[2] = new Level(1);
        result[3] = new Level(2);
        result[4] = new Level(3);
        result[5] = new Level(4);
        result[6] = new Level(5);
        result[7] = new Level(6);
        result[8] = new Level(7);
        result[9] = new Level(8);
        result[10] = new Level(9);
        result[11] = new Level(10);
        result[12] = new Level(11);
        result[13] = new Level(12);
        result[14] = new Level(13);
        result[15] = new Level(14);
        result[16] = new Level(15);
        result[17] = new Level(16);
        result[18] = new Level(17);
        result[19] = new Level(18);
        return result;
    }

    private int[] GenerateDefaultEquippedScrollsList(){
        return new int[4]{-1, -1, -1, -1};
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
        //Debug.Log("Game data saved!");
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
            data.scrolls = GenerateDefaultScrollList();
            return data;
        }
        else
            Debug.LogError("There is no save data!, generating new dataFile");
        // if not exists we create a new saveGame from default values
        SaveData newSaveData = new SaveData();
        newSaveData.scrolls = GenerateDefaultScrollList();
        newSaveData.levels = GenerateDefaultLevelsList();
        newSaveData.equippedScrolls = GenerateDefaultEquippedScrollsList();
        Debug.Log("lang:" +newSaveData.language);
        data = newSaveData;
        UpdateSaveGame();
        return newSaveData;
    }
}
