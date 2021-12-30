using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController controller;

    public float reverseAccel;
    public float forwardAccel;
    public float accel;
    public float turnStrength;
    public float maxWheelTurn;
    public float gravityForce;
    public float dragGroundForce;

    public float stuntHability;

    public List<PlayerDestructablePart> parts;

    [SerializeField]
    internal AudioSource RunningCarAudioSource;

    [SerializeField]
    internal AudioSource SkidCarAudioSource;

    [SerializeField]
    internal AudioClip initCarChunk;
    [SerializeField]
    internal AudioClip runningCarChunk;
    [SerializeField]
    private float value;

    private void Awake() {
        SkidCarAudioSource.loop = true;
        RunningCarAudioSource.loop = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(controller != null)
        {
            controller.communicatePlayerBaseCollition(collision);
        }
        
    }

    public void UpdatePlayerCarInformation(Player newP){

        // Basic data
        forwardAccel = newP.forwardAccel;
        reverseAccel = newP.reverseAccel;
        turnStrength = newP.turnStrength;
        maxWheelTurn = newP.maxWheelTurn;
        gravityForce = newP.gravityForce;
        stuntHability = newP.stuntHability;
        accel = newP.accel;
        dragGroundForce = newP.dragGroundForce;

        // Destructable parts meshes
        for(int i =0; i < parts.Count; i++){
            MeshFilter mf = parts[i].GetComponent<MeshFilter>();
            MeshFilter newMf = newP.parts[i].GetComponent<MeshFilter>();
            mf.sharedMesh = newMf.sharedMesh;
        }
        // Main mesh
        MeshFilter mainMf = GetComponent<MeshFilter>();
        MeshFilter mainNewMf = newP.GetComponent<MeshFilter>();

        mainMf.sharedMesh = mainNewMf.sharedMesh;
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
