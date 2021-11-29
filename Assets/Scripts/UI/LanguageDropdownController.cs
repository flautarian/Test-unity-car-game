using UnityEngine.UI;
using UnityEngine;

public class LanguageDropdownController : MonoBehaviour
{
    Dropdown m_Dropdown;

    private void Start() {
        m_Dropdown = GetComponent<Dropdown>();
    }
    public void UpdateLanguage(){
        Debug.Log(m_Dropdown.options[m_Dropdown.value].text);
        GlobalVariables.Instance.UpdateActiveLanguage(m_Dropdown.options[m_Dropdown.value].text);
    }
}
