using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    private float orthographicSize;
    private float targetOrthographicSize;

    private void Start()
    {
        orthographicSize = virtualCamera.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 moveDir = new Vector3(x, y).normalized;

        float moveSpeed = 30f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        float zoomAmount = 2f;
        targetOrthographicSize += -Input.mouseScrollDelta.y * zoomAmount;
        // Validation
        float minOrthograhpicSize = 10f;
        float maxOrthograhpicSize = 30f;
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthograhpicSize, maxOrthograhpicSize);
        // Smoothing (using lerp)
        float zoomSpeed = 5f;
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);

        virtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }
}
