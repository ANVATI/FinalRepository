using UnityEngine;
public class ShowDialogue : MonoBehaviour
{
    public UIManager uiManager;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            uiManager.EndDialogue();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            uiManager.StartDialogue();
        }
    }
}
