using UnityEngine;

public class ColisionDetectada : MonoBehaviour
{
    public int indiceArmaADesbloquear;
    public GameObject BG;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory != null)
            {
                inventory.DesbloquearArma(indiceArmaADesbloquear);
                gameObject.SetActive(false);
                Destroy(BG);
            }
        }
    }
}
