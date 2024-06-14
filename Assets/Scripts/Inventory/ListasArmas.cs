using UnityEngine;
using UnityEngine.UI;
public class ListaArmas
{
    private NodoArma cabeza;
    private int longitud;
    private int capacidad;

    public ListaArmas(int capacidad)
    {
        this.capacidad = capacidad;
        cabeza = null;
        longitud = 0;
    }

    public NodoArma Cabeza
    {
        get { return cabeza; }
    }

    public int Longitud
    {
        get { return longitud; }
    }

    public bool EstaLlena()
    {
        return longitud == capacidad;
    }

    public void Agregar(GameObject arma, Image imagenArma)
    {
        if (EstaLlena())
        {
            Debug.Log("La lista de armas está llena");
            return;
        }

        NodoArma nuevoNodo = new NodoArma(arma, imagenArma);
        if (cabeza == null)
        {
            cabeza = nuevoNodo;
            nuevoNodo.Siguiente = nuevoNodo;
        }
        else
        {
            NodoArma ultimo = cabeza;
            while (ultimo.Siguiente != cabeza)
            {
                ultimo = ultimo.Siguiente;
            }
            ultimo.Siguiente = nuevoNodo;
            nuevoNodo.Siguiente = cabeza;
        }
        longitud++;
    }

    public void NavegarAnterior()
    {
        if (cabeza != null)
        {
            cabeza = cabeza.Siguiente;
        }
    }

    public void NavegarSiguiente()
    {
        if (cabeza != null)
        {
            NodoArma ultimo = cabeza;
            while (ultimo.Siguiente != cabeza)
            {
                ultimo = ultimo.Siguiente;
            }
            cabeza = ultimo;
        }
    }
}