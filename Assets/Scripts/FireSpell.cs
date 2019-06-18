using System.Collections;
using UnityEngine;

/// <summary>
/// Fire tornado spell that kills enemies it touches
/// </summary>
public class FireSpell : MonoBehaviour
{
    /// <summary>
    /// Stone speed
    /// </summary>
	public float speed;
    /// <summary>
    /// Duration of the spell
    /// </summary>
	public float duration;
    /// <summary>
    /// How long the spell should take to be destroyed
    /// We do this to wait for the particles to end
    /// </summary>
	public float shutDownTime;
    /// <summary>
    /// Timer for how long the spell will last before it is destroyed
    /// </summary>
	private float timer;
    /// <summary>
    /// Particles to spawn
    /// </summary>
	public GameObject particlesObject;
    /// <summary>
    /// Current particles spawned
    /// </summary>
	private GameObject currParticlesObject;
    /// <summary>
    /// Particle system attached to this object
    /// </summary>
	private ParticleSystem ps;
    /// <summary>
    /// Closest enemy to the tornado that the tornado should move towards
    /// </summary>
	private GameObject nearestEnemy;

    /// <summary>
    /// The Unity Start method
    /// </summary>
    void Start()
	{
		timer = duration;

		transform.localScale *= 0.05f;

		ps = GetComponent<ParticleSystem>();
		ps.Play();


	}

    /// <summary>
    /// The Unity Update method
    /// </summary>
    void Update()
	{
		timer -= Time.deltaTime;

		if (timer <= 0)
		{
			ps.Stop();
			StartCoroutine(destroySpell(shutDownTime));
		}
		else
		{
			SearchForNearestEnemy();
			if(nearestEnemy!=null)
				transform.position = Vector3.MoveTowards(transform.position, nearestEnemy.transform.position, 0.001f);
		}

	}
    /// <summary>
    /// Destroys a spell object after a fixed amount of time
    /// </summary>
    /// <param name="sdTime">The time it takes to destroy the object</param>
    /// <returns></returns>
	IEnumerator destroySpell(float sdTime)
	{
		yield return new WaitForSeconds(sdTime);
		GameObject.FindObjectOfType<MarkerController>().m_Visualizers.Remove(gameObject.transform.parent.gameObject.GetComponent<EffectVisualizer>().Image.DatabaseIndex);
		Destroy(gameObject.transform.parent.gameObject);
	}

    /// <summary>
    /// Finds the closest enemy in order to start moving towards it
    /// </summary>
	void SearchForNearestEnemy()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

		if (enemies.Length == 0)
		{
			nearestEnemy = null;
		}
		else
		{
			float distanceToNearestEnemy = 100f;
			foreach (GameObject enemy in enemies)
			{
				if ((transform.position - enemy.transform.position).magnitude <= distanceToNearestEnemy)
				{
					nearestEnemy = enemy;
					distanceToNearestEnemy = (transform.position - enemy.transform.position).magnitude;
				}
			}
		}
	}
}
