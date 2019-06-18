using GoogleARCore.Examples.ObjectManipulationInternal;
using UnityEngine;
using System.Collections;

/// <summary>
/// Manipulates the position of an object via a drag gesture.
/// If not selected, the object will be selected when the drag gesture starts.
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    /// <summary>
    /// The target hut GameObject that the enemy moves towards
    /// </summary>
	private GameObject targetHut;
    /// <summary>
    /// The speed at which the enemy moves towards the targetHut
    /// </summary>
	public float moveSpeed;

    /// <summary>
    /// The Rigidbody component attached to this object
    /// </summary>
	private Rigidbody rb;

    /// <summary>
    /// How long the enemy should stay alive after attacking the hut before being destroyed
    /// </summary>
	public float lifeAfterReachingHut;

    /// <summary>
    /// Current HP of the enemy
    /// </summary>
	public float hp;
    /// <summary>
    /// Minimum HP of the enemy
    /// </summary>
	public float minHp;
    /// <summary>
    /// Maximum HP of the enemy
    /// </summary>
	public float maxHp;

    /// <summary>
    /// The enemy manager script that manages all enemies in the scene
    /// </summary>
	private EnemyManager enemyManager;


    /// <summary>
    /// The Unity Start method
    /// </summary>
    void Start()
	{
        // Store the original position
		Vector3 oldPositon = transform.position;

		rb = gameObject.GetComponent<Rigidbody>();

        // Move towards the hut from original position
		rb.velocity = (targetHut.transform.position - oldPositon).normalized * moveSpeed;

		enemyManager = FindObjectOfType<EnemyManager>();
	}

    /// <summary>
    /// If first is close to second (less than one unit), return true. Else return false
    /// </summary>
    /// <param name="first">The first of two Vector3 variables to check for proximity</param>
    /// <param name="second">The second of two Vector3 variables to check for proximity</param>
    /// <returns></returns>
	private bool VectorNear(Vector3 first, Vector3 second)
	{
		if ((first - second).magnitude < 1)
			return true;

		return false;
	}

    /// <summary>
    /// Called when the enemy hits the hut
    /// Animates the enemy to start punching and then sets a countdown to destroy the enemy
    /// </summary>
	public void EnemyReachedGoal()
	{

		gameObject.GetComponent<EnemyAnimation>().ReachedGoal();

		rb.velocity = Vector3.zero;

		StartCoroutine(DestroyCountdown());
	}

    /// <summary>
    /// Destroys the object after some time
    /// Calls KillThisEnemy()
    /// </summary>
    /// <returns></returns>
	public IEnumerator DestroyCountdown()
	{
		float time = 0f;
		while (time <= lifeAfterReachingHut)
		{
			time += Time.deltaTime;
			yield return null;

		}

		KillThisEnemy();


	}

    /// <summary>
    /// Destroys the enemy from the enemyManager script
    /// </summary>
	public void KillThisEnemy() {
		enemyManager.DestroyEnemy(gameObject);
	}

    /// <summary>
    /// Sets the hut target for the enemy to move towards
    /// </summary>
    /// <param name="newHut">The hut GameObject to target</param>
	public void SetTargetHut(GameObject newHut) {
		targetHut = newHut;
	}
}