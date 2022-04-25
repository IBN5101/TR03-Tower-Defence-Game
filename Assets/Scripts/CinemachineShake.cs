using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    // Singleton pattern
    public static CinemachineShake Instance { get; private set; }

    private CinemachineVirtualCamera cmCamera;
    private CinemachineBasicMultiChannelPerlin channelPerlin;

    private float startingIntensity;
    private float timer;
    private float timerMax;

    private void Awake()
    {
        Instance = this;

        cmCamera = GetComponent<CinemachineVirtualCamera>();
        channelPerlin = cmCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (timer < timerMax)
        {
            timer += Time.deltaTime;
            float amplitude = Mathf.Lerp(startingIntensity, 0, timer / timerMax);
            channelPerlin.m_AmplitudeGain = amplitude;
        }
    }

    public void ShakeCamera(float intensity, float timerMax)
    {
        startingIntensity = intensity;
        channelPerlin.m_AmplitudeGain = startingIntensity;

        timer = 0f;
        this.timerMax = timerMax;
    }
}
