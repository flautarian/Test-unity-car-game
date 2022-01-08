using UnityEngine.UI;
using UnityEngine;

public class QualityDropdownController : MonoBehaviour
{
    Dropdown m_Dropdown;

    private void Start() {
        m_Dropdown = GetComponent<Dropdown>();
        if(m_Dropdown != null)
            m_Dropdown.value = GlobalVariables.Instance.GetQuality();
    }
    public void UpdateQuality(){
        GlobalVariables.Instance.UpdateQuality(m_Dropdown.value);
    }
}
