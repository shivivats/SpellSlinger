using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMagic : MonoBehaviour
{
    /// <summary>
    /// Stone speed
    /// </summary>
	public float speed;
    /// <summary>
    /// Direction of the stone
    /// </summary>
	private Vector3 direction;
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
    /// Rigidbody attached to this object
    /// </summary>
	private Rigidbody rb;
    /// <summary>
    /// Particle system attached to this object
    /// </summary>
	private ParticleSystem ps;

    /// <summary>
    /// The Unity Start method
    /// </summary>
    void Start()
	{
		timer = duration;

		ps = GetComponent<ParticleSystem>();
		ps.Play();

		rb = gameObject.GetComponent<Rigidbody>();

		direction = (FindObjectOfType<LevelManager>().madeHut.transform.position - transform.position).normalized;

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

	}

    /// <summary>
    /// The Unity FixedUpdate method
    /// </summary>
	private void FixedUpdate()
	{
		if (timer > 0)
		{
			rb.velocity = direction * speed;
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
		if (gameObject != null)
			Destroy(gameObject);
	}
}
