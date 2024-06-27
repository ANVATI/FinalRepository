using UnityEngine;
public class GraphNode
{
    public GameObject NodeObject { get; private set; }
    private SimpleList<GraphNode> neighbors;

    public GraphNode(GameObject nodeObject)
    {
        NodeObject = nodeObject;
        neighbors = new SimpleList<GraphNode>();
    }

    public void AddNeighbor(GraphNode neighbor)
    {
        if (!neighbors.Contains(neighbor))
        {
            neighbors.Add(neighbor);
        }
    }

    public void RemoveNeighbor(GraphNode neighbor)
    {
        neighbors.Remove(neighbor);
    }

    public SimpleList<GraphNode> GetNeighbors()
    {
        return neighbors;
    }
}
