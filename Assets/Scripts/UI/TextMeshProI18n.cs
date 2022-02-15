using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Honeti;

public class TextMeshProI18n : MonoBehaviour
{
    
    TMP_Text textMeshPro;

    private void Awake() {
        textMeshPro = gameObject.GetComponent<TMP_Text>();
    }
    private void Start() {
        UpdateText();
    }
    
    public void UpdateText(){
        if(textMeshPro.text.Contains("^"))
            textMeshPro.text = I18N.instance.getValue(textMeshPro.text);
    }
}
