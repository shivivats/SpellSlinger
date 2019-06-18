using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that kills enemies when they collide with this object
/// </summary>
public class KillScript : MonoBehaviour
{

    /// <summary>
    /// Kills the enemy when it enters the trigger
    /// </summary>
    /// <param name="other">The enemy</param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
			Debug.Log("enemy collided with spell");
            other.gameObject.GetComponent<EnemyMovement>().KillThisEnemy();
        }
    }

    /// <summary>
    /// Kills the enemy when it collides with this object
    /// </summary>
    /// <param name="collision">The colliding enemy</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyMovement>().KillThisEnemy();
        }
    }
}
