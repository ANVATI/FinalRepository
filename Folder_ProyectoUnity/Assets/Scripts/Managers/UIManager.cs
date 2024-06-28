using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("UI_Vida y Stamina")]
    public Image staminaBarImage;
    public Image lifeBarImage;
    public Slider RageBar;
    public GameObject slider;
    public PlayerController playerController;
    public PlayerActions playerAction;

    [Header("UI_Dialogos NPC y enemigos")]
    public Dialogue dialogue;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;
    public Image dialogueImage1;
    public GameObject dialoguePanel;
    private int currentDialogueIndex = 0;

    [Header("Options")]
    public GameObject ApperOptions;
    public GameObject VolumeSettings;
    public bool isOpen = false;

    [Header("Fade")]
    [SerializeField] private Image fadeImage;

    private void Start()
    {
        dialoguePanel.SetActive(false);
        ApperOptions.SetActive(false);
        VolumeSettings.SetActive(false);

        if (playerController == null)
        {
            playerController = PlayerController.Instance;
        }
    }

    private void Update()
    {
        if (playerController != null)
        {
            UpdateStaminaBar();
            UpdateLifeBar();
            UpdateRageBar();
        }
    }

    public void NextDialogue(InputAction.CallbackContext context)
    {
        if (context.performed && dialoguePanel.activeSelf)
        {
            DisplayNextDialogue();
        }
    }

    public void OpenOptions(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (ApperOptions.activeSelf)
            {
                ApperOptions.SetActive(false);
                VolumeSettings.SetActive(false);
                Time.timeScale = 1;
                isOpen = false;
            }
            else
            {
                ApperOptions.SetActive(true);
                Time.timeScale = 0;
                isOpen = true;
            }
        }
    }

    public void CloseOption()
    {
        ApperOptions.SetActive(false);
        Time.timeScale = 1;
        isOpen = false;
    }

    public void OpenVolumeSettings()
    {
        VolumeSettings.SetActive(true);
        ApperOptions.SetActive(false);
    }

    public void CloseVolumeSettings()
    {
        ApperOptions.SetActive(true);
        VolumeSettings.SetActive(false);
    }

    private void UpdateStaminaBar()
    {
        if (staminaBarImage != null && playerController != null)
        {
            float fillAmount = playerController.playerAttributes.Stamina / 100f;
            staminaBarImage.fillAmount = fillAmount;
        }
    }

    private void UpdateLifeBar()
    {
        if (lifeBarImage != null && playerController != null)
        {
            float fillAmount = playerController.playerAttributes.Life / 100f;
            lifeBarImage.fillAmount = fillAmount;
        }
    }

    private void UpdateRageBar()
    {
        RageBar.value = playerController.playerAction.GetEnemyKillCount();

        if (playerController.playerAction.inRageMode)
        {
            slider.SetActive(true);
            RageBar.value = playerController.playerAction.GetRemainingRageDuration();
        }
        else
        {
            slider.SetActive(false);
        }
    }

    public void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        currentDialogueIndex = 0;
        DisplayNextDialogue();
    }

    public void DisplayNextDialogue()
    {
        if (currentDialogueIndex < dialogue.dialogueEntries.Count)
        {
            characterNameText.text = dialogue.dialogueEntries[currentDialogueIndex].characterName;
            dialogueText.text = dialogue.dialogueEntries[currentDialogueIndex].dialogueText;
            dialogueImage1.sprite = dialogue.dialogueEntries[currentDialogueIndex].dialogueImage1;
            currentDialogueIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }

    public void Fade()
    {
        fadeImage.DOFade(1, 5);
    }
}
