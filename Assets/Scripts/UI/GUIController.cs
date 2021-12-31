using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    
    public PlayerController playerController;
    public GasIndicator gasIndicator;
    public CoinsIndicator coinsIndicator;
    public CarPartsIndicator carPartsIndicator;
    public StuntsIndicator stuntsIndicator;
    private Animator animator;
    public Animator cmvStateDriveCameraAnimator;
    public bool forceStartGame = false;
    private float cameraXAxisOffset = 20;
    private float cameraYAxisOffset = 5;
    [SerializeField]
    private Transform ActiveUITransform;

    private Hashtable stuntsEarned = new Hashtable();

    private void Start()
    {
        // init de controlador de partes de coche;
        if (carPartsIndicator != null && playerController != null)
            carPartsIndicator.startGame(playerController.destructableParts.Count);

        animator = GetComponent<Animator>();

        // force startGame for edit actions
        if (forceStartGame) startGame();
        StartCoroutine(GlobalVariables.Instance.PlayDefaultSceneSong());
    }

        // Update is called once per frame
        void LateUpdate()
    {
        float playerAcceleration = 0;
        float playerBrake = 0;
        if (playerController != null)
        {
            playerAcceleration = playerController.VerticalAxis;
            playerBrake = playerController.HorizontalAxis;

            if (GlobalVariables.Instance.repairflag)
            {
                RecoverCarPlayerParts();
                GlobalVariables.Instance.repairflag = false;
            }
        }
        ControlIndicatorsRendering(Time.timeScale != 0f);
        //transform.LookAt(player.transform);
        ActiveUITransform.rotation = Quaternion.Euler(transform.rotation.x + (cameraXAxisOffset * playerAcceleration), transform.rotation.y + (cameraYAxisOffset * playerBrake), transform.rotation.z);
    }
    internal void propagueFisicButton(FisicButtonController fisicButtonController)
    {
        switch (fisicButtonController.actionButton)
        {
            case ActionButtonType.left:
                playerController.turnLeft();
                break;
            case ActionButtonType.right:
                playerController.turnRight();
                break;
            case ActionButtonType.brake:
                break;
        }
    }

    #region Triggers game init

    public void startGame()
    {
        if (cmvStateDriveCameraAnimator != null)
            cmvStateDriveCameraAnimator.Play("MainGameCamera");

        if(GlobalVariables.Instance.IsLevelGameState())
            if (gasIndicator != null) gasIndicator.startGame();

        if(playerController != null)
            playerController.startGame();        

        if (coinsIndicator != null)
            coinsIndicator.startGame();

        if(stuntsIndicator != null)
            stuntsIndicator.startGame();

    }

    private void ControlIndicatorsRendering(bool enable){
        gasIndicator.gameObject.SetActive(enable);
        coinsIndicator.gameObject.SetActive(enable);
        stuntsIndicator.gameObject.SetActive(enable);
        carPartsIndicator.gameObject.SetActive(enable);
    }


    public void startGameOver(String msg)
    {
        GlobalVariables.Instance.UpdateLevelState(InGamePanels.GAMELOST);
        GlobalVariables.Instance.ResetShaders();

        Debug.LogWarning("game ended by: " + msg);
        if (gasIndicator != null)
            gasIndicator.startGameOver();

        if (playerController != null)
            playerController.startGameOver();

        if (coinsIndicator != null)
            coinsIndicator.startGameOver();

        if (carPartsIndicator != null )
            carPartsIndicator.startGameOver();

        if(stuntsIndicator != null)
            stuntsIndicator.startGameOver();

        if (cmvStateDriveCameraAnimator != null)
            cmvStateDriveCameraAnimator.Play("GameOverAnimation");

        StartCoroutine(WaitUntilShowPanel(Constants.ANIMATION_TRIGGER_GAMEOVER_PANELS));
    }

    public void StartGameWon(){
        if(GlobalVariables.Instance.gameMode == GameMode.MAINMENU ||
        GlobalVariables.Instance.gameMode == GameMode.WOLRDMAINMENU) 
            return;
        GlobalVariables.Instance.UpdateLevelState(InGamePanels.GAMEWON);

        if (gasIndicator != null)
            gasIndicator.startGameOver();

        if (playerController != null)
            playerController.startGameOver();

        if (coinsIndicator != null)
            coinsIndicator.startGameOver();

        if (carPartsIndicator != null )
            carPartsIndicator.startGameOver();

        if(stuntsIndicator != null)
            stuntsIndicator.startGameOver();

        if (cmvStateDriveCameraAnimator != null)
        cmvStateDriveCameraAnimator.Play("GameWonAnimation");

        StartCoroutine(WaitUntilShowPanel(Constants.ANIMATION_TRIGGER_GAMEWONPANELS));
    }

    private IEnumerator WaitUntilShowPanel(String panelsTrigger){
        var panelCanvas = GameObject.FindGameObjectWithTag(Constants.GO_TAG_PANEL_CANVAS_CONTAINER);
        Animator animator = null;
        if(panelCanvas != null)
            animator = panelCanvas.GetComponent<Animator>();
        yield return new WaitForSeconds(6f);
        if(animator != null)animator.SetTrigger(panelsTrigger);

    }

    #endregion

    #region Bridge functions
    
    internal void AddSeconds(float value)
    {
        gasIndicator.AddSeconds(value);
    }

    internal void communicateNewStuntKeyPressed(int keyCode, bool groundedVehicle)
    {
        stuntsIndicator.communicateNewStuntKeyPressed(keyCode, groundedVehicle);
    }

    internal void communicateStuntInitialized(){
        animator.SetBool(Constants.ANIMATION_NAME_STUNT_INDICATOR_BOOL, true);
    }

    internal void communicateStuntReset(){
        stuntsIndicator.communicateStuntReset();
    }

    internal void rebootStuntKeys(){
        stuntsIndicator.rebootStuntKeys();
    }

    internal void communicateStuntClose(){
        animator.SetBool(Constants.ANIMATION_NAME_STUNT_INDICATOR_BOOL, false);
        stuntsIndicator.communicateStuntClose();
    }

    internal void RecoverCarPlayerParts()
    {
        if(playerController != null)
            playerController.RecoverParts();
        if(carPartsIndicator != null )
            carPartsIndicator.resetIndicator();
    }

    internal void InitStunt(Stunt stunt){
        playerController.InitStunt(stunt);
        if (stuntsEarned.Contains(stunt.stuntName))
        {
            stuntsEarned[stunt.stuntName] = (int)stuntsEarned[stunt.stuntName] + 1;
        }
        else stuntsEarned.Add(stunt.stuntName, 1);
    }

    #endregion
    
}