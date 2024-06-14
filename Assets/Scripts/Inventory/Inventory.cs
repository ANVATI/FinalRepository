using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject[] armas;
    public Image[] imagenesArmas;
    private bool[] armasDesbloqueadas;
    private int indiceActual = 0;
    private PlayerController _player;

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
            ActualizarInterfaz();
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
                    imagenesArmas[i].color = new Color(0, 0,0, 1f);
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
        if (Input.GetKeyDown(KeyCode.Q) && _player.ChangeWeapon())
        {
            CambiarArmaAnterior();
        }
        else if (Input.GetKeyDown(KeyCode.E) && _player.ChangeWeapon())
        {
            CambiarArmaSiguiente();
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
}
