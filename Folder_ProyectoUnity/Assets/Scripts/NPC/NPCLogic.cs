using System.Collections;
using UnityEngine;

public class NPCLogic : MonoBehaviour
{
    Animator _animator;
    bool inArea;
    public GameObject Dialog;
    private void Start()
    {
        Dialog.SetActive(true);
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inArea = true;
            if (inArea)
            {
                Dialog.SetActive(false);
                _animator.SetBool("Talk", true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _animator.SetBool("Talk", false);
            inArea = false;
        }
    }
}