using UnityEngine;
public class ShowDialogue : MonoBehaviour
{
    public UIManager uiManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag  == "Player")
        {
            uiManager.StartDialogue();
        }
    }
}
