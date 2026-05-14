using System;
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
    private float retreatTimer;
    private int currentHealth;
    [SerializeField] private bool dieAfterDamage = false;

    void Start()
    {
        currentHealth = maxHealth;
        player = FindPlayer();
        SetNewPatrolDestination();
        patrolTimer = patrolChangeInterval;
        attackTimer = 0f;
        retreatTimer = 0f;
    }

    void Update()
    {
        if (currentHealth <= 0)
            return;

        attackTimer -= Time.deltaTime;
        retreatTimer -= Time.deltaTime;

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= chaseRange)
            {
                if (retreatTimer > 0f)
                {
                    RetreatFromPlayer();
                }
                else
                {
                    ChasePlayer(distanceToPlayer);
                }
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
        MoveInDirection(direction, player.position);

        if (distanceToPlayer <= attackRange)
        {
            TryAttack();
        }
    }

    private void RetreatFromPlayer()
    {
        Vector3 direction = (transform.position - player.position).normalized;
        MoveInDirection(direction, player.position);
    }

    private void Patrol()
    {
        patrolTimer -= Time.deltaTime;
        if (patrolTimer <= 0f || Vector3.Distance(transform.position, patrolDestination) < 0.2f)
        {
            SetNewPatrolDestination();
        }

        Vector3 direction = (patrolDestination - transform.position).normalized;
        MoveInDirection(direction, patrolDestination);
    }

    private void SetNewPatrolDestination()
    {
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * patrolRadius;
        patrolDestination = transform.position + new Vector3(randomCircle.x, -Math.Abs(randomCircle.y) / 3f, randomCircle.y);
        patrolTimer = patrolChangeInterval;
    }

    private void MoveInDirection(Vector3 direction, Vector3 targetPosition)
    {
        if (direction.sqrMagnitude < 0.01f)
            return;

        direction = (targetPosition - transform.position).normalized;
        direction.y = Mathf.Clamp(direction.y, -0.5f, 0.5f);
        direction = direction.normalized;

        Transform t = transform;
        t.rotation = Quaternion.Slerp(t.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
        t.position += direction * moveSpeed * Time.deltaTime;
    }

    private void TryAttack()
    {
        if (attackTimer > 0f)
            return;

        attackTimer = attackCooldown;
        retreatTimer = attackCooldown;
        if (player != null && player.TryGetComponent<Health>(out var damageable))
        {
            damageable.TakeDamage(attackDamage);
            if (dieAfterDamage)
            {
                TakeDamage(maxHealth); // Instantly die after attacking
            }
            Debug.Log("Enemy attacked player for " + attackDamage + " damage.");
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
