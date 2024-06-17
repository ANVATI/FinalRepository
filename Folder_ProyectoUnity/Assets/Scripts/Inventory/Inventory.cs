using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Inventory : MonoBehaviour
{
    public GameObject[] armas;
    public Image[] imagenesArmas;
    private bool[] armasDesbloqueadas;
    private int indiceActual = 0;
    private PlayerController _player;
    private float timeToChange = 0;
    private bool isChangingWeapon = false; // Añadido para evitar múltiples cambios simultáneos

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
                    imagenesArmas[i].color = new Color(0, 0, 0, 1f);
                    armas[i].SetActive(true);
                }
                else
                {
                    imagenesArmas[i].color = new Color(0, 0, 0, 0.5f);
                    armas[i].SetActive(false);
                }
            }
            else
            {
                imagenesArmas[i].color = new Color(1f, 1f, 1f, 0.5f);
                armas[i].SetActive(false);
            }
        }
    }

    void Update()
    {
        timeToChange += Time.deltaTime;

        if (!isChangingWeapon && timeToChange >= 1.5f)
        {
            if (Input.GetKeyDown(KeyCode.Q) && _player.ChangeWeapon())
            {
                StartCoroutine(DelayNextWeapon());
                timeToChange = 0;
            }
            else if (Input.GetKeyDown(KeyCode.E) && _player.ChangeWeapon())
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
            int nuevoIndice = indiceActual;
            do
            {
                nuevoIndice = (nuevoIndice - 1 + armas.Length) % armas.Length;
            } while (!armasDesbloqueadas[nuevoIndice] && nuevoIndice != indiceActual);

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
            int nuevoIndice = indiceActual;
            do
            {
                nuevoIndice = (nuevoIndice + 1) % armas.Length;
            } while (!armasDesbloqueadas[nuevoIndice] && nuevoIndice != indiceActual);

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
        isChangingWeapon = false;
    }

    IEnumerator DelayBeforeWeapon()
    {
        isChangingWeapon = true;
        yield return new WaitForSeconds(0.5f);
        CambiarArmaAnterior();
        isChangingWeapon = false;
    }

    IEnumerator DelayInterface()
    {
        ActualizarInterfaz();
        yield return new WaitForSeconds(0.5f);
    }
}