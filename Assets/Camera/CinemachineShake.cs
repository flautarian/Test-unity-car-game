using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (GlobalVariables.Instance.shakeParam > 0)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = GlobalVariables.Instance.shakeParam;
            GlobalVariables.Instance.shakeParam -= 0.5f;
        }
        else cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
    }
}
