using UnityEngine;
using DG.Tweening;

public class Doors : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject targetObject1;
    public GameObject DoorExit;
    public GameObject DoorExit1;
    public PlayerActions playerAction;
    private void OnEnable()
    {
        playerAction.onKillAllGoblins += OcultDoorBosque;
        playerAction.onKillAllViking += AppearDoorExitAldea;
        playerAction.OnAldeaArea += AppearDoorAldea;
    }
    private void OnDisable()
    {
        playerAction.onKillAllGoblins -= AppearDoorAldea;
        playerAction.OnAldeaArea -= OcultDoorBosque;
        playerAction.onKillAllViking -= AppearDoorExitAldea;
    }
    public void OcultDoorBosque()
    {
        targetObject.SetActive(false);
        targetObject1.SetActive(false);
    }

    public void AppearDoorAldea()
    {
        targetObject.SetActive(true);
        targetObject1.SetActive(true);
    }

    public void OcultDoorAldea()
    {

    }
    public void AppearDoorExitAldea()
    {
        DoorExit.SetActive(false);
        DoorExit1.SetActive(false);
    }
}

