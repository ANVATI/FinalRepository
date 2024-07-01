using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections;

public class OptionsMenuController : MonoBehaviour
{
    public RectTransform optionsWindow;
    public CanvasGroup optionsCanvasGroup;
    public RectTransform ui_menu; 
    public CanvasGroup buttonsmenú;
    public GameObject block;
    private AudioSource _audio;
    public LibrarySounds menuClips;

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

    [Header("ImageFade")]
    public Image Fade;
    void Start()
    {
        Fade.DOFade(0, 4f);
        _audio = GetComponent<AudioSource>();
        optionsCanvasGroup.alpha = 0;
        optionsCanvasGroup.interactable = false;
        optionsCanvasGroup.blocksRaycasts = false;
        block.SetActive(false);
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
        _audio.PlayOneShot(menuClips.clipSounds[0]);
        _audio.PlayOneShot(menuClips.clipSounds[1]);
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
        _audio.PlayOneShot(menuClips.clipSounds[0]);
        _audio.PlayOneShot(menuClips.clipSounds[1]);
        block.SetActive(false);
        optionsCanvasGroup.interactable = false;
        optionsCanvasGroup.blocksRaycasts = false;
        optionsWindow.DOAnchorPos(hiddenPosition, 1f).SetEase(Ease.InOutBack);
        optionsCanvasGroup.DOFade(0, 1f);
    }
    public void SlideMenuLeft()
    {
        block.SetActive(true);
        _audio.PlayOneShot(menuClips.clipSounds[0]);
        _audio.PlayOneShot(menuClips.clipSounds[2]);
        ui_menu.DOAnchorPosX(visibleMenuPosition.x, 1.5f).SetEase(Ease.InSine);
    }

    public void SlideMenuRight()
    {
        block.SetActive(false);
        _audio.PlayOneShot(menuClips.clipSounds[0]);
        _audio.PlayOneShot(menuClips.clipSounds[2]);
        ui_menu.DOAnchorPosX(hiddenMenuPosition.x, 1.5f).SetEase(Ease.InSine);
    }
    public void MoveImages()
    {
        RectTransform canvasRect = optionsWindow.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        Vector3 middlePoint = canvasRect.position;

        if (!isMoved)
        {
            _audio.PlayOneShot(menuClips.clipSounds[0]);
            _audio.PlayOneShot(menuClips.clipSounds[3]);

            leftImage.rectTransform.DOMoveX(middlePoint.x, moveDuration).SetEase(Ease.InOutQuint);
            rightImage.rectTransform.DOMoveX(middlePoint.x, moveDuration).SetEase(Ease.InOutQuint);

            StartCoroutine(WaitForClip());
            isMoved = true;
            block.SetActive(true);
            OnImagesMoved?.Invoke();
        }
        else
        {
            Vector3 leftOriginalPosition = middlePoint - new Vector3(moveDistance / 2, 0, 0);
            Vector3 rightOriginalPosition = middlePoint + new Vector3(moveDistance / 2, 0, 0);
            leftImage.rectTransform.DOMoveX(leftOriginalPosition.x, moveDuration).SetEase(Ease.InOutQuint);
            rightImage.rectTransform.DOMoveX(rightOriginalPosition.x, moveDuration).SetEase(Ease.InOutQuint);

            isMoved = false;
        }
    }


    public void CloseGame()
    {
        StartCoroutine(WaitForClose());

    }
    IEnumerator WaitForClip()
    {
        yield return new WaitForSeconds(1.2f);
        _audio.PlayOneShot(menuClips.clipSounds[4]);
    }
    IEnumerator WaitForClose()
    {
        block.SetActive(true);
        _audio.PlayOneShot(menuClips.clipSounds[0]);
        Close.rectTransform.DOMoveY(Close.rectTransform.position.y - distanceY, duration).SetEase(Ease.InBack);
        yield return new WaitForSeconds(3f);
        Application.Quit();
    }
}
   
