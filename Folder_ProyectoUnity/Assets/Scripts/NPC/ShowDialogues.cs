using UnityEngine;
public class ShowDialogue : MonoBehaviour
{
    public UIManager uiManager;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            uiManager.EndDialogue();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            uiManager.StartDialogue();
        }
    }
}
