using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class OptionsMenuController : MonoBehaviour
{
    public RectTransform optionsWindow;
    public CanvasGroup optionsCanvasGroup;
    public RectTransform ui_menu; 
    public CanvasGroup buttonsmenú; 

    private Vector2 hiddenPosition;
    private Vector2 visiblePosition;
    private Vector2 hiddenMenuPosition;
    private Vector2 visibleMenuPosition;
    public float menuSlideDistance;

    [Header("Imagenes")]
    public Image leftImage;
    public Image rightImage;
    public Image Close;

    [Header("Distancia")]
    public float distanceY;
    public float duration;

    public float moveDistance; 
    public float moveDuration = 1f;

    private bool isMoved = false;


    public static event Action OnImagesMoved;
    void Start()
    {
        optionsCanvasGroup.alpha = 0;
        optionsCanvasGroup.interactable = false;
        optionsCanvasGroup.blocksRaycasts = false;

        buttonsmenú.alpha = 1;
        buttonsmenú.interactable = true;
        buttonsmenú.blocksRaycasts = true;

        hiddenPosition = new Vector2(optionsWindow.anchoredPosition.x, Screen.height);
        visiblePosition = optionsWindow.anchoredPosition;

        hiddenMenuPosition = ui_menu.anchoredPosition;
        visibleMenuPosition = hiddenMenuPosition - new Vector2(menuSlideDistance, 0f);

        optionsWindow.anchoredPosition = hiddenPosition;

    }

    public void OpenOptionsMenu()
    {
        optionsWindow.DOAnchorPos(visiblePosition, 1f).SetEase(Ease.InOutBack);
        optionsCanvasGroup.DOFade(1, 1f).OnComplete(() =>
        {
            optionsCanvasGroup.interactable = true;
            optionsCanvasGroup.blocksRaycasts = true;
        });
    }

    public void CloseOptionsMenu()
    {
        optionsCanvasGroup.interactable = false;
        optionsCanvasGroup.blocksRaycasts = false;
        optionsWindow.DOAnchorPos(hiddenPosition, 1f).SetEase(Ease.InOutBack);
        optionsCanvasGroup.DOFade(0, 1f);
    }
    public void SlideMenuLeft()
    {
        ui_menu.DOAnchorPosX(visibleMenuPosition.x, 1.5f).SetEase(Ease.OutSine);
    }

    public void SlideMenuRight()
    {
        ui_menu.DOAnchorPosX(hiddenMenuPosition.x, 1.5f).SetEase(Ease.InSine);
    }
    public void MoveImages()
    {
        if (!isMoved)
        {
            leftImage.rectTransform.DOMoveX(leftImage.rectTransform.position.x - moveDistance, moveDuration).SetEase(Ease.InOutQuint);
            rightImage.rectTransform.DOMoveX(rightImage.rectTransform.position.x + moveDistance, moveDuration).SetEase(Ease.InOutQuint);
            isMoved = true;

            OnImagesMoved?.Invoke();

        }
        else
        {
            leftImage.rectTransform.DOMoveX(leftImage.rectTransform.position.x + moveDistance, moveDuration).SetEase(Ease.InOutQuint);
            rightImage.rectTransform.DOMoveX(rightImage.rectTransform.position.x - moveDistance, moveDuration).SetEase(Ease.InOutQuint);
            isMoved = false;
        }
    }
    public void CloseGame()
    {
        Close.rectTransform.DOMoveY(Close.rectTransform.position.y - distanceY, duration).SetEase(Ease.InBack);
        Debug.Log("Saliendo del juego...");
    }

}
   
