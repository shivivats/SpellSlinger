using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCoreInternal;

/// <summary>
/// A custom controller for marker recognition and spawning spells. Based on the AugmentedImageExampleController given by Google
/// </summary>
public class MarkerController : MonoBehaviour
{
	/// <summary>
	/// A prefab for visualizing an AugmentedImage.
	/// </summary>
	public EffectVisualizer AugmentedImageVisualizerPrefab;

	/// <summary>
	/// The overlay containing the fit to scan user guide.
	/// </summary>
	public GameObject FitToScanOverlay;

	/// <summary>
	/// A dictionary with every currently active visualizer
	/// </summary>
	public Dictionary<int, EffectVisualizer> m_Visualizers
		= new Dictionary<int, EffectVisualizer>();

	/// <summary>
	/// Updated augmented images in the current frame
	/// </summary>
	private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

	/// <summary>
	/// The LevelManager of the scene
	/// </summary>
	private LevelManager levelManager;

	/// <summary>
	/// Cooldown for the shield spell
	/// </summary>
	public float shieldCooldown;

	/// <summary>
	/// Can she shield spell be used?
	/// </summary>
	public bool canUseShield;

	/// <summary>
	/// Cooldown for the fire spell
	/// </summary>
	public float fireCooldown;

	/// <summary>
	/// Can the fire spell be used?
	/// </summary>
	public bool canUseFire;

	/// <summary>
	/// Cooldown for the spike spell
	/// </summary>
	public float spikeCooldown;

	/// <summary>
	/// Can the spike spell be used?
	/// </summary>
	public bool canUseSpikes;

	/// <summary>
	/// Cooldown for the ball spell
	/// </summary>
	public float ballCooldown;

	/// <summary>
	/// Can the ball spell be used?
	/// </summary>
	public bool canUseBall;

	/// <summary>
	/// UI image for the shield spell
	/// </summary>
	public Image shieldCooldownImage;

	/// <summary>
	/// UI image for the fire spell
	/// </summary>
	public Image fireCooldownImage;

	/// <summary>
	/// UI image for the spike spell
	/// </summary>
	public Image spikeCooldownImage;

	/// <summary>
	/// UI image for the ball spell
	/// </summary>
	public Image ballCooldownImage;

	/// <summary>
	/// The Unity Start method
	/// </summary>
	private void Start()
	{
		levelManager = GameObject.FindObjectOfType<LevelManager>();

		canUseShield = true;
		canUseBall = true;
		canUseFire = true;
		canUseSpikes = true;
	}

	/// <summary>
	/// The Unity Update method.
	/// </summary>
	public void Update()
	{
		// Exit the app when the 'back' button is pressed.
		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}

