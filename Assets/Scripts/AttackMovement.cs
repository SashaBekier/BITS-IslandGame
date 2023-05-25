using UnityEngine;

public class AttackMovement : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 enemyPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool hasReachedEnemy = false;
    private bool isAttacking = false;

    private PlayerStats playerStats;
    private EnemyHealth enemyHealth;

    private void Start()
    {
        originalPosition = transform.position;

        // Find the enemy sprite GameObject or get a reference to it in some way
        GameObject enemySprite = GameObject.Find("Enemy");

        if (enemySprite != null)
        {
            enemyPosition = enemySprite.transform.position;
            enemyHealth = enemySprite.GetComponent<EnemyHealth>();
        }
        else
        {
            Debug.LogError("Enemy sprite not found or reference not set!");
        }

        // Get the PlayerStats component from the character
        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (isMoving)
        {
            // Move the character sprite towards the target position
            float speed = 10f; // Adjust the movement speed as desired
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            // Check if the character has reached the target position
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                if (targetPosition == enemyPosition)
                {
                    hasReachedEnemy = true;
                    targetPosition = originalPosition;
                }
                else if (targetPosition == originalPosition && hasReachedEnemy)
                {
                    isMoving = false;
                    hasReachedEnemy = false;
                    Attack();
                }
            }
        }
        else if (isAttacking)
        {
            // Check if the character is not moving and its current position is not the original position or the enemy position
            if (transform.position != originalPosition)
            {
                // Move the character sprite back to the original position
                float speed = 10f; // Adjust the movement speed as desired
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, step);
            }
            else
            {
                isAttacking = false;
            }
        }
    }

    public void MoveToEnemy()
    {
        if (!isMoving && !hasReachedEnemy)
        {
            // Set the target position to the enemy position
            targetPosition = enemyPosition;
            isMoving = true;
        }
    }

    private void Attack()
    {
        if (playerStats != null && enemyHealth != null)
        {
            // Calculate the damage based on player's stats
            int damage = playerStats.StrengthTotal;

            // Apply the damage to the enemy's health
            enemyHealth.TakeDamage(damage);
        }
    }
}