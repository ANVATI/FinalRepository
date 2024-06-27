using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Graph graph;
    public GameObject[] patrolRouteArray;
    private SimpleList<GameObject> patrolRoute = new SimpleList<GameObject>();
    public float speed = 5f;

    private int currentPatrolIndex = 0;
    private GameObject currentTargetNode;

    public float detectionRadius = 10f;
    private GameObject player; 

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true; 

        InitializePatrolRoute();
        InitializeEnemy();
    }

    void Update()
    {
        if (player != null && IsPlayerInDetectionRange())
        {
            if (navMeshAgent.isActiveAndEnabled && navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.SetDestination(player.transform.position);
            }
            else
            {
                Debug.LogWarning("NavMeshAgent is not active or not on NavMesh.");
            }
        }
        else if (currentTargetNode != null)
        {
            MoveTowardsObjective(currentTargetNode.transform.position);

            if (Vector3.Distance(transform.position, currentTargetNode.transform.position) < 0.1f)
            {
                UpdatePatrolRoute();
            }
        }
    }

    bool IsPlayerInDetectionRange()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer <= detectionRadius;
        }
        return false;
    }

    void InitializePatrolRoute()
    {
        patrolRoute.Clear();
        for (int i = 0; i < patrolRouteArray.Length; i++)
        {
            patrolRoute.Add(patrolRouteArray[i]);
        }
    }

    void InitializeEnemy()
    {
        if (patrolRoute.Length > 0)
        {
            currentTargetNode = patrolRoute.Get(currentPatrolIndex);
        }
    }

    void MoveTowardsObjective(Vector3 objective)
    {
        transform.position = Vector3.MoveTowards(transform.position, objective, speed * Time.deltaTime);
    }

    void UpdatePatrolRoute()
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolRoute.Length;
        currentTargetNode = patrolRoute.Get(currentPatrolIndex);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }
}
