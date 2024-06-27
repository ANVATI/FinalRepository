using UnityEngine;

public class Graph
{
    private SimpleList<GraphNode> nodes = new SimpleList<GraphNode>();

    public void AddNode(GameObject nodeObject)
    {
        if (!Contains(nodeObject))
        {
            nodes.Add(new GraphNode(nodeObject));
        }
    }

    public void AddDirectedEdge(GameObject nodeA, GameObject nodeB)
    {
        GraphNode graphNodeA = FindNode(nodeA);
        GraphNode graphNodeB = FindNode(nodeB);

        if (graphNodeA != null && graphNodeB != null)
        {
            graphNodeA.AddNeighbor(graphNodeB);
        }
    }

    public SimpleList<GameObject> GetNeighbors(GameObject nodeObject)
    {
        GraphNode node = FindNode(nodeObject);
        if (node != null)
        {
            SimpleList<GameObject> neighbors = new SimpleList<GameObject>();
            SimpleList<GraphNode> neighborNodes = node.GetNeighbors();
            for (int i = 0; i < neighborNodes.Length; i++)
            {
                neighbors.Add(neighborNodes.Get(i).NodeObject);
            }
            return neighbors;
        }
        return null;
    }

    public bool Contains(GameObject nodeObject)
    {
        return FindNode(nodeObject) != null;
    }

    private GraphNode FindNode(GameObject nodeObject)
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes.Get(i).NodeObject == nodeObject)
            {
                return nodes.Get(i);
            }
        }
        return null;
    }
}
