using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float wanderSpeed = 0f;
    public float chaseSpeed = 0f;
    public float chaseDistance = 0f;
    public float wanderRadius = 0f;
    public float changeDirectionInterval = 2f;

    private Transform player;
    private Vector3 wanderTarget;
    private ConstantForce Force;
    private float changeDirectionTimer;

    void Start()
    {
        player = Camera.main.transform;
        Force = GetComponent<ConstantForce>();
        SetNewWanderTarget();
        changeDirectionTimer = changeDirectionInterval;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseDistance)
        {
            ChasePlayer();
        }
        else
        {
            Wander();
        }
    }

    void Wander()
    {
        changeDirectionTimer -= Time.deltaTime;

        if (changeDirectionTimer <= 0f)
        {
            SetNewWanderTarget();
            changeDirectionTimer = changeDirectionInterval;
        }

        Vector3 direction = (wanderTarget - transform.position).normalized;
        Force.force = direction * wanderSpeed;
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Force.force = direction * chaseSpeed;

        Vector3 velocity = Force.force;
        if (velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    void SetNewWanderTarget()
    {
        wanderTarget = transform.position + Random.insideUnitSphere * wanderRadius;
    }
}
