using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

/// <summary>
/// Spawns and manages all the enemies
/// </summary>
public class EnemyManager : MonoBehaviour
{
	/// <summary>
	/// The enemy prefab to spawn
	/// </summary>
	public GameObject prefab;

	/// <summary>
	/// The LevelManager of the scene
	/// </summary>
	public GameObject levelManager;

	/// <summary>
	/// The main plane on which the game is played
	/// </summary>
	private Trackable gamePlane;

	/// <summary>
	/// A list of all the enemies in the scene currently
	/// </summary>
	private List<GameObject> enemiesList;

	/// <summary>
	/// UI Text displaying the current wave number
	/// </summary>
	public Text waveText;

	/// <summary>
	/// The current wave number
	/// </summary>
	private int waveNumber;

	/// <summary>
	/// The Unity Start function
	/// </summary>
	void Start()
	{
		// Start the spawning timer for the first batch of enemies
		StartCoroutine(SpawnDelayTimer(5f, 2));

		enemiesList = new List<GameObject>();
		waveNumber = 0;
		UpdateWaveText();
	}

	/// <summary>
	/// Spawns enemies based on the given parameters
	/// </summary>
	/// <param name="maxSpawnDelay">Maximum time delay for next wave spawn, minimum is 1 second</param>
	/// <param name="maxNumberOfEnemies">Maximum number of enemies to spawn, minimum is 1 enemy</param>
	/// <returns></returns>
	IEnumerator SpawnDelayTimer(float maxSpawnDelay, int maxNumberOfEnemies)
	{

		float time = 0f;

		float spawnDelay = Random.Range(1f, maxSpawnDelay);

		int numberOfEnemies = Random.Range(1, maxNumberOfEnemies);

		// Wait for the spawn delay
		while (time < spawnDelay)
		{

			time += Time.deltaTime;
			yield return null;
		}

		gamePlane = levelManager.GetComponent<LevelManager>().GetGamePlane();

		// If game has started and we have a plane, set the hut on the plane as the center
		// and spawn all enemies one by one
		if (gamePlane != null)
		{
			waveNumber++;
			UpdateWaveText();

			DetectedPlane gameDetectedPlane = gamePlane as DetectedPlane;

			Vector3 center = levelManager.GetComponent<LevelManager>().madeHut.transform.position;

			Pose enemyPose;

			for (int i = 0; i < numberOfEnemies; i++)
			{
				Vector3 pos = RandomCircle(center, 10.0f);

				Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);

				enemyPose = new Pose(pos, rot);

				Debug.Log("enemy spawned at " + pos);

				GameObject newEnemy = Instantiate(prefab, pos, rot);

				newEnemy.GetComponent<EnemyMovement>().SetTargetHut(levelManager.GetComponent<LevelManager>().madeHut);

				AddEnemyToList(newEnemy);

#if UNITY_EDITOR
				Debug.Log("Enemy spawned in editor");
#else
			Debug.Log("Enemy spawned in android");
			var enemyAnchor = gameDetectedPlane.CreateAnchor(enemyPose);

			newEnemy.transform.parent = enemyAnchor.transform;
#endif
				newEnemy.GetComponent<EnemyMovement>().SetTargetHut(levelManager.GetComponent<LevelManager>().madeHut);

			}
		}
		
		// Start timer for the next batch of enemies
		StartCoroutine(SpawnDelayTimer(5f, 2));

	}

	/// <summary>
	/// Gives a random point on a circle in the XZ plane with given parameters
	/// </summary>
	/// <param name="center">The center of the circle</param>
	/// <param name="radius">The radius of the circle</param>
	/// <returns></returns>
	Vector3 RandomCircle(Vector3 center, float radius)
	{
		float ang = Random.value * 360;
		Vector3 pos;

		pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
		pos.y = center.y;
		pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		
		return pos;
	}

	/// <summary>
	/// Adds enemies to the list maintained by enemy manager
	/// </summary>
	public void AddEnemyToList(GameObject enemy)
	{
		enemiesList.Add(enemy);
	}

	/// <summary>
	/// Handles destroying an enemy
	/// </summary>
	/// <param name="enemy">The enemy to destroy</param>
	public void DestroyEnemy(GameObject enemy)
	{
		
		// Find the enemy to destroy from the enemies list
		foreach (GameObject currentEnemy in enemiesList)
		{
			if (currentEnemy == enemy)
			{
				enemiesList.Remove(enemy);
				Destroy(enemy);
				Debug.Log("found and destroyed an enemy in the list");
				break;
			}
		}

	}

	/// <summary>
	/// Updates the wave number on the UI
	/// </summary>
	public void UpdateWaveText()
	{
		waveText.text = "Wave: " + waveNumber;
	}

	/// <summary>
	/// Getter function for the current wave number
	/// </summary>
	public int GetCurrentWave()
	{
		return waveNumber;
	}
}
