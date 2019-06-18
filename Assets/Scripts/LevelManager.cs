using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using GoogleARCore.Examples.Common;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The controller class for the plane detection and the game. Based on the HelloARController class by Google
/// </summary>
public class LevelManager : MonoBehaviour
{
	/// <summary>
	/// The first-person camera being used to render the passthrough camera image (i.e. AR
	/// background).
	/// </summary>
	public Camera FirstPersonCamera;

	/// <summary>
	/// A prefab for tracking and visualizing detected planes.
	/// </summary>
	public GameObject DetectedPlanePrefab;

	/// <summary>
	/// The rotation in degrees need to apply to model when the Andy model is placed.
	/// </summary>
	private const float k_ModelRotation = 180.0f;

	/// <summary>
	/// True if the app is in the process of quitting due to an ARCore connection error,
	/// otherwise false.
	/// </summary>
	private bool m_IsQuitting = false;

	/// <summary>
	/// The current gameplane on which the game is played
	/// </summary>
	private Trackable gamePlane;

	/// <summary>
	/// Is the gameplane set or not
	/// </summary>
	public bool gamePlaneSet;

	/// <summary>
	/// The enemy manager object in the scene
	/// </summary>
	public GameObject enemyManager;

	/// <summary>
	/// The hut prefab
	/// </summary>
	public GameObject hut;

	/// <summary>
	/// The hut made in the game
	/// </summary>
	public GameObject madeHut;

	/// <summary>
	/// Duration to wait before extiting the game after hut is destroyed
	/// </summary>
	public float endTimerDuration;

	/// <summary>
	/// The Unity Start Method
	/// </summary>
	private void Start()
	{
		gamePlaneSet = false;

		// if we're in the editor, then instantiate hut at 0,0,0 instead of waiting for user touch in Update() function
		#if UNITY_EDITOR
			madeHut = Instantiate(hut, new Vector3(0f, 0f, 0f), Quaternion.identity);
			madeHut.transform.localScale *= 0.1f;
		#endif

	}

	/// <summary>
	/// The Unity Update() method.
	/// </summary>
	public void Update()
	{
		_UpdateApplicationLifecycle();

		// If the player has not touched the screen, we are done with this update.
		Touch touch;
		if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
		{
			return;
		}

		// Should not handle input if the player is pointing on UI.
		if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
		{
			return;
		}

		// If we havent set the gameplane yet
		if (!gamePlaneSet)
		{

			// Raycast against the location the player touched to search for planes.
			TrackableHit hit;
			TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
				TrackableHitFlags.FeaturePointWithSurfaceNormal;

			if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
			{

				Debug.Log("hit something");

				// Use hit pose and camera pose to check if hittest is from the
				// back of the plane, if it is, no need to create the anchor.
				if ((hit.Trackable is DetectedPlane) &&
					Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
						hit.Pose.rotation * Vector3.up) < 0)
				{
					Debug.Log("Hit at back of the current DetectedPlane");
				}
				else
				{
					// Else, the hit is on the front of the plane
					// Hence we make this plane the main plane of the game

					// Instantiate Hut model at the hit pose.
					madeHut = Instantiate(hut, hit.Pose.position, hit.Pose.rotation);

					// Compensate for the hitPose rotation facing away from the raycast (i.e.
					// camera).
					madeHut.transform.Rotate(0, k_ModelRotation, 0, Space.Self);

					madeHut.transform.localScale *= 0.1f;

					// Create an anchor to allow ARCore to track the hitpoint as understanding of
					// the physical world evolves.
					var anchor = hit.Trackable.CreateAnchor(hit.Pose);

					// Make hut model a child of the anchor.
					madeHut.transform.parent = anchor.transform;

					Debug.Log("hut position: " + madeHut.transform.position);

					Debug.Log("hit a plane");

					gamePlane = hit.Trackable;

					gamePlaneSet = true;

					// Enable enemy manager to start spawning enemies
					enemyManager.GetComponent<EnemyManager>().enabled = true;

				}
			}
		}
	}

	/// <summary>
	/// Return the game plane
	/// </summary>
	/// <returns></returns>
	public Trackable GetGamePlane()
	{
		return gamePlane;
	}


	/// <summary>
	/// Check and update the application lifecycle.
	/// </summary>
	private void _UpdateApplicationLifecycle()
	{
		// Exit the app when the 'back' button is pressed.
		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}

		// Only allow the screen to sleep when not tracking.
		if (Session.Status != SessionStatus.Tracking)
		{
			const int lostTrackingSleepTimeout = 15;
			Screen.sleepTimeout = lostTrackingSleepTimeout;
		}
		else
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
		}

		if (m_IsQuitting)
		{
			return;
		}

		// Quit if ARCore was unable to connect and give Unity some time for the toast to
		// appear.
		if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
		{
			_ShowAndroidToastMessage("Camera permission is needed to run this application.");
			m_IsQuitting = true;
			Invoke("_DoQuit", 0.5f);
		}
		else if (Session.Status.IsError())
		{
			_ShowAndroidToastMessage(
				"ARCore encountered a problem connecting.  Please start the app again.");
			m_IsQuitting = true;
			Invoke("_DoQuit", 0.5f);
		}
	}

	/// <summary>
	/// Actually quit the application.
	/// </summary>
	private void _DoQuit()
	{
		Application.Quit();
	}

	/// <summary>
	/// Show an Android toast message.
	/// </summary>
	/// <param name="message">Message string to show in the toast.</param>
	private void _ShowAndroidToastMessage(string message)
	{
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject unityActivity =
			unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

		if (unityActivity != null)
		{
			AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
			unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
			{
				AndroidJavaObject toastObject =
					toastClass.CallStatic<AndroidJavaObject>(
						"makeText", unityActivity, message, 0);
				toastObject.Call("show");
			}));
		}
	}

	/// <summary>
	/// Handles the game's ending
	/// </summary>
	public void GameEnd()
	{
		GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

		// Stop all enemies in the scene
		foreach (GameObject enemy in allEnemies)
		{

			enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;

		}

		StartCoroutine(EndTimer());

	}

	/// <summary>
	/// Load the end scene after a timer
	/// </summary>
	public IEnumerator EndTimer()
	{
		float time = 0f;

		while (time < endTimerDuration)
		{
			time += Time.deltaTime;

			yield return null;
		}

		SceneManager.LoadScene("End");
	}

}
