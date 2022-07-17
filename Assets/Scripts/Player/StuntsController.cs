using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntsController : MonoBehaviour
{
    public string[] buttons;

    public float allowedTimeBetweenButtons = 0.3f;
    private float timeLastButtonPressed;
    private float comboStunt;

    internal bool stuntsModeEnabled = false, trickMode = false;

    private bool[] stuntsKeysState = {true, true, true, true}; 

    private int keyPressed = -1;
    
    private int isStunting = -1;
    public StuntType actualStuntTricking = StuntType.NONE;

    private CarController carController;

    internal Animator playerAnimator;

    private GUIController guiController;
    
    [SerializeField]
    private ParticleSystem stuntComboPS;
    private ParticleSystem.EmissionModule stuntComboPSEmissionVar;
    private StuntAnimationOverriderController stuntAnimationOverriderController;

    [SerializeField]
    internal StuntComboIndicator stuntComboIndicator;

    private void Awake() {
        carController = GetComponent<CarController>();
        playerAnimator = GetComponent<Animator>();
        stuntAnimationOverriderController = GetComponent<StuntAnimationOverriderController>();
        GameObject gui = GameObject.FindGameObjectWithTag(Constants.GO_TAG_GUI);
        if (gui != null) 
            guiController = gui.GetComponent<GUIController>();
    }

    private void Start() {
        if(stuntComboPS != null)
            stuntComboPSEmissionVar = stuntComboPS.emission;
        timeLastButtonPressed = Time.time;
    }

    void Update() {
        if(carController.canMove && !carController.turned && GlobalVariables.Instance.inGameState == InGamePanels.GAMEON){
            if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_STUNT)) && !stuntsModeEnabled)
                    ActivateStuntMode();
            else if(stuntsModeEnabled){
                if(Input.GetKeyUp(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_STUNT)))
                    DeactivateStuntMode();
                else
                {
                    if (Time.time > timeLastButtonPressed + allowedTimeBetweenButtons) communicateStuntReset();
                    if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_UP))) keyPressed= 0;
                    else if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_DOWN))) keyPressed= 1;
                    else {
                        stuntsKeysState[0] = true;
                        stuntsKeysState[1] = true;
                        keyPressed = -1;
                    }
                    if(keyPressed < 0){
                        if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_RIGHT))) keyPressed= 2;
                        else if(Input.GetKeyDown(GlobalVariables.Instance.GetKeyCodeBinded(Constants.KEY_INPUT_LEFT))) keyPressed= 3;
                        else {
                            stuntsKeysState[2] = true;
                            stuntsKeysState[3] = true;
                            keyPressed = -1;
                        }
                    }
                    // invocar un icono de direccion que se ha pulsado para que se vea en pantalla                    
                    if(keyPressed > -1 && stuntsKeysState[keyPressed]){
                        stuntsKeysState[keyPressed] = false;
                        timeLastButtonPressed = Time.time;
                        communicateStuntKeyPressed(keyPressed);
                        keyPressed = -1;
                    }
                }
            }
                
        }
        else{
            stuntsModeEnabled = false;
        }
    }

    private void DeactivateStuntMode(){
        communicateStuntClose();
        stuntsModeEnabled = false;
        UpdatePlayerAnimationBool(Constants.ANIMATION_NAME_IS_IN_STUNT_BOOL, stuntsModeEnabled);
    }

    internal GUIController GetGUIController(){
        return guiController;
    }

    private void ActivateStuntMode(){
        if(GlobalVariables.Instance.actualPanelInteractionType == PanelInteractionType.NO_INTERACTION){
            communicateStuntInitialized();
            stuntsModeEnabled = true;
            UpdatePlayerAnimationBool(Constants.ANIMATION_NAME_IS_IN_STUNT_BOOL, stuntsModeEnabled);
        }
    }

    internal void updateTrickState(int state){
        isStunting = state;
    }

    internal void updateActualTrick(StuntType stuntType){
        actualStuntTricking = stuntType;
    }
    internal bool IsInStuntMode(){
        return isStunting > -1 && stuntsModeEnabled;
    }
    internal void UpdatePlayerAnimationBool(string animKey, bool newState){
        playerAnimator.SetBool(animKey, newState);
    }

    internal void UpdatePlayerAnimationInt(string animKey, int newState){
        playerAnimator.SetInteger(animKey, newState);
    }

    internal int GetPlayerAnimationInt(string animKey){
        return playerAnimator.GetInteger(animKey);
    }

    internal bool GetPlayerAnimationBoolState(string animKey){
        return playerAnimator.GetBool(animKey);
    }

    internal void TriggerAnimation(string triggerName){
        playerAnimator.SetTrigger(triggerName);
    }
    internal void UpdatePlayerAnimationSpeed(float stuntHability){
        playerAnimator.speed = stuntHability;
    }

    internal void communicateStuntKeyPressed(int keyCode){
        if(stuntsModeEnabled && !playerAnimator.GetBool(Constants.ANIMATION_NAME_HIT_BOOL))
            guiController.communicateNewStuntKeyPressed(keyCode, carController.grounded);
    }

    internal void communicateStuntInitialized(){
        if(guiController != null)
            guiController.communicateStuntInitialized();
    }

    internal void communicateStuntClose(){
        if(guiController != null)
            guiController.communicateStuntClose();
    }

    internal void communicateStuntReset(){
        if(guiController != null)
            guiController.communicateStuntReset();
    }

    internal bool InitStunt(Stunt stunt){
        GlobalVariables.Instance.AddObjectivePoint(stunt.groundStunt ? ObjectiveGameType.NUMBER_GROUNDAL_STUNTS : ObjectiveGameType.NUMBER_AERIAL_STUNTS, 1);
        if(stunt.stuntType != StuntType.NORMAL && stunt.units > GlobalVariables.Instance.totalStuntEC)
        {
            GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_HIT, transform.position);
            GlobalVariables.Instance.GetAndPlayChunk("UI_Ko", 1.0f);
            return false;
        }
        else if(stunt.stuntType == StuntType.NORMAL)
            GlobalVariables.Instance.addStuntEC(stunt.units);
        else
            GlobalVariables.Instance.substractStuntEC(stunt.units);
        GlobalVariables.Instance.GetAndPlayChunk(stunt.chunkName, 1.0f);
        GlobalVariables.RequestAndExecuteParticleSystem(Constants.PARTICLE_S_HIT, transform.position);
        SetAnimationOverriderControllerAnimation("StuntDefaultAnimation", stunt.GetAnimation());
        playerAnimator.SetTrigger(Constants.ANIMATION_TRIGGER_INIT_STUNT);
        playerAnimator.SetBool(Constants.ANIMATION_NAME_IS_IN_STUNT_BOOL, true);
        stuntComboPSEmissionVar.enabled = true;
        if(stuntComboIndicator != null)
            stuntComboIndicator.AddComboLevel();

        switch(stunt.comboKeys.Count){
            case 2:
                comboStunt = 25f;
            break;
            case 3:
                comboStunt = 50f;
            break;
            case 4:
                comboStunt = 100f;
            break;
            case 5:
                comboStunt = 200f;
            break;
            default:
            break;
        }
        return true;
    }

    public void ResetComboStunt(){
        comboStunt = 0f;
        if(stuntComboIndicator != null)
            stuntComboIndicator.ResetComboIndicator();
        stuntComboPSEmissionVar.enabled = false;
    }

    public float GetComboStunt(){
        return comboStunt;
    }

    public void SetAnimationOverriderControllerAnimation(string animationName, AnimationClip anim){
        stuntAnimationOverriderController.SetAnimation(animationName, anim);
    }

    public void DecrementGUIPart(){
        if (guiController != null && 
                guiController.carPartsIndicator != null)
            guiController.carPartsIndicator.decrementPart();
    }
}
