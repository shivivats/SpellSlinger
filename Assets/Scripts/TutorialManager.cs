using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages scene transitions while in the tutorial scene
/// Functions are called from the OnClick() methods of buttons
/// </summary>
public class TutorialManager : MonoBehaviour
{
    /// <summary>
    /// Loads the main AR scene where the game is played
    /// </summary>
	public void StartGame()
	{

		SceneManager.LoadScene("MilestoneTwo");

	}

    /// <summary>
    /// Loads the Start scene
    /// </summary>
	public void BackButton()
	{

		SceneManager.LoadScene("Start");

	}
}
