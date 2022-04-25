using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Gradient gradient;
    [SerializeField] private float secondsPerDay = 10f;
    private Light2D light;

    private float dayTime;
    private float dayTimeSpeed;

    private void Awake()
    {
        light = GetComponent<Light2D>();

        dayTimeSpeed = 1f / secondsPerDay;
    }

    private void Update()
    {
        dayTime += Time.deltaTime * dayTimeSpeed;
        light.color = gradient.Evaluate(dayTime % 1f);
    }
}
