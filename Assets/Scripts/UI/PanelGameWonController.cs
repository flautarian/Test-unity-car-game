using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelGameWonController : MonoBehaviour
{
    public Button firstButton;

    [SerializeField]
    private MeshFilter prizeIcon;

    [SerializeField]
    private TextMesh explanation;

    [SerializeField]
    private Mesh[] prizeIcons;

    public void ExistenceCheck() {
        if(!GlobalVariables.Instance.IsLevelGameState())
                Destroy(this.gameObject);
    }

    private void OnEnable() {
        if(GlobalVariables.Instance != null){
            var eventSystem = EventSystem.current;
            if(firstButton != null) eventSystem.SetSelectedGameObject( firstButton.gameObject, new BaseEventData(eventSystem));
            LevelSettings.PrizeLevel prize = GlobalVariables.Instance.GetActualLevelPrizeType();
            var prizeDetail = GlobalVariables.Instance.actualLevelSettings.prizeDetail;
            explanation.text = "X " + prizeDetail;
            explanation.gameObject.SetActive(prize == LevelSettings.PrizeLevel.COINS);
            GlobalVariables.Instance.SucceessActualLevel();
            if(prize == LevelSettings.PrizeLevel.COINS){
                GlobalVariables.Instance.addCoins(prizeDetail);
            }
            else if(prize == LevelSettings.PrizeLevel.SCROLL){
                GlobalVariables.Instance.UnlockScroll(prizeDetail);
            }
        }
    }
}
