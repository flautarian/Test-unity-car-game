using UnityEngine.UI;
using UnityEngine;

public class LanguageDropdownController : MonoBehaviour
{
    Dropdown m_Dropdown;

    private void Start() {
        m_Dropdown = GetComponent<Dropdown>();
        if(m_Dropdown != null){
            switch(GlobalVariables.Instance.GetLanguage()){
                case "ES":
                    m_Dropdown.value = 0;
                break;
                default:
                    m_Dropdown.value = 1;
                break;
            }
        }
    }
    public void UpdateLanguage(){
        GlobalVariables.Instance.UpdateActiveLanguage(m_Dropdown.options[m_Dropdown.value].text);
    }
}
