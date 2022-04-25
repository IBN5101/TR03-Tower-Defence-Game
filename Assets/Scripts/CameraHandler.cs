using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    // Singleton pattern
    public static CameraHandler Instance { get; private set; }

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private PolygonCollider2D cameraBoundsCollider2D;
    private float orthographicSize;
    private float targetOrthographicSize;
    private bool edgeScrolling;

    private void Awake()
    {
        Instance = this;

        // PlayerPrefs does not have bool, so int (0 or 1) is used
        edgeScrolling = (PlayerPrefs.GetInt("edgeScrolling", 0) == 1);
    }

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
        // Keyboard
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        // Mouse (edge-scrolling)
        if (edgeScrolling)
        {
            float edgeScrollingSize = 30f;
            if (Input.mousePosition.x > Screen.width - edgeScrollingSize)
                x = +1f;
            if (Input.mousePosition.x < edgeScrollingSize)
                x = -1f;
            if (Input.mousePosition.y > Screen.height - edgeScrollingSize)
                y = +1f;
            if (Input.mousePosition.y < edgeScrollingSize)
                y = -1f;
        }

        Vector3 moveDir = new Vector3(x, y).normalized;

        float moveSpeed = 30f;
        Vector3 movementVector = transform.position + (moveDir * moveSpeed * Time.deltaTime);
        if (cameraBoundsCollider2D.bounds.Contains(movementVector))
        {
            transform.position = movementVector;
        }
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

    public bool GetEdgeScrolling()
    {
        return edgeScrolling;
    }

    public void SetEdgeScrolling(bool edgeScrolling)
    {
        this.edgeScrolling = edgeScrolling;
        PlayerPrefs.SetInt("edgeScrolling", edgeScrolling ? 1 : 0);
    }
}
