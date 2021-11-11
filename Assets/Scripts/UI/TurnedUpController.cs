using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnedUpController : MonoBehaviour
{
    // Start is called before the first frame update
    Camera m_MainCamera;
    void Start()
    {
        
    }

    private void OnEnable() {
        m_MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(m_MainCamera.transform.position);
    }
}
