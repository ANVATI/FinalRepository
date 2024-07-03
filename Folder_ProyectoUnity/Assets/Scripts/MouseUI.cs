using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class MouseUI : MonoBehaviour
{
    private RectTransform rectTransform;
    private Button button;
    private EventTrigger eventTrigger;

    private void Awake()
    {
        eventTrigger = gameObject.AddComponent<EventTrigger>();
    }
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();

        button.onClick.AddListener(OnButtonClick);

        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((eventData) => { OnPointerEnter(); });
        eventTrigger.triggers.Add(pointerEnter);

        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((eventData) => { OnPointerExit(); });
        eventTrigger.triggers.Add(pointerExit);
    }

    public void OnPointerEnter()
    {
        rectTransform.DOScale(0.9f, 0.2f).SetEase(Ease.OutBack);
        //GetComponent<Image>().DOColor(Color.red, 0.2f);
    }

    public void OnPointerExit()
    {
        rectTransform.DOScale(0.85855f, 0.2f).SetEase(Ease.OutBack);
        //GetComponent<Image>().DOColor(Color.white, 0.8f);
    }

    private void OnButtonClick()
    {
        rectTransform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 10, 1);
    }
}