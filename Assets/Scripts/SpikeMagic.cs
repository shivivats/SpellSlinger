using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spike spell that kills enemies in an area
/// </summary>
public class SpikeMagic : MonoBehaviour
{
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
    /// The Unity Start method
    /// </summary>
    void Start()
	{
		timer = duration;

		transform.localScale *= 1f;

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
}
