using UnityEngine;
using System.Collections;
public class ColisionDetectada : MonoBehaviour
{
    public int indiceArmaADesbloquear;
    public GameObject BG;
    public AudioSource _audio;
    public AudioClip cogerArma;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory != null)
            {
                inventory.DesbloquearArma(indiceArmaADesbloquear);
                StartCoroutine(UnlockWeapon());
            }
        }
    }
    IEnumerator UnlockWeapon()
    {
        _audio.PlayOneShot(cogerArma);
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
        Destroy(BG);
    }
}
