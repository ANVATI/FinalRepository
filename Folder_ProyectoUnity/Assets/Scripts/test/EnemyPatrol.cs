using UnityEngine;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    public List<Node3D> patrolPath = new List<Node3D>();
    public float speed = 3.0f;
    public float waitTime = 2.0f;
    public Transform player; 

    private int currentNodeIndex = 0;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private bool isFollowingPlayer = false;

    void Start()
    {
        if (patrolPath.Count > 0)
        {
            transform.position = patrolPath[0].transform.position;
        }
    }

    void Update()
    {
        if (isFollowingPlayer)
        {
            FollowPlayer();
        }
        else
        {
            if (!isWaiting)
            {
                MoveTowardsNode();
            }
        }
    }

    void MoveTowardsNode()
    {
        if (patrolPath.Count == 0) return;

        Node3D targetNode = patrolPath[currentNodeIndex];
        Vector3 direction = (targetNode.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetNode.transform.position) < 0.1f)
        {
            isWaiting = true;
            Invoke(nameof(SetNextNode), waitTime);
        }
    }

    void SetNextNode()
    {
        currentNodeIndex = (currentNodeIndex + 1) % patrolPath.Count;
        isWaiting = false;
    }

    public void StartChasingPlayer()
    {
        isFollowingPlayer = true;
    }

    void FollowPlayer()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            transform.position += directionToPlayer * speed * Time.deltaTime;

            transform.LookAt(player.position);
        }
    }
}
