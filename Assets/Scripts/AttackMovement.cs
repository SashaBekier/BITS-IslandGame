using UnityEngine;

public class AttackMovement : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 enemyPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (isMoving)
        {
            // Move the character sprite towards the target position
            float speed = 5f; // Adjust the movement speed as desired
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            // Check if the character has reached the target position
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                // Check if the target position is the enemy position
                if (targetPosition == enemyPosition)
                {
                    // Set the target position back to the original position
                    targetPosition = originalPosition;
                }
                else
                {
                    // Reset the target position to move back to the enemy position
                    targetPosition = enemyPosition;
                }

                isMoving = false;
            }
        }
    }

    public void MoveToEnemy()
    {
        // Find the enemy sprite GameObject or get a reference to it in some way
        GameObject enemySprite = GameObject.Find("Enemy");

        if (enemySprite != null)
        {
            enemyPosition = enemySprite.transform.position;

            // Set the target position to the enemy position
            targetPosition = enemyPosition;
            isMoving = true;
        }
        else
        {
            Debug.LogError("Enemy sprite not found or reference not set!");
        }
    }
}