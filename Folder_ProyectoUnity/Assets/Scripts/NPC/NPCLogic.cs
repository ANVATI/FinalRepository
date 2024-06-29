using System.Collections;
using UnityEngine;

public class NPCLogic : MonoBehaviour
{
    Animator _animator;
    bool inArea;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inArea = true;
            if (inArea)
            {
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