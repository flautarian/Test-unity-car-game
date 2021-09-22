using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{

    // Terrain types
    public const string CESPED = "Cesped";
    public const string ASPHALT = "asphalt";
    public const string WATER = "water";

    // Particle system tagNames
    public const string PARTICLE_S_BOOM = "PS_Boom";
    public const string PARTICLE_S_LANDINGCAR = "LandingCarParticle";
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

    
    // player parts
    public const string ANIMATION_START_ICON_ANIMATION = "StartIconAnimation";
    public const string ANIMATION_START_PART_ANIMATION_BOOL = "action";
    // movable obstacle
    public const string ANIMATION_MOVABLE_OBSTACLE_RUN = "Run";
    public const string ANIMATION_MOVABLE_OBSTACLE_HIT_PARAM = "hit";
    public const string ANIMATION_MOVABLE_OBSTACLE_VELOCITY_PARAM = "velocity";
    // items
    public const string ANIMATION_NAME_TAKEN_BOOL = "hasBeenTaken";


    // Axis types
    public const string AXIS_VERTICAL = "Vertical";
    public const string AXIS_HORIZONTAL= "Horizontal";
    public const string INPUT_ACCELERATE = "Accelerate";
    public const string INPUT_FIRE = "Fire1";


    // game object tags
    public const string GO_TAG_GUI= "GUI";
    public const string GO_TAG_PLAYER = "Player";
    public const string GO_TAG_PLAYER_PART = "PlayerPart";
    public const string GO_TAG_CONTAINS_OBSTACULO = "Obstaculo";
    public const string GO_TAG_STREET_CONTAINER = "StreetsContainer";
    public const string GO_TAG_PARTICLE_CONTAINER = "ParticlesContainer";
    public const string GO_TAG_OBSTACLE_CONTAINER = "ObstaclesContainer";
    public const string GO_TAG_WAYPOINT = "WayPoint";


    // pool names
    public const string POOL_STREET_OBSTACLE = "StreetObstaculo";
    public const string POOL_BEREDA_OBSTACLE = "BeredaObstaculo";
    public const string POOL_ONE_TO_ONE_STREET = "oneToOneWayRoads";
    public const string POOL_TWO_TO_ONE_STREET = "twoToOneWayRoads";
    public const string POOL_TWO_TO_TWO_STREET = "twoToTwoWayRoads";
    public const string POOL_ONE_TO_TWO_STREET = "oneToTwoWayRoads";

    // functions for SendMessage
    public const string SEND_MESSAGE_FRONTAL_CAR_BUMPER_DETECTED = "frontalCarBumperDetected";



}
