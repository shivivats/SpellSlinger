using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HutManager : MonoBehaviour
{
    /// <summary>
    /// The maximum hits the hut can take before it is destroyed
    /// </summary>
	public int maxNumberOfHits;

    /// <summary>
    /// Current number of hits the hut has sustained
    /// </summary>
	private int currentNumberOfHits;

    /// <summary>
    /// The current level of damage the hut has taken
    /// </summary>
	private int hutDamageLevel;

    /// <summary>
    /// Contains all the meshes for Full Life, Half Destroyed, and Destroyed hut models
    /// </summary>
	public Mesh[] hutMeshes;

    /// <summary>
    /// Contains all the materials for Full Life, Half Destroyed, and Destroyed hut models
    /// </summary>
	public Material[] hutMaterials;

    /// <summary>
    /// Mesh Filter component attached to this object
    /// </summary>
	private MeshFilter hutMeshFilterComponent;

    /// <summary>
    /// Mesh Renderer component attached to this object
    /// </summary>
	private MeshRenderer hutMeshRendererComponent;

    /// <summary>
    /// The hut HP UI Image
    /// </summary>
	public Image hutHPImage;

    /// <summary>
    /// The Unity Start method
    /// </summary>
	private void Start()
	{
		currentNumberOfHits = 0;
		hutDamageLevel = 0;

		hutMeshFilterComponent = GetComponent<MeshFilter>();
		hutMeshRendererComponent = GetComponent<MeshRenderer>();

		hutHPImage = GameObject.FindGameObjectWithTag("HutHPImg").GetComponent<Image>();

		UpdateHut();
	}

    /// <summary>
    /// Take damage and update/destroy the hut on each hit
    /// </summary>
	public void TakeHit()
	{
		currentNumberOfHits++;
        hutDamageLevel = currentNumberOfHits / 50000;

        if (currentNumberOfHits >= maxNumberOfHits)
		{
			UpdateHut();
			HutDestroyed();
		}
		else
		{
			UpdateHut();
		}
	}

    /// <summary>
    /// Gets called when the hut is destroyed
    /// Sets the highscore if current wave is higher and loads the end scene from the LevelManager script
    /// </summary>
	public void HutDestroyed()
	{
        if (!PlayerPrefs.HasKey("Highscore") || GameObject.FindObjectOfType<EnemyManager>().GetCurrentWave() > PlayerPrefs.GetInt("Highscore"))
            PlayerPrefs.SetInt("Highscore", GameObject.FindObjectOfType<EnemyManager>().GetCurrentWave());
		GameObject.FindObjectOfType<LevelManager>().GameEnd();
	}

    /// <summary>
    /// Updates the UI image for hut's HP level
    /// </summary>
	private void UpdateHut()
	{
		hutHPImage.fillAmount = ((float)maxNumberOfHits - (float)currentNumberOfHits) / (float)maxNumberOfHits;
		Debug.Log("Hut at " + (((float)maxNumberOfHits - (float)currentNumberOfHits) / (float)maxNumberOfHits) * 100f + "% hp.");
	}

    /// <summary>
    /// When an enemy collides with the hut, take damage and call EnemyReachedGoal() on that enemy
    /// </summary>
    /// <param name="collision">The enemy that collided</param>
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			TakeHit();
			collision.gameObject.GetComponent<EnemyMovement>().EnemyReachedGoal();
		}
	}

    /// <summary>
    /// When an enemy collides with the hut trigger, take damage and call EnemyReachedGoal() on that enemy
    /// </summary>
    /// <param name="other">The enemy that collided with the trigger</param>
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			TakeHit();
			other.gameObject.GetComponent<EnemyMovement>().EnemyReachedGoal();
		}
	}
}
