using UnityEngine;
using UnityEngine.UI;

public class NodoArma
{
    public GameObject Arma { get; private set; }
    public Image ImagenArma { get; private set; }
    public NodoArma Siguiente { get; set; }

    public NodoArma(GameObject arma, Image imagenArma)
    {
        Arma = arma;
        ImagenArma = imagenArma;
        Siguiente = null;
    }
}