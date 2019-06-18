using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages scene transitions while in the Start scene
/// Functions are called from the OnClick() methods of buttons
/// </summary>
public class StartManager : MonoBehaviour
{

    /// <summary>
    /// Loads the Tutorial Scene, from which the game can be started
    /// </summary>
	public void TutorialGame()
	{

		SceneManager.LoadScene("TutorialScene");

	}

    /// <summary>
    /// Quits out of the application
    /// </summary>
	public void QuitGame()
	{

		Application.Quit();

	}
}
