using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Honeti
{
    public class PanelPauseCanvasController : MonoBehaviour
    {
        [SerializeField]
        private Button firstButton;

        [SerializeField]
        private TextMesh objectiveText;

        [SerializeField]
        private I18NTextMesh objectiveI18N;

        [SerializeField]
        private GameObject objectivePanel;

        private void OnEnable() {
            var eventSystem = EventSystem.current;
            if(firstButton != null) eventSystem.SetSelectedGameObject( firstButton.gameObject, new BaseEventData(eventSystem));
            if(GlobalVariables.Instance.IsLevelGameState()){
                objectiveText.text = Constants.OBJECTIVE_LITERAL + (int)GlobalVariables.Instance.GetLevelObjective();
                if(objectiveI18N != null) objectiveI18N._updateParams(new string[]
                {"" + GlobalVariables.Instance.GetLevelObjectiveTarget(), GlobalVariables.Instance.GetLevelObjectiveEsp()});
                objectiveI18N.updateTranslation(true);
            }
            else objectivePanel.SetActive(false);
        }
    }
}
