using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 3f;
    public float chaseRange = 8f;
    public float attackRange = 1.5f;
    public float patrolRadius = 5f;
    public float patrolChangeInterval = 3f;
    public int maxHealth = 3;
    public int attackDamage = 1;
    public float attackCooldown = 1.2f;

    private Transform player;
    private Vector3 patrolDestination;
    private float patrolTimer;
    private float attackTimer;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        player = FindPlayer();
        SetNewPatrolDestination();
        patrolTimer = patrolChangeInterval;
        attackTimer = 0f;
    }

    void Update()
    {
        if (currentHealth <= 0)
            return;

        attackTimer -= Time.deltaTime;

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= chaseRange)
            {
                ChasePlayer(distanceToPlayer);
                return;
            }
        }

        Patrol();
    }

    private Transform FindPlayer()
    {
        var playerObject = GameObject.FindWithTag("Player");
        return playerObject != null ? playerObject.transform : null;
    }

    private void ChasePlayer(float distanceToPlayer)
    {
        Vector3 direction = (player.position - transform.position).normalized;
        MoveInDirection(direction);

        if (distanceToPlayer <= attackRange)
        {
            TryAttack();
        }
    }

    private void Patrol()
    {
        patrolTimer -= Time.deltaTime;
        if (patrolTimer <= 0f || Vector3.Distance(transform.position, patrolDestination) < 0.2f)
        {
            SetNewPatrolDestination();
        }

        Vector3 direction = (patrolDestination - transform.position).normalized;
        MoveInDirection(direction);
    }

    private void SetNewPatrolDestination()
    {
        Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
        patrolDestination = transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
        patrolTimer = patrolChangeInterval;
    }

    private void MoveInDirection(Vector3 direction)
    {
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.01f)
            return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void TryAttack()
    {
        if (attackTimer > 0f)
            return;

        attackTimer = attackCooldown;
        var damageable = player.GetComponent<Health>();
        if (damageable != null)
        {
            damageable.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
            return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