		// The game plane is only set when the game has started
		if (levelManager.gamePlaneSet)
		{

			// Get updated augmented images for this frame.
			Session.GetTrackables<AugmentedImage>(
			m_TempAugmentedImages, TrackableQueryFilter.Updated);


			// Create visualizers and anchors for updated augmented images that are tracking and do
			// not previously have a visualizer. Remove visualizers for stopped images.
			foreach (var image in m_TempAugmentedImages)
			{

				EffectVisualizer visualizer = null;
				m_Visualizers.TryGetValue(image.DatabaseIndex, out visualizer);

				// Check for shield image and shield cooldown
				if (image.DatabaseIndex == 0 && canUseShield)
				{
					// similar if statement for every spell. This checks if the spell marker is currently in the view and there isnt a spell already being run from the marker.
					if (image.TrackingState == TrackingState.Tracking && visualizer == null && image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking)
					{
						// Get the game detected plane
						DetectedPlane gameDetectedPlane = levelManager.GetGamePlane() as DetectedPlane;

						// Create an anchor to ensure that ARCore keeps tracking this augmented image.
						var anchor = gameDetectedPlane.CreateAnchor(gameDetectedPlane.CenterPose);

						// Set the effect position to the hut position for the shield spell
						Vector3 effectPosition = levelManager.madeHut.transform.position;

						// Make the visualizer and anchor it to the hut
						visualizer = (EffectVisualizer)Instantiate(AugmentedImageVisualizerPrefab, effectPosition, levelManager.madeHut.transform.rotation);

						visualizer.gameObject.transform.parent = anchor.transform;

						Debug.Log("effect position is: " + effectPosition);

						Debug.Log("image pose is: " + image.CenterPose);

						visualizer.Image = image;

						m_Visualizers.Add(image.DatabaseIndex, visualizer);

						// We can not use shields anymore till the cooldown is up
						canUseShield = false;

						// Start shield cooldown
						StartCoroutine(ShieldCooldownTimer());
					}
					// if  the image isn't being tracked and there is still a visualizer, destroy the visualizer
					else if (image.TrackingState == TrackingState.Stopped && visualizer != null)
					{
						m_Visualizers.Remove(image.DatabaseIndex);
						GameObject.Destroy(visualizer.gameObject);
					}
				}
				// Check for spike image and spike cooldown
				else if (image.DatabaseIndex == 1 && canUseSpikes)
				{

					if (image.TrackingState == TrackingState.Tracking && visualizer == null && image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking)
					{
						// Create an anchor to ensure that ARCore keeps tracking this augmented image.
						var anchor = image.CreateAnchor(image.CenterPose);

						DetectedPlane gameDetectedPlane = levelManager.GetGamePlane() as DetectedPlane;

						Vector3 effectPosition = new Vector3(image.CenterPose.position.x, gameDetectedPlane.CenterPose.position.y, image.CenterPose.position.z);

						visualizer = (EffectVisualizer)Instantiate(AugmentedImageVisualizerPrefab, effectPosition, image.CenterPose.rotation);

						visualizer.gameObject.transform.parent = anchor.transform;

						Debug.Log("effect position is: " + effectPosition);

						Debug.Log("image pose is: " + image.CenterPose);

						visualizer.Image = image;

						m_Visualizers.Add(image.DatabaseIndex, visualizer);

						canUseSpikes = false;

						StartCoroutine(SpikesCooldownTimer());
					}
					else if (image.TrackingState == TrackingState.Stopped && visualizer != null)
					{
						m_Visualizers.Remove(image.DatabaseIndex);
						GameObject.Destroy(visualizer.gameObject);
					}
				}
				// Check for ball image and ball cooldown
				else if (image.DatabaseIndex == 2 && canUseBall)
				{

					if (image.TrackingState == TrackingState.Tracking && visualizer == null && image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking)
					{
						// Create an anchor to ensure that ARCore keeps tracking this augmented image.
						var anchor = image.CreateAnchor(image.CenterPose);

						DetectedPlane gameDetectedPlane = levelManager.GetGamePlane() as DetectedPlane;

						Vector3 effectPosition = new Vector3(image.CenterPose.position.x, gameDetectedPlane.CenterPose.position.y, image.CenterPose.position.z);

						visualizer = (EffectVisualizer)Instantiate(AugmentedImageVisualizerPrefab, effectPosition, image.CenterPose.rotation);

						visualizer.gameObject.transform.parent = anchor.transform;

						Debug.Log("effect position is: " + effectPosition);

						Debug.Log("image pose is: " + image.CenterPose);

						visualizer.Image = image;

						m_Visualizers.Add(image.DatabaseIndex, visualizer);

						canUseBall = false;

						StartCoroutine(BallCooldownTimer());
					}
					else if (image.TrackingState == TrackingState.Stopped && visualizer != null)
					{
						m_Visualizers.Remove(image.DatabaseIndex);
						GameObject.Destroy(visualizer.gameObject);
					}
				}
				// Check for fire image and fire cooldown
				else if (image.DatabaseIndex == 3 && canUseFire)
				{

					if (image.TrackingState == TrackingState.Tracking && visualizer == null && image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking)
					{
						// Create an anchor to ensure that ARCore keeps tracking this augmented image.
						var anchor = image.CreateAnchor(image.CenterPose);

						DetectedPlane gameDetectedPlane = levelManager.GetGamePlane() as DetectedPlane;

						Vector3 effectPosition = new Vector3(image.CenterPose.position.x, gameDetectedPlane.CenterPose.position.y, image.CenterPose.position.z);

						visualizer = (EffectVisualizer)Instantiate(AugmentedImageVisualizerPrefab, effectPosition, image.CenterPose.rotation);

						visualizer.gameObject.transform.parent = anchor.transform;

						Debug.Log("effect position is: " + effectPosition);

						Debug.Log("image pose is: " + image.CenterPose);

						visualizer.Image = image;

						m_Visualizers.Add(image.DatabaseIndex, visualizer);

						canUseFire = false;

						StartCoroutine(FireCooldownTimer());

					}
					else if (image.TrackingState == TrackingState.Stopped && visualizer != null)
					{
						m_Visualizers.Remove(image.DatabaseIndex);
						GameObject.Destroy(visualizer.gameObject);
					}
				}
				else
				{
					Debug.Log("image not in database");
				}
			}
		}

	}



	/// <summary>
	/// Shield spell cooldown
	/// </summary>
	/// <returns></returns>
	private IEnumerator ShieldCooldownTimer()
	{
		float time = 0f;
		while (time <= shieldCooldown)
		{
			time += Time.deltaTime;

			// update ui
			shieldCooldownImage.fillAmount = time / shieldCooldown;

			yield return null;
		}
		canUseShield = true;
		Debug.Log("can use shield spell again");
	}

	/// <summary>
	/// Spike spell cooldown
	/// </summary>
	/// <returns></returns>
	private IEnumerator SpikesCooldownTimer()
	{
		float time = 0f;
		while (time <= spikeCooldown)
		{
			time += Time.deltaTime;

			// update ui
			spikeCooldownImage.fillAmount = time / spikeCooldown;

			yield return null;
		}
		canUseSpikes = true;
		Debug.Log("can use spikes spell again");
	}

	/// <summary>
	/// Fire spell cooldown
	/// </summary>
	/// <returns></returns>
	private IEnumerator FireCooldownTimer()
	{
		float time = 0f;
		while (time <= fireCooldown)
		{
			time += Time.deltaTime;

			// update ui
			fireCooldownImage.fillAmount = time / fireCooldown;

			yield return null;
		}
		canUseFire = true;
		Debug.Log("can use fire spell again");
	}

	/// <summary>
	/// Ball spell cooldown
	/// </summary>
	/// <returns></returns>
	private IEnumerator BallCooldownTimer()
	{
		float time = 0f;
		while (time <= ballCooldown)
		{
			time += Time.deltaTime;

			// update ui
			ballCooldownImage.fillAmount = time / ballCooldown;

			yield return null;
		}
		canUseBall = true;
		Debug.Log("can use ball spell again");
	}

}