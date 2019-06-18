using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSpell : MonoBehaviour
{
	public float duration;
	public float shutDownTime;
	private float timer;
	private ParticleSystem ps;

	// Start is called before the first frame update
	void Start()
	{
		timer = duration;

		transform.localScale *= 0.3f;

		ps = gameObject.GetComponent<ParticleSystem>();
		ps.Play();
	}

	// Update is called once per frame
	void Update()
	{
		timer -= Time.deltaTime;

		if (timer <= 0)
		{
			ps.Stop();
			StartCoroutine(destroySpell(shutDownTime));
		}

	}

	IEnumerator destroySpell(float sdTime)
	{
		yield return new WaitForSeconds(sdTime);
		GameObject.FindObjectOfType<MarkerController>().m_Visualizers.Remove(gameObject.transform.parent.gameObject.GetComponent<EffectVisualizer>().Image.DatabaseIndex);
		Destroy(gameObject.transform.parent.gameObject);
	}
}
