using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages scene transitions while in the End scene
/// Functions are called from the OnClick() methods of buttons
/// </summary>
public class EndManager : MonoBehaviour
{

    /// <summary>
    /// Loads the Start scene
    /// </summary>
	public void RestartGame() {
		SceneManager.LoadScene("Start");

	}

    /// <summary>
    /// Quits out of the application
    /// </summary>
	public void QuitGame() {

		Application.Quit();
	}
}
