using UnityEngine;
public class Node<T>
{
    public T Dato { get; set; }
    public Node<T> Siguiente { get; set; }
    public Node<T> Anterior { get; set; }

    public Node(T dato)
    {
        Dato = dato;
        Siguiente = null;
        Anterior = null;
    }
}
