using UnityEngine;
public class ListaCircularDoble<T>
{
    private Node<T> cabeza;
    private int longitud;
    private int capacidad;

    public ListaCircularDoble(int capacidad)
    {
        this.capacidad = capacidad;
        cabeza = null;
        longitud = 0;
    }

    public Node<T> Cabeza
    {
        get { return cabeza; }
    }

    public bool EstaLlena()
    {
        return longitud == capacidad;
    }

    public void Agregar(T dato)
    {
        if (EstaLlena())
        {
            Debug.Log("El inventario está lleno");
            return;
        }

        Node<T> nuevoNodo = new Node<T>(dato);
        if (cabeza == null)
        {
            cabeza = nuevoNodo;
            nuevoNodo.Siguiente = nuevoNodo;
            nuevoNodo.Anterior = nuevoNodo;
        }
        else
        {
            Node<T> ultimo = cabeza.Anterior;
            ultimo.Siguiente = nuevoNodo;
            nuevoNodo.Anterior = ultimo;
            nuevoNodo.Siguiente = cabeza;
            cabeza.Anterior = nuevoNodo;
        }
        longitud++;
    }
}
