using UnityEngine;
using System.Collections.Generic;

public class Node3D : MonoBehaviour
{
    public List<Connection> connections = new List<Connection>();

    public void AddConnection(Node3D targetNode, float weight)
    {
        Connection connection = new Connection(targetNode, weight);
        connections.Add(connection);
    }

    public Connection GetRandomConnection()
    {
        if (connections.Count == 0)
            return null;

        int randomIndex = Random.Range(0, connections.Count);
        return connections[randomIndex];
    }
}

[System.Serializable]
public class Connection
{
    public Node3D targetNode;

    public Connection(Node3D targetNode, float weight)
    {
        this.targetNode = targetNode;
    }
}
