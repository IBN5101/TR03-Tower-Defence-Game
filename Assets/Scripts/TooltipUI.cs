using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }

    [SerializeField] RectTransform canvasRectTransform;

    private RectTransform rectTransform;
    private TextMeshProUGUI tooltipText;
    private RectTransform backgroundRectTransform;

    private TooltipTimer tooltipTimer;

    private void Awake()
    {
        Instance = this;

        rectTransform = GetComponent<RectTransform>();
        tooltipText = transform.Find("text").GetComponent<TextMeshProUGUI>();
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();

        Hide();
    }

    private void Update()
    {
        HandleFollowMouse();
        // Timer
        if (tooltipTimer != null)
        {
            tooltipTimer.timer -= Time.deltaTime;
            if (tooltipTimer.timer <= 0)
            {
                Hide();
            }
        }
    }

    private void HandleFollowMouse()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

        if (anchoredPosition.x > canvasRectTransform.rect.width - backgroundRectTransform.rect.width)
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        if (anchoredPosition.y > canvasRectTransform.rect.height - backgroundRectTransform.rect.height)
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;

        rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetText(string text)
    {
        tooltipText.SetText(text);
        tooltipText.ForceMeshUpdate();

        Vector2 textSize = tooltipText.GetRenderedValues(false);
        Vector2 padding = new Vector2(8, 8);
        backgroundRectTransform.sizeDelta = textSize + padding;
    }

    public void Show(string text, TooltipTimer tooltipTimer = null)
    {
        this.tooltipTimer = tooltipTimer;

        gameObject.SetActive(true);
        SetText(text);
        HandleFollowMouse();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public class TooltipTimer
    {
        public float timer;
    }
}
