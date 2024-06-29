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
    public TextMeshProUGUI goblinCountText;
    public TextMeshProUGUI vikingCountText;
    private int currentDialogueIndex = 0;
    private int goblinCount = 11;
    private int vikingCount = 10;
    private bool goblinDead = false;

    [Header("Options")]
    public GameObject ApperOptions;
    public GameObject VolumeSettings;
    public bool isOpen = false;

    [Header("Fade")]
    [SerializeField] private Image fadeImage;

    [Header("Enemy")]
    [SerializeField] private GameObject Goblin_ui;
    [SerializeField] private GameObject Viking_ui;

    private void Start()
    {
        goblinDead = false;
        dialoguePanel.SetActive(false);
        ApperOptions.SetActive(false);
        VolumeSettings.SetActive(false);

        if (playerController == null)
        {
            playerController = PlayerController.Instance;
        }

        UpdateEnemyCount();
    }

    private void OnEnable()
    {
        HerenciaEnemy.OnEnemyKilled += UpdateEnemyCount;
        playerAction.OnGoblinArea += ShowGoblinImage;
        playerAction.OnAldeaArea += ShowVikingImage;
    }

    private void OnDisable()
    {
        HerenciaEnemy.OnEnemyKilled -= UpdateEnemyCount;
        playerAction.OnGoblinArea -= ShowGoblinImage;
        playerAction.OnAldeaArea -= ShowVikingImage;
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
            float fillAmount = playerAction.currentHP / 40f;
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
            dialogueImage1.sprite = dialogue.dialogueEntries[currentDialogueIndex].dialogueImage1;

            StartCoroutine(TypeSentence(dialogue.dialogueEntries[currentDialogueIndex].dialogueText));
            currentDialogueIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        for (int i = 0; i < sentence.Length; i++)
        {
            dialogueText.text += sentence[i];
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }

    public void Fade()
    {
        fadeImage.DOFade(1, 5);
    }

    private void UpdateEnemyCount()
    {
        if (!goblinDead)
        {
            goblinCount--;
            goblinCountText.text = "Goblins: " + goblinCount.ToString("F0");
            if (goblinCount <= 0)
            {
                goblinDead = true;
                Goblin_ui.SetActive(false);
                playerAction.onKillAllGoblins?.Invoke();
                UpdateVikingCount();
            }
        }
        else
        {
            vikingCount--;
            vikingCountText.text = "Vikings: " + vikingCount.ToString("F0");
            if (vikingCount <= 0)
            {
                playerAction.onKillAllViking?.Invoke();
                Viking_ui.SetActive(false);              
            }
        }
    }

    private void UpdateVikingCount()
    {
        vikingCountText.text = "Vikings: " + vikingCount.ToString("F0");
    }

    private void ShowGoblinImage()
    {
        Goblin_ui.SetActive(true);
    }

    private void ShowVikingImage()
    {
        Viking_ui.SetActive(true);
    }
}
