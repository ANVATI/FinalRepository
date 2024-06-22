using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System;

public class OptionsMenuController : MonoBehaviour
{
    public RectTransform optionsWindow;
    public CanvasGroup optionsCanvasGroup;
    public RectTransform ui_menu;
    public CanvasGroup buttonsmenú;
    public GameObject block;
    private AudioSource _audio;
    public LibrarySounds clips;

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
        _audio = GetComponent<AudioSource>();
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
        _audio.PlayOneShot(clips.clipSounds[1]);
        block.SetActive(true);
        optionsWindow.DOAnchorPos(visiblePosition, 1f).SetEase(Ease.InOutBack);
        optionsCanvasGroup.DOFade(1, 1f).OnComplete(() =>
        {
            optionsCanvasGroup.interactable = true;
            optionsCanvasGroup.blocksRaycasts = true;
        });
    }

    public void CloseOptionsMenu()
    {
        _audio.PlayOneShot(clips.clipSounds[1]);
        block.SetActive(false);
        optionsCanvasGroup.interactable = false;
        optionsCanvasGroup.blocksRaycasts = false;
        optionsWindow.DOAnchorPos(hiddenPosition, 1f).SetEase(Ease.InOutBack);
        optionsCanvasGroup.DOFade(0, 1f);
    }

    public void SlideMenuLeft()
    {
        _audio.PlayOneShot(clips.clipSounds[0]);
        _audio.PlayOneShot(clips.clipSounds[4]);
        ui_menu.DOAnchorPosX(visibleMenuPosition.x, 1.5f).SetEase(Ease.InSine);
    }

    public void SlideMenuRight()
    {
        _audio.PlayOneShot(clips.clipSounds[0]);
        _audio.PlayOneShot(clips.clipSounds[4]);
        ui_menu.DOAnchorPosX(hiddenMenuPosition.x, 1.5f).SetEase(Ease.InSine);
    }

    public void MoveImages()
    {
        if (!isMoved)
        {
            _audio.PlayOneShot(clips.clipSounds[0]);
            _audio.PlayOneShot(clips.clipSounds[2]);
            leftImage.rectTransform.DOMoveX(leftImage.rectTransform.position.x - moveDistance, moveDuration).SetEase(Ease.InOutQuint);
            rightImage.rectTransform.DOMoveX(rightImage.rectTransform.position.x + moveDistance, moveDuration).SetEase(Ease.InOutQuint);
            StartCoroutine(WaitforClip());
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
        _audio.PlayOneShot(clips.clipSounds[0]); 
        Close.rectTransform.DOMoveY(Close.rectTransform.position.y - distanceY, duration).SetEase(Ease.InBack);
        Debug.Log("Saliendo del juego...");
    }

    IEnumerator WaitforClip()
    {
        yield return new WaitForSeconds(1.2f);
        _audio.PlayOneShot(clips.clipSounds[3]);
    }
}
