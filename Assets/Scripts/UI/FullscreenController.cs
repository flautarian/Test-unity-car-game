using UnityEngine;
using UnityEngine.UI;

public class FullscreenController : MonoBehaviour
{
    private Toggle tog;

    void Start()
    {
        tog = GetComponent<Toggle>();
        //Add listener for when the state of the Toggle changes, to take action
        tog.onValueChanged.AddListener(delegate {
            ToggleValueChanged(tog);
        });
        tog.isOn = GlobalVariables.Instance.GetFullscreen();
    }
    void ToggleValueChanged(Toggle change)
    {
        GlobalVariables.Instance.UpdateFullscreen(tog.isOn);
        tog.isOn = GlobalVariables.Instance.GetFullscreen();
    }
}
