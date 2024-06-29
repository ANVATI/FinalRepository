using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public GameObject[] armas;
    public Image[] imagenesArmas;
    private bool[] armasDesbloqueadas;
    private int indiceActual = 0;
    private PlayerController _player;
    private float timeToChange = 0;
    private bool isChangingWeapon = false;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    void Start()
    {
        armasDesbloqueadas = new bool[armas.Length];
        armasDesbloqueadas[0] = true;
        ActualizarInterfaz();
    }

    public void DesbloquearArma(int indiceArma)
    {
        if (indiceArma >= 0 && indiceArma < armas.Length)
        {
            armasDesbloqueadas[indiceArma] = true;
            StartCoroutine(DelayInterface());
        }
    }

    void ActualizarInterfaz()
    {
        for (int i = 0; i < armas.Length; i++)
        {
            if (armasDesbloqueadas[i])
            {
                if (i == indiceActual)
                {
                    imagenesArmas[i].color = new Color(imagenesArmas[i].color.r, imagenesArmas[i].color.g, imagenesArmas[i].color.b, 1f);
                    armas[i].SetActive(true);
                }
                else
                {
                    imagenesArmas[i].color = new Color(imagenesArmas[i].color.r, imagenesArmas[i].color.g, imagenesArmas[i].color.b, 0.5f);
                    armas[i].SetActive(false);
                }
            }
        }
    }

    void Update()
    {
        timeToChange += Time.deltaTime;
    }

    public void OnNextWeapon(InputAction.CallbackContext context)
    {
        if (context.performed && !isChangingWeapon)
        {
            if (_player.ChangeWeapon() && timeToChange >= 1.5f)
            StartCoroutine(DelayNextWeapon());
            timeToChange = 0;
        }
    }

    public void OnPreviousWeapon(InputAction.CallbackContext context)
    {
        if (context.performed && !isChangingWeapon)
        {
            if (_player.ChangeWeapon() && timeToChange >= 1.5f)
            {
                StartCoroutine(DelayBeforeWeapon());
                timeToChange = 0;
            }
        }
    }

    void CambiarArmaAnterior()
    {
        if (!_player.isAttacking)
        {
            int nuevoIndice = (indiceActual - 1 + armas.Length) % armas.Length;
            while (!armasDesbloqueadas[nuevoIndice] && nuevoIndice != indiceActual)
            {
                nuevoIndice = (nuevoIndice - 1 + armas.Length) % armas.Length;
            }

            if (nuevoIndice != indiceActual)
            {
                indiceActual = nuevoIndice;
                ActualizarInterfaz();
            }
        }
    }

    void CambiarArmaSiguiente()
    {
        if (!_player.isAttacking)
        {
            int nuevoIndice = (indiceActual + 1) % armas.Length;
            while (!armasDesbloqueadas[nuevoIndice] && nuevoIndice != indiceActual)
            {
                nuevoIndice = (nuevoIndice + 1) % armas.Length;
            }

            if (nuevoIndice != indiceActual)
            {
                indiceActual = nuevoIndice;

                ActualizarInterfaz();
            }
        }
    }

    IEnumerator DelayNextWeapon()
    {
        isChangingWeapon = true;
        yield return new WaitForSeconds(0.5f);
        CambiarArmaSiguiente();
        yield return new WaitForSeconds(2f);
        isChangingWeapon = false;
    }

    IEnumerator DelayBeforeWeapon()
    {
        isChangingWeapon = true;
        yield return new WaitForSeconds(0.5f);
        CambiarArmaAnterior();
        yield return new WaitForSeconds(2f);
        isChangingWeapon = false;
    }

    IEnumerator DelayInterface()
    {
        ActualizarInterfaz();
        yield return new WaitForSeconds(0.5f);
    }
}
