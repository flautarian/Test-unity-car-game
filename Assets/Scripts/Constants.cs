using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{

    // Chunks

    public const string CHUNK_INIT_BLUE_CAR = "ArranqueMotor"; 

    public const string CHUNK_HIT_PLAYER = "PlayerHit";

    public const string CHUNK_HIT_COIN = "CoinHit";   

    public const string CHUNK_HIT_NITRO = "NitroHit";  
    public const string CHUNK_MOVE_UI_BUTTON = "UI_Move";  
    public const string CHUNK_OK_UI_BUTTON = "UI_Ok";  
    public const string CHUNK_BACK_UI_BUTTON = "UI_Back"; 

    public static readonly string[] DEFAULT_SCENE_SONGS = GenerateDefaultSceneSongList();

    // Stunts

    public static readonly string[] STUNT_NAMES = GenerateStuntNamesList();

    // Cars

    public static readonly string[] PREFAB_CAR_NAMES = GenerateCarPrefabNamesList();

    // Objectives

    public const string OBJECTIVE_LITERAL = "^detail_lvl_panel_objective_";

    // Numbers
    public const int MAX_BUTTONS_STUNTS_COUNT = 5;

    public const int OUTLINE_WITH_DISABLED = 0;

    public const int OUTLINE_WITH_ENABLED = 12;

    public const int MAX_VELOCITY_CARS = 8;

    public const int MAX_TURN_STRENGTH_CARS = 240;
    public const int MAX_GRAVITY_FORCE_CARS = 7;
    public const int MAX_DRAG_FORCE_CARS = 5;
    public const int MAX_STUNT_HABILITY_CARS = 2;
    public const int MAX_ACCEL_CARS = 15;


    // Terrain types
    public const string CESPED = "Cesped";
    public const string ASPHALT = "asphalt";
    public const string WATER = "water";
    public const string CANVAS_INTERACTION = "CanvasInteraction";

    // Particle system tagNames
    public const string PARTICLE_S_BOOM = "PS_Boom";
    public const string PARTICLE_S_LANDINGCAR = "LandingCarParticle";
    public const string PARTICLE_S_TURNUPCAR = "TurnUpCarParticle";
    public const string PARTICLE_S_HIT = "HitParticle";
    public const string PARTICLE_S_SMOKE_HIT = "SmokeHitParticle";

    // Game over types
    public const string GAME_OVER_VEHICLE_DROWNED = "Vehicle drowned";
    public const string GAME_OVER_LETHAL_OBS_COLLIDED = "collitioned with lethal object";
    public const string GAME_OVER_VEHICLE_DESTROYED = "Vehicle destroyed";


    // Animation names
    // player
    public const string ANIMATION_NAME_NITRO_BOOL = "nitro";
    public const string ANIMATION_NAME_HIT_BOOL = "hit";
    public const string ANIMATION_NAME_EXPLODE_BOOL = "explode";
    public const string ANIMATION_NAME_IS_IN_STUNT_BOOL = "isInStunt";
    public const string ANIMATION_NAME_CAST_STUNT_INT = "castStunt";
    public const string ANIMATION_NAME_STUNT_INDICATOR_BOOL = "StuntIndicatorEnabled";
    public const string ANIMATION_BUTTON_CONTROLLER_SELECTED_BOOL = "Selected";
    public const string ANIMATION_BUTTON_CONTROLLER_TRIGGERED_BOOL = "Triggered";

    
    // player parts
    public const string ANIMATION_START_ICON_ANIMATION = "StartIconAnimation";
    public const string ANIMATION_TRIGGER_TURN_UP = "TurnUp";
    public const string ANIMATION_START_PART_ANIMATION_BOOL = "action";
    // movable obstacle
    public const string ANIMATION_MOVABLE_OBSTACLE_RUN = "Run";
    public const string ANIMATION_MOVABLE_OBSTACLE_HIT_PARAM = "hit";
    public const string ANIMATION_MOVABLE_OBSTACLE_VELOCITY_PARAM = "velocity";
    // items
    public const string ANIMATION_NAME_TAKEN_BOOL = "hasBeenTaken";
    public const string ANIMATION_STREET_FALL_BOOL = "fall";

    // stunts

    public const string ANIMATION_NAME_STUNT_WRONG_ICON_BOOL = "wrong";
    public const string ANIMATION_NAME_STUNT_STUNT_COMPLETED_ICON_BOOL = "correct";

    // panels
    public const string ANIMATION_TRIGGER_PAUSEGAME_PANELS = "PauseGame";
    public const string ANIMATION_TRIGGER_GAMEOVER_PANELS = "GameOver";
    public const string ANIMATION_TRIGGER_GAMEWONPANELS = "GameWon";
    public const string ANIMATION_TRIGGER_TAX_PANEL_BUTTON = "TaxLevelSelect";
    public const string ANIMATION_TRIGGER_COMM_PANEL_BUTTON = "CommCenterSelect";
    public const string ANIMATION_TRIGGER_RELIC_PANEL_BUTTON = "RelicSelect";
    public const string ANIMATION_TRIGGER_CHALLENGE_INTERACTION = "ChallengeLvlSelect";
    public const string ANIMATION_TRIGGER_GO_BACK_PANEL_BUTTON = "GoGameFromPanel";
    public const string ANIMATION_TRIGGER_LIBRARY_PANEL_BUTTON = "LibrarySelect";
    public const string ANIMATION_TRIGGER_TURNED_PANEL_BUTTON = "Enable";
    public const string ANIMATION_TRIGGER_DISABLE_TURNED_PANEL_BUTTON = "Disable";
    public const string ANIMATION_TRIGGER_PANELBUTTON_ENABLE_INTERACTION = "EnableButton";
    public const string ANIMATION_TRIGGER_PANELBUTTON_DISABLE_INTERACTION = "DisableButton";
    public const string ANIMATION_TRIGGER_LIBRARY_INTERACTION = "LibraryEquipSelect";

    // Shader control variables

    public const string SHADER_CONTROL_STUNT_BAR_FILL_RATE = "_FillRate";
    public const string SHADER_CONTROL_STUNT_BAR_PROGRESS_BORDER = "_ProgressBorder";


    // Axis types and buttons

    public const int KEY_INPUT_UP = 0;
    public const int KEY_INPUT_DOWN = 1;
    public const int KEY_INPUT_LEFT = 2;
    public const int KEY_INPUT_RIGHT = 3;
    public const int KEY_INPUT_ACCELERATE = 4;
    public const int KEY_INPUT_STUNT = 5;

    public const string BACK = "Cancel";


    // game object tags
    public const string GO_TAG_GUI= "GUI";
    public const string GO_TAG_PLAYER = "Player";
    public const string GO_TAG_PLAYER_PART = "PlayerPart";
    public const string GO_TAG_CONTAINS_OBSTACULO = "Obstaculo";
    public const string GO_TAG_CONTAINS_GOALLINE = "GoalLine";
    public const string GO_TAG_STREET_CONTAINER = "StreetsContainer";
    public const string GO_TAG_PARTICLE_CONTAINER = "ParticlesContainer";
    public const string ANIMATION_TRIGGER_INIT_STUNT = "InitStunt";
    public const string GO_TAG_OBSTACLE_CONTAINER = "ObstaclesContainer";
    public const string GO_TAG_WAYPOINT = "WayPoint";
    public const string GO_TAG_PANEL_CANVAS_CONTAINER = "PanelsCanvas";


    // pool names
    public const string POOL_STREET_OBSTACLE = "StreetObstaculo";
    public const string POOL_BEREDA_OBSTACLE = "BeredaObstaculo";
    public const string POOL_ONE_TO_ONE_STREET = "oneToOneWayRoads";
    public const string POOL_TWO_TO_ONE_STREET = "twoToOneWayRoads";
    public const string POOL_TWO_TO_TWO_STREET = "twoToTwoWayRoads";
    public const string POOL_ONE_TO_TWO_STREET = "oneToTwoWayRoads";

    // functions for SendMessage
    public const string SEND_MESSAGE_FRONTAL_CAR_BUMPER_DETECTED = "frontalCarBumperDetected";

    private static string[] GenerateStuntNamesList(){
        return new string[20]{
            "Barrel Roll Left",
            "Barrel Roll Right",
            "Gainer",
            "Reversal Gainer",
            "Wind Strike",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""};
    }

    private static string[] GenerateDefaultSceneSongList(){
        return new string[4]{
            "MainMenu",
            "World1",
            "Endless1",
            "Endless1"};
    }

    private static string[] GenerateCarPrefabNamesList(){
        return new string[10]{
            "Blue Car",
            "Red Car",
            "Van Car",
            "",
            "",
            "",
            "",
            "",
            "",
            "",};
    }

}
