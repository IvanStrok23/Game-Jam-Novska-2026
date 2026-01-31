using UnityEngine;
using UnityEngine.AI;

public class ObstacleBase : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] walkPoints;
    public DirtType DirtType;
    public float Damage = 0;
    public float LifetimeAfterHit = 3;

    private int currentPointIndex = 0;
    public bool IsHit = false;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        GoToNextPoint();
    }

    void Update()
    {
        if (IsHit) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            GoToNextPoint();
        }
    }

    void GoToNextPoint()
    {
        if (walkPoints.Length == 0) return;

        agent.destination = walkPoints[currentPointIndex].position;
        currentPointIndex = (currentPointIndex + 1) % walkPoints.Length;
    }

    public void Hit(Vector3 force)
    {
        if (IsHit) return;
        IsHit = true;

        agent.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(force, ForceMode.Impulse);
        }
        Invoke(nameof(DestroyObject), LifetimeAfterHit);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

}

