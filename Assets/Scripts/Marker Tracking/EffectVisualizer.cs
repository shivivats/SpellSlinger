using System;
using System.Collections;
using System.Runtime.InteropServices;
using GoogleARCore;
using GoogleARCoreInternal;
using UnityEngine;

/// <summary>
/// Spawns spells based on what card it has started tracking
/// </summary>
public class EffectVisualizer : MonoBehaviour
{
	/// <summary>
	/// The AugmentedImage to visualize from a scanned card
	/// </summary>
	public AugmentedImage Image;

    /// <summary>
    /// The fire spell to spawn if fire card is scanned
    /// </summary>
	public GameObject Fire;
    /// <summary>
    /// The spike spell to spawn if spike card is scanned
    /// </summary>
	public GameObject Spikes;
    /// <summary>
    /// The shield spell to spawn if shield card is scanned
    /// </summary>
	public GameObject Shield;
    /// <summary>
    /// The ball spell to spawn if ball card is scanned
    /// </summary>
	public GameObject Ball;

    /// <summary>
    /// How long until the spell can be used again
    /// </summary>
	public float spellCooldown;
    /// <summary>
    /// Whether or not we can spawn the spell
    ///  This is so we don't spawn the spell multiple times on the same card
    /// </summary>
	public bool canSpawn;

    /// <summary>
    /// The Unity Start method
    /// </summary>
    private void Start()
	{
		canSpawn = true;
	}

	/// <summary>
	/// The Unity Update method
	/// </summary>
	public void Update()
	{
        // Return until we start tracking an image
		if (Image == null || Image.TrackingState != TrackingState.Tracking)
		{
			return;
		}

        // If we can still spawn a spell, then spawn whichever spell relates to the database index we scanned
        // Then update the different paramaters to display the spell better in the world
		if (canSpawn)
		{
			Debug.Log("spell position is " + transform.position);

			if (Image.DatabaseIndex == 0)
			{
				Shield = Instantiate(Shield, transform.position, Quaternion.identity);
				Shield.SetActive(true);
				Shield.transform.localScale *= 0.05f;
				Shield.transform.parent = transform;

			}
			else if (Image.DatabaseIndex == 1)
			{
				Spikes = Instantiate(Spikes, transform.position, Quaternion.identity);
				Spikes.SetActive(true);
				Spikes.transform.parent = transform;
			}
			else if (Image.DatabaseIndex == 2)
			{
				Ball = Instantiate(Ball, transform.position, Quaternion.identity);
				Ball.SetActive(true);
			}
			else if (Image.DatabaseIndex == 3)
			{
				Fire = Instantiate(Fire, transform.position, Quaternion.identity);
				Fire.SetActive(true);
				Fire.transform.parent = transform;
			}
			else
			{
				Spikes = Instantiate(Spikes, transform.position, Quaternion.identity);
				Spikes.SetActive(true);
			}

            // After spawning the spell, toggle canSpawn to false
			canSpawn = false;
		}

	}
}
