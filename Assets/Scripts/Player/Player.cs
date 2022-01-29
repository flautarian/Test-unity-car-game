using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    internal PlayerController controller;
    public CarController carController;
    public float stuntHability;
    [SerializeField]
    internal List<PlayerDestructablePart> parts;
    [SerializeField]
    internal AudioSource RunningCarAudioSource;
    [SerializeField]
    internal AudioSource SkidCarAudioSource;
    [SerializeField]
    internal AudioClip initCarChunk;
    [SerializeField]
    internal AudioClip runningCarChunk;
    [SerializeField]
    private MeshFilter mainMeshFilter;
    [SerializeField]
    private float value;

    private void Awake() {
        SkidCarAudioSource.loop = true;
        RunningCarAudioSource.loop = true;
        carController = GetComponent<CarController>();
    }

    public void UpdatePlayerCarInformation(CarInfo carInfo){

        stuntHability = carInfo.stuntHability;
        carController.idealRPM = carInfo.idealRPM;
        carController.maxRPM = carInfo.maxRPM;
        carController.turnRadius = carInfo.turnRadius;
        carController.torque = carInfo.torque;
        carController.brakeTorque = carInfo.brakeTorque;

        // Destructable parts meshes
        for(int i =0; i < parts.Count; i++){
            MeshFilter mf = parts[i].GetComponent<MeshFilter>();
            MeshFilter newMf = carInfo.parts[i].GetComponent<MeshFilter>();
            mf.sharedMesh = newMf.sharedMesh;
        }
        // Main mesh
        MeshFilter mainNewMf = carInfo.gameObject.GetComponent<MeshFilter>();
        mainMeshFilter.sharedMesh = mainNewMf.sharedMesh;

        transform.localScale = carInfo.gameObject.transform.localScale;
    }

    internal void PlayInitCarChunk(){
        if(RunningCarAudioSource != null)
            RunningCarAudioSource.PlayOneShot(initCarChunk);
    }

    internal void PlayRunningCarChunk(){
        if(RunningCarAudioSource != null){
            RunningCarAudioSource.clip = runningCarChunk;
            RunningCarAudioSource.Play();
        }
    }

    internal void UpdatePitchEngine(float vertical, float horizontal){
        if(RunningCarAudioSource != null){
            RunningCarAudioSource.pitch = Mathf.Lerp(RunningCarAudioSource.pitch, 0.25f + Math.Abs(vertical) - (Math.Abs(horizontal) / 2f), value * Time.deltaTime);
            RunningCarAudioSource.volume = GlobalVariables.Instance.GetChunkLevel();
        }
        if(SkidCarAudioSource != null && horizontal != 0){
            if(!SkidCarAudioSource.isPlaying) SkidCarAudioSource.Play();
            //SkidCarAudioSource.pitch = Math.Abs(horizontal);
            SkidCarAudioSource.volume = GlobalVariables.Instance.GetChunkLevel();
        }
        else SkidCarAudioSource.Pause();

    }

    internal void StartGameWon(){
        if(controller != null)
            controller.StartGameWon();
    }

}
